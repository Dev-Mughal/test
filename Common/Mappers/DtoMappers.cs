using Common.Features.Auth.SignUp.DTOs;
using Common.Features.Coupon.DTOs;
using Common.Features.Business.DTOs;
using Common.Features.Incentive.Promo.DTOs;
using Common.Features.Incentive.Stamp.DTOs;
using Common.Features.Incentive.GiftCard.DTOs;
using Common.Features.Incentive.Vip.DTOs;
using Common.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace Common.Mappers
{
    public static class DtoMappers
    {

        #region SIGNUP_DTO_MAPPER
        extension(SignUpDto dto)
        {
            public Business ToBusiness(string? businessImageUrl, long stateCityId, long stateCityZipId)
            {
                return new Business()
                {
                    BusinessEmail   = dto.BusinessEmail ?? string.Empty,
                    BusinessName    = dto.BusinessName,
                    BusinessPhone   = dto.BusinessPhone,
                    BusinessURL     = dto.BusinessURL ?? string.Empty,
                    CountryCode     = dto.CountryCode,
                    StreetAddress   = dto.StreetAddress,
                    AddressLine2    = dto.AddressLine2,
                    Country         = dto.Country,
                    Longitude       = dto.Longitude,
                    Latitude        = dto.Latitude,
                    StateCityId     = stateCityId,
                    StateCityZipId  = stateCityZipId,
                    BusinessImageUrl = businessImageUrl,
                    CreatedOn       = DateTime.UtcNow,
                    CategoryId      = dto.CategoryId
                };
            }

            public BusinessUser ToBusinessUser()
            {
                return new BusinessUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    TimeZone = dto.TimeZone,
                    CreatedOn = DateTime.UtcNow,
                    LockoutEnabled = false,
                    RefreshToken = null,
                    RefreshTokenExpiryTime = null,
                };
            }
        }
        #endregion

        #region AUTHORIZED_USER_DTO_MAPPER
        extension(ClaimsPrincipal user)
        {
            public AuthorizedUserDto ToAuthorizedUserDto()
            {
                return new AuthorizedUserDto
                {
                    Id = long.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var sub) ? sub : 0,
                    Email = user.FindFirst(ClaimTypes.Email)?.Value!,
                    FirstName = user.FindFirst(ClaimTypes.GivenName)?.Value!,
                    LastName = user.FindFirst(ClaimTypes.Surname)?.Value!,
                    BusinessId = int.TryParse(user.FindFirst("BusinessId")?.Value, out var bId) ? bId : 0,
                    TimeZone = user.FindFirst("TimeZone")?.Value ?? "UTC"
                };
            }
        }
        #endregion

        #region CREATE_COUPON_MAPPER
        extension(CreateCouponDto dto)
        {
            public Coupon ToCoupon(string? photoUrl, int businessId, string timeZone)
            {
                return new Coupon
                {
                    BusinessId = businessId,
                    Title = dto.Title,
                    Description = dto.Description,
                    PhotoUrl = photoUrl,
                    // QRCode populated in second SaveChangesAsync after PK is known
                    QRCode = string.Empty,
                    StartDateTime = ConvertToUtc(dto.StartDateTime, timeZone),
                    EndDateTime = ConvertToUtc(dto.EndDateTime, timeZone),
                    ExpirationTime = ConvertToUtc(dto.ExpirationTime ?? dto.EndDateTime, timeZone),
                    IsActive = dto.IsActive,
                    IsFeatured = dto.IsFeatured,
                    CreatedOn = DateTime.UtcNow
                };
            }
        }
        #endregion

        #region UPDATE_COUPON_MAPPER
        extension(UpdateCouponDto dto)
        {
            public void ApplyTo(Coupon coupon, string timeZone)
            {
                coupon.Title = dto.Title;
                coupon.Description = dto.Description;
                coupon.StartDateTime = ConvertToUtc(dto.StartDateTime, timeZone);
                coupon.EndDateTime = ConvertToUtc(dto.EndDateTime, timeZone);
                coupon.ExpirationTime = ConvertToUtc(dto.ExpirationTime ?? dto.EndDateTime, timeZone);
                coupon.IsActive = dto.IsActive;
                coupon.IsFeatured = dto.IsFeatured;
            }
        }
        #endregion

        #region COUPON_TO_RESPONSE_MAPPER
        extension(Coupon coupon)
        {
            public CouponResponseDto ToCouponResponseDto()
            {
                return new CouponResponseDto
                (
                    Id: coupon.Id,
                    Title: coupon.Title,
                    Description: coupon.Description,
                    PhotoUrl: coupon.PhotoUrl,
                    TrackCode: coupon.QRCode,  // Send code string for manual entry / scanning
                    QRCodeImageUrl: null,      // Enriched later with IQRCodeService.GenerateQRCodeImageUrl
                    StartDateTime: coupon.StartDateTime,
                    EndDateTime: coupon.EndDateTime,
                    ExpirationTime: coupon.ExpirationTime,
                    IsActive: coupon.IsActive,
                    IsFeatured: coupon.IsFeatured,
                    CreatedOn: coupon.CreatedOn
                );
            }
        }
        #endregion

        #region BUSINESS_TO_CARD_DTO_MAPPER
        extension(Business business)
        {
            public Common.Features.Business.DTOs.BusinessCardDto ToBusinessCardDto()
            {
                return new Common.Features.Business.DTOs.BusinessCardDto(
                    Id: business.BusinessId,
                    Name: business.BusinessName,
                    ImageUrl: business.BusinessImageUrl,
                    Address: business.StreetAddress,
                    AddressLine2: business.AddressLine2,
                    City: business.GeoCity?.City ?? string.Empty,
                    State: business.GeoCity?.State ?? string.Empty,
                    Country: business.Country,
                    ZipCode: business.GeoZipCode?.ZipCode ?? string.Empty,
                    Latitude: business.Latitude,
                    Longitude: business.Longitude,
                    Email: business.BusinessEmail,
                    Phone: business.BusinessPhone,
                    WebsiteUrl: business.BusinessURL,
                    CategoryName: business.Category?.CategoryName ?? "Unknown",
                    CategoryId: business.CategoryId,
                    CreatedOn: business.CreatedOn
                );
            }
        }
        #endregion

        #region CREATE_PROMO_MAPPER
        extension(CreatePromoIncentiveDto dto)
        {
            public PromoBizDef ToPromoBizDef(int businessId)
            {
                return new PromoBizDef
                {
                    BusinessId = businessId,
                    PromotionDesc = dto.PromotionDesc,
                    StartDate = dto.StartDate,
                    ExpirationDate = dto.ExpirationDate,
                    FinePrint = dto.FinePrint,
                    AdminNote = dto.AdminNote,
                    CashierPOSMessage = dto.CashierPOSMessage,
                    VoidedReason = dto.VoidedReason,
                    QRCode = string.Empty
                };
            }
        }
        #endregion

        #region UPDATE_PROMO_MAPPER
        extension(UpdatePromoIncentiveDto dto)
        {
            public void ApplyTo(PromoBizDef promo)
            {
                promo.PromotionDesc = dto.PromotionDesc;
                promo.StartDate = dto.StartDate;
                promo.ExpirationDate = dto.ExpirationDate;
                promo.FinePrint = dto.FinePrint;
                promo.AdminNote = dto.AdminNote;
                promo.CashierPOSMessage = dto.CashierPOSMessage;
                promo.VoidedReason = dto.VoidedReason;
            }
        }
        #endregion

        #region PROMO_TO_RESPONSE_MAPPER
        extension(PromoBizDef promo)
        {
            public PromoIncentiveResponseDto ToPromoIncentiveResponseDto(string? qrCodeImageUrl = null)
            {
                return new PromoIncentiveResponseDto(
                    promo.Id,
                    promo.BusinessId,
                    promo.PromotionDesc,
                    promo.PhotoUrl,
                    promo.QRCode,
                    qrCodeImageUrl,
                    promo.StartDate,
                    promo.ExpirationDate,
                    promo.FinePrint,
                    promo.AdminNote,
                    promo.CashierPOSMessage,
                    promo.VoidedReason);
            }

            public PromoIncentiveListItemDto ToPromoIncentiveListItemDto(string? qrCodeImageUrl = null)
            {
                return new PromoIncentiveListItemDto(
                    promo.Id,
                    promo.PromotionDesc,
                    promo.PhotoUrl,
                    promo.QRCode,
                    qrCodeImageUrl,
                    promo.StartDate,
                    promo.ExpirationDate);
            }
        }
        #endregion

        #region CREATE_STAMP_MAPPER
        extension(CreateStampIncentiveDto dto)
        {
            public StampBizDef ToStampBizDef(int businessId)
            {
                return new StampBizDef
                {
                    BusinessId = businessId,
                    RewardDesc = dto.RewardDesc,
                    StampGoal = dto.StampGoal,
                    GoalReachedMessage = dto.GoalReachedMessage,
                    FinePrint = dto.FinePrint,
                    AdminNote = dto.AdminNote,
                    CashierPOSMessage = dto.CashierPOSMessage,
                    MaxStampPerDay = dto.MaxStampPerDay,
                    QRCode = string.Empty
                };
            }
        }
        #endregion

        #region UPDATE_STAMP_MAPPER
        extension(UpdateStampIncentiveDto dto)
        {
            public void ApplyTo(StampBizDef stamp)
            {
                stamp.RewardDesc = dto.RewardDesc;
                stamp.StampGoal = dto.StampGoal;
                stamp.GoalReachedMessage = dto.GoalReachedMessage;
                stamp.FinePrint = dto.FinePrint;
                stamp.AdminNote = dto.AdminNote;
                stamp.CashierPOSMessage = dto.CashierPOSMessage;
                stamp.MaxStampPerDay = dto.MaxStampPerDay;
            }
        }
        #endregion

        #region STAMP_TO_RESPONSE_MAPPER
        extension(StampBizDef stamp)
        {
            public StampIncentiveResponseDto ToStampIncentiveResponseDto(string? qrCodeImageUrl = null)
            {
                return new StampIncentiveResponseDto(
                    stamp.Id,
                    stamp.BusinessId,
                    stamp.RewardDesc,
                    stamp.PhotoUrl,
                    stamp.QRCode,
                    qrCodeImageUrl,
                    stamp.StampGoal,
                    stamp.GoalReachedMessage,
                    stamp.FinePrint,
                    stamp.AdminNote,
                    stamp.CashierPOSMessage,
                    stamp.MaxStampPerDay);
            }

            public StampIncentiveListItemDto ToStampIncentiveListItemDto(string? qrCodeImageUrl = null)
            {
                return new StampIncentiveListItemDto(
                    stamp.Id,
                    stamp.RewardDesc,
                    stamp.PhotoUrl,
                    stamp.QRCode,
                    qrCodeImageUrl,
                    stamp.StampGoal);
            }
        }
        #endregion

        #region CREATE_GIFTCARD_MAPPER
        extension(CreateGiftCardIncentiveDto dto)
        {
            public GiftCardBizDef ToGiftCardBizDef(int businessId)
            {
                return new GiftCardBizDef
                {
                    BusinessId = businessId,
                    Title = dto.Title,
                    MarketingText = dto.MarketingText,
                    FinePrint = dto.FinePrint,
                    Expiration = dto.Expiration,
                    AdminNote = dto.AdminNote,
                    CashierPOSMessage = dto.CashierPOSMessage,
                    Status = dto.Status,
                    StatusDate = dto.StatusDate,
                    StatusNote = dto.StatusNote,
                    QRCode = string.Empty
                };
            }
        }
        #endregion

        #region UPDATE_GIFTCARD_MAPPER
        extension(UpdateGiftCardIncentiveDto dto)
        {
            public void ApplyTo(GiftCardBizDef giftCard)
            {
                giftCard.Title = dto.Title;
                giftCard.MarketingText = dto.MarketingText;
                giftCard.FinePrint = dto.FinePrint;
                giftCard.Expiration = dto.Expiration;
                giftCard.AdminNote = dto.AdminNote;
                giftCard.CashierPOSMessage = dto.CashierPOSMessage;
                giftCard.Status = dto.Status;
                giftCard.StatusDate = dto.StatusDate;
                giftCard.StatusNote = dto.StatusNote;
            }
        }
        #endregion

        #region GIFTCARD_TO_RESPONSE_MAPPER
        extension(GiftCardBizDef giftCard)
        {
            public GiftCardIncentiveResponseDto ToGiftCardIncentiveResponseDto(string? qrCodeImageUrl = null)
            {
                return new GiftCardIncentiveResponseDto(
                    giftCard.Id,
                    giftCard.BusinessId,
                    giftCard.Title,
                    giftCard.PhotoUrl,
                    giftCard.QRCode,
                    qrCodeImageUrl,
                    giftCard.MarketingText,
                    giftCard.FinePrint,
                    giftCard.Expiration,
                    giftCard.AdminNote,
                    giftCard.CashierPOSMessage,
                    giftCard.Status,
                    giftCard.StatusDate,
                    giftCard.StatusNote);
            }

            public GiftCardIncentiveListItemDto ToGiftCardIncentiveListItemDto(string? qrCodeImageUrl = null)
            {
                return new GiftCardIncentiveListItemDto(
                    giftCard.Id,
                    giftCard.Title,
                    giftCard.PhotoUrl,
                    giftCard.QRCode,
                    qrCodeImageUrl,
                    giftCard.Expiration,
                    giftCard.Status);
            }
        }
        #endregion

        #region CREATE_VIP_MAPPER
        extension(CreateVipIncentiveDto dto)
        {
            public VipBizDef ToVipBizDef(int businessId)
            {
                return new VipBizDef
                {
                    BusinessId = businessId,
                    Description = dto.Description,
                    DesignData = dto.DesignData,
                    FinePrint = dto.FinePrint,
                    DefaultStartDay = dto.DefaultStartDay,
                    DefaultEndDay = dto.DefaultEndDay,
                    DefaultDailyStartHour = dto.DefaultDailyStartHour,
                    DefaultDailyEndHour = dto.DefaultDailyEndHour,
                    Expiration = dto.Expiration,
                    AdminNote = dto.AdminNote,
                    CashierPOSMessage = dto.CashierPOSMessage,
                    QRCode = string.Empty
                };
            }
        }
        #endregion

        #region UPDATE_VIP_MAPPER
        extension(UpdateVipIncentiveDto dto)
        {
            public void ApplyTo(VipBizDef vip)
            {
                vip.Description = dto.Description;
                vip.DesignData = dto.DesignData;
                vip.FinePrint = dto.FinePrint;
                vip.DefaultStartDay = dto.DefaultStartDay;
                vip.DefaultEndDay = dto.DefaultEndDay;
                vip.DefaultDailyStartHour = dto.DefaultDailyStartHour;
                vip.DefaultDailyEndHour = dto.DefaultDailyEndHour;
                vip.Expiration = dto.Expiration;
                vip.AdminNote = dto.AdminNote;
                vip.CashierPOSMessage = dto.CashierPOSMessage;
            }
        }
        #endregion

        #region VIP_TO_RESPONSE_MAPPER
        extension(VipBizDef vip)
        {
            public VipIncentiveResponseDto ToVipIncentiveResponseDto(string? qrCodeImageUrl = null)
            {
                return new VipIncentiveResponseDto(
                    vip.Id,
                    vip.BusinessId,
                    vip.Description,
                    vip.PhotoUrl,
                    vip.QRCode,
                    qrCodeImageUrl,
                    vip.DesignData,
                    vip.FinePrint,
                    vip.DefaultStartDay,
                    vip.DefaultEndDay,
                    vip.DefaultDailyStartHour,
                    vip.DefaultDailyEndHour,
                    vip.Expiration,
                    vip.AdminNote,
                    vip.CashierPOSMessage);
            }

            public VipIncentiveListItemDto ToVipIncentiveListItemDto(string? qrCodeImageUrl = null)
            {
                return new VipIncentiveListItemDto(
                    vip.Id,
                    vip.Description,
                    vip.PhotoUrl,
                    vip.QRCode,
                    qrCodeImageUrl,
                    vip.Expiration);
            }
        }
        #endregion

        private static DateTime ConvertToUtc(DateTime localDateTime, string ianaTimeZone)
        {
            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(ianaTimeZone);
                return TimeZoneInfo.ConvertTimeToUtc(
                    DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified), tz);
            }
            catch (TimeZoneNotFoundException)
            {
                return DateTime.SpecifyKind(localDateTime, DateTimeKind.Utc);
            }
        }
    }
}
