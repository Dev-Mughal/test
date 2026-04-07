# Incentive Configuration Audit Report

## Executive Summary
I've reviewed all 7 incentive types against the ACCESS_DB_SCHEMA_REFERENCE. Found **several logical issues & discrepancies** that need clarification.

---

## 1?? BizyPop Dollars (TypeCode: B)

### Schema Reference
```
01B_BizDollarUserBalance ? 01C_BizDollarAction
- Balance: int
- CreatedChannel: int (FK ? 01LK1_BizDollerCreatedChannel)
- EntitlementId, BusinessId, Amount, CashierId, TransactionDate, UserId
```

### Current Implementation ?
- **BizDollarUserBalance.cs**: ? Correct (Id, UserId, Balance, CreatedChannel, LastUpdated, Created)
- **BizDollarAction.cs**: ? Correct (EntitlementId, BusinessId, Amount, CashierId, TransactionDate, UserId)
- **Configuration**: ? Proper FK relationships

### ?? **ISSUE: Missing CreatedChannel Lookup Table**
- Schema references `01LK1_BizDollerCreatedChannel` (lookup table)
- Current implementation: `CreatedChannel` is stored as **int** (no entity for the lookup)
- **Question 1**: Should we create a `BizDollarCreatedChannel` lookup entity? Or keep it as a simple enum/int?

---

## 2?? Coupon (TypeCode: C)

### Schema Reference
```
10A_Coupon_BizDef ? 10B_CouponUserEnt
- Status: FK ? L02_EntStatus
- Various date/time fields
- UserRedeemLimit, UserRedeemMaxPerDay (FUTURE SCOPE)
```

### Current Implementation ?
- **Coupon.cs**: ? Exists (Title, Description, PhotoUrl, StartDateTime, EndDateTime, ExpirationTime, IsActive, IsFeatured)
- **CustomerCoupon.cs**: ? Maps to 10B_CouponUserEnt (Status, DateRedeemed, etc.)
- **No issues detected**

---

## 3?? Promotions (TypeCode: M)

### Schema Reference
```
11A_PromoBizDef ? 11B_PromoUserUsage
- Columns: ID, BusinessID, Promotion Desc, FinePrint, StartDate, ExpirationDate, AdminNote, CashierPOSMessage, VoidedReason
- PromoUserUsage: ID, UserId, PromotionID, LastUpdated, Created, UsedDate
```

### Current Implementation ?
- **PromoBizDef.cs**: ? All columns present (PromotionDesc, QRCode, PhotoUrl, FinePrint, StartDate, ExpirationDate, AdminNote, CashierPOSMessage, VoidedReason)
- **PromoUserUsage.cs**: ? All columns present (UserId, PromotionId, QRCode, LastUpdated, Created, UsedDate)
- **Configuration**: ? Proper setup
- **No issues detected**

---

## 4?? Stamp Cards (TypeCode: S)

### Schema Reference
```
12A_StampBizDef ? 12B_StampUserEnt ? 12C_StampAction + 12V_StampVoidLog
- StampBizDef: ID, BusinessID, RewardDesc, StampGoal, StampCount, GoalReachedMessage, FinePrint, AdminNote, CashierPOSMessage, MaxStampPerDay
- StampUserEnt: Status, StampCount, StampGoal (DUPLICATION with A-table)
```

### Current Implementation ?
- **StampBizDef.cs**: ? All columns (StampGoal, StampCount, GoalReachedMessage, FinePrint, MaxStampPerDay, etc.)
- **StampUserEnt.cs**: ? All columns (Status, StampCount, StampGoal, RedeemedDate, CashierNote, etc.)
- **StampAction.cs**: ? Present (EntitlementId, CashierId, TransactionDate, Note, IsVoided)
- **StampVoidLog.cs**: ? Present (EntitlementId [mapped from "EntitlmentID" - typo preserved], Reason, CashierId)

### ?? **ISSUE: Column Name Typo in Database**
- Schema has: `EntitlmentID` (missing 'e')
- Code properly maps it: `HasColumnName("EntitlmentID")`
- **Question 2**: Is this typo intentional/preserved from Access DB, or should we fix it?

