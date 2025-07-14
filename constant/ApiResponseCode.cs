namespace lmsApi.Constants.AppStatusCode
{
    public static class AppStatusCode
    {
        // Success Codes
        public const int Success = 200; // OK
        public const int Created = 201; // Resource created successfully
        public const int Accepted = 202; // Request accepted for processing
        public const int NonAuthoritativeInformation = 203; // Non-authoritative information
        public const int NoContent = 204; // No content to return
        public const int ResetContent = 205; // Reset content
        public const int PartialContent = 206; // Partial content

        // Client Error Codes
        public const int BadRequest = 400; // Bad request
        public const int Unauthorized = 401; // Unauthorized access
        public const int Forbidden = 403; // Forbidden
        public const int NotFound = 404; // Resource not found
        public const int AlreadyExists = 409; // Conflict in request

        // Server Error Codes
        public const int InternalServerError = 500; // Internal server error
        public const int NotImplemented = 501; // Not implemented
        public const int BadGateway = 502; // Bad gateway
        public const int ServiceUnavailable = 503; // Service unavailable

        // Custom Application Codes
        public const int ValidationError = 1001; // Validation error
        public const int DatabaseError = 1002; // Database error
        public const int UnknownError = 1003; // Unknown error
    }
}