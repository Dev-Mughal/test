using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    /// <summary>
    /// Thrown when a scanned or submitted incentive code has an invalid format
    /// or references an unrecognised table code.
    /// </summary>
    public class InvalidIncentiveCodeException : AppException
    {
        public override int StatusCode => StatusCodes.Status400BadRequest;

        /// <summary>Unrecognised table code — e.g. a code whose middle segment is not in IncentiveTableCode.</summary>
        public InvalidIncentiveCodeException(string tableCode)
            : base($"Unrecognized incentive table code: '{tableCode}'", "INVALID_INCENTIVE_CODE") { }

        /// <summary>Malformed code — prefix missing, wrong segment count, or non-numeric id.</summary>
        public InvalidIncentiveCodeException(string code, string reason)
            : base($"Invalid incentive code '{code}': {reason}", "INVALID_INCENTIVE_CODE") { }
    }
}