### ?? **ISSUE: StampCount & StampGoal Duplication**
- **StampBizDef** has: `StampCount`, `StampGoal`
- **StampUserEnt** also has: `StampCount`, `StampGoal`
- **Question 3**: 
  - Does `StampBizDef.StampCount` represent the **global** stamp count across all users?
  - Does `StampUserEnt.StampCount` represent the **per-user** count for this specific stamp card?
  - If so, should we rename them for clarity (e.g., `GlobalStampCount` vs `UserStampCount`)?

---

## 5?? Gift Cards (TypeCode: G)

### Schema Reference
```
20A_GiftCardBizDef ? 20B_GiftCardUserEnt ? 20C_GiftCardAction + 20T_GiftCardTransfer
- GiftCardBizDef: GiftCardValue, Status (FK ? L02_EntStatus)
- GiftCardUserEnt: GiftCardBalance, GiftCardValue (DUPLICATION with A-table)
- GiftCardAction: Amount, TransferRecieverUserID
- GiftCardTransfer: SenderEntitlementID, RecieverEntitlementID
```

### Current Implementation ?
- **GiftCardBizDef.cs**: ? Has GiftCardValue, Status (with IncentiveEntitlementStatus enum)
- **GiftCardUserEnt.cs**: ? Has GiftCardBalance
- **GiftCardAction.cs**: ? Has Amount, TransferReceiverUserId

### ?? **ISSUE: GiftCardValue Not in GiftCardUserEnt**
- **Schema says**: `20B_GiftCardUserEnt` should have `GiftCardValue` column
- **Current code**: `GiftCardUserEnt.cs` **DOES NOT** have this column (only `GiftCardBalance`)
- **Question 4**: 
  - Should `GiftCardValue` be on the **B-table** (entitlement), or just on the **A-table** (definition)?
  - Is `GiftCardValue` the fixed value of the gift card (A-table only), while `GiftCardBalance` is the remaining balance (B-table)?
  - If so, the schema reference is **misleading/incorrect**

### ?? **MINOR: Column Spelling "Recieve" ? "Receive"**
- Schema uses: `TransferRecieverUserID`, `RecieverEntitlementID`
- Code uses proper spelling: `TransferReceiverUserId`, `ReceiverEntitlementId`
- **Question 5**: Should we preserve the original Access DB spelling for column names (`HasColumnName("TransferRecieverUserID")`)?

---

## 6?? Store Credit (TypeCode: R)

### Schema Reference
```
21A_StoreCreditBizDef ? 21B_StoreCreditUserEnt ? BP_LogStoreCredit
- StoreCreditBizDef: ID, BusinessID, AdminNote, CashierPOSMessage
- StoreCreditUserEnt: ID, UserId, StoreCredID, StoreCreditBalance, CashierNote, LastUpdated, Created
- StoreCreditAction (BP_LogStoreCredit): EntitlementID, TransAmount, CashierID, TransDate, Note, ReasonID (FK ? 21LK1_StoreCreditReason)
```

### Current Implementation ?
- **StoreCreditBizDef.cs**: ? Present (QRCode, PhotoUrl, AdminNote, CashierPOSMessage)
- **StoreCreditUserEnt.cs**: ? Present (UserId, StoreCredId, StoreCreditBalance, CashierNote, etc.)
- **StoreCreditAction.cs**: ? Present (EntitlementId, TransAmount, CashierId, TransDate, Note, ReasonId)

### ?? **ISSUE: Missing StoreCreditReason Lookup Table**
- Schema references: `21LK1_StoreCreditReason` (lookup with Reason Description)
- Current implementation: `ReasonId` is stored as **int?** (no entity for the lookup)
- **Question 6**: Should we create a `StoreCreditReason` lookup entity? Seed data:
  - 1 = Customer Service Issue
  - 2 = Quality Issue
  - 3 = Friend
  - 4 = Family

---

## 7?? Store Points (TypeCode: P)

