namespace Application.Constants
{
    /// <summary>
    /// Type codes for each incentive category as defined in the 00LK1_IncentiveType seed data.
    /// Used to embed the incentive type identity into QR codes (BZP-{code}01-{segment}).
    /// </summary>
    public static class IncentiveTypeCodes
    {
        public const string BizyPopDollars = "B";
        public const string Coupon         = "C";
        public const string Stamp          = "S";
        public const string Promotions     = "M";
        public const string GiftCard       = "G";
        public const string StoreCredit    = "R";
        public const string StorePoint     = "P";
        public const string VIPAccess      = "V";
        public const string CheckIn        = "I";
    }
}
