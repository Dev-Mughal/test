namespace Domain
{
    /// <summary>
    /// Entitlement status codes shared across all incentive wallet entries.
    /// Maps to the <c>L02_EntStatus</c> lookup table in the Access DB reference.
    /// </summary>
    public enum IncentiveEntitlementStatus
    {
        Accumulating  = 0,
        ReadyToUse    = 1,
        Used          = 2,
        Inactive      = 3,
        Request       = 4,
        Discontinued  = 8,
        Voided        = 9
    }
}