### Schema Reference
```
22A_StorePointsBizDef ? 22B_StorePointUserEnt ? 22C_StorePointAction + 22T_StorePointTransfer
- StorePointsBizDef: DollarPointRatio, Status (FK ? L02_EntStatus)
- StorePointUserEnt: StorePointTotal
- StorePointAction: PointAmount, IsTransfer
- StorePointTransfer: SenderEntitlementID, ReceiverEntitlementID
```

### Current Implementation ?
- **StorePointsBizDef.cs**: ? All columns (Description, DollarPointRatio, Status, StatusDate, StatusNote, etc.)
- **StorePointUserEnt.cs**: ? Present (UserId, StorePointId, StorePointTotal, CashierNote, etc.)
- **StorePointAction.cs**: ? Present (EntitlementId, PointAmount, IsTransfer, etc.)
- **StorePointTransfer.cs**: ? Should exist (SenderEntitlementId, ReceiverEntitlementId, Reason, CashierId)

### ?? **ISSUE: StorePointTransfer Missing from Files**
- **Question 7**: Does `StorePointTransfer` entity exist? I didn't see it in the file list.

---

## 8?? VIP Access (TypeCode: V)

### Schema Reference
```
30A_VIPBizDef ? 30B_VIPUserEnt ? 30C_VipAction + 30T_VipTransfer
- VIPBizDef: DefaultStartDay, DefaultEndDay, DefaultDailyStartHour, DefaultDailyEndHour
- VIPUserEnt: Status, StartDay, EndDay, DailyStartHour, DailyEndHour (per-user overrides)
- VipAction: TransferRecieverUserID, IsValid
- VipTransfer: SenderEntitlementID, ReceiverEntitlementID
```

### Current Implementation ?
- **VipBizDef.cs**: ? All defaults (DefaultStartDay, DefaultEndDay, DefaultDailyStartHour, DefaultDailyEndHour, etc.)
- **VipUserEnt.cs**: ? All overrides (StartDay, EndDay, DailyStartHour, DailyEndHour, Status, etc.)
- **VipAction.cs**: ? Present (EntitlementId, TransferReceiverUserId, IsValid, etc.)
- **VipTransfer.cs**: ? Present (SenderEntitlementId, ReceiverEntitlementId, Reason, CashierId)

### ?? **MINOR: VIP_BizDef Link Issue**
- **Schema note**: "Linked to the business directly (not to VipBizDef)"
- **Current code**: `VipUserEnt` has both `BusinessId` AND navigation to `Business` (correct!)
- **No issues detected**

---

## Summary of Questions for Clarification

| # | Incentive | Issue | Question |
|---|-----------|-------|----------|
| 1 | BizyPop Dollars | Lookup Table | Create `BizDollarCreatedChannel` entity or keep as int? |
| 2 | Stamps | Typo | Preserve "EntitlmentID" typo in column name or fix to "EntitlementID"? |
| 3 | Stamps | Duplication | Are `StampCount` and `StampGoal` meant to be **global** (A) vs **per-user** (B)? Should we rename for clarity? |
| 4 | Gift Cards | Schema Mismatch | Should `GiftCardValue` be on B-table (entitlement) or only on A-table (definition)? |
| 5 | Gift Cards | Spelling | Use original "Recieve" spelling or corrected "Receive"? |
| 6 | Store Credit | Lookup Table | Create `StoreCreditReason` lookup entity with seed data? |
| 7 | Store Points | Missing Entity | Does `StorePointTransfer` entity exist? |
| 8 | All | General | Missing unique index on `Customer.Email` — should we add? |

---

## Recommendations (Pending Your Answers)

1. **Do NOT** modify existing column names that preserve Access DB spelling (e.g., "EntitlmentID", "TransferRecieverUserID") — maintain data integrity with legacy system
2. **Create lookup tables** for: `BizDollarCreatedChannel`, `StoreCreditReason` (if not using simple enums)
3. **Clarify B-table vs A-table** property duplication (especially Stamps, Gift Cards)
4. **Add missing entities/configurations** once above is clarified

---

Please address these questions so I can verify/update the configurations.
