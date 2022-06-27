namespace UrlShortener.Core.Contracts.V1;

public static class ApiRoutes
{
    private const string _root = "api";
    private const string _version = "v{version:apiVersion}";
    private const string _base = $"{_root}/{_version}";

    public static class Account
    {
        private const string _endpoint = $"{_base}/account";

        public const string Login = $"{_endpoint}/login";
        public const string ConfirmEmail = $"{_endpoint}/confirmEmail";
        public const string Register = $"{_endpoint}/register";
        public const string ResendVerificationEmail = $"{_endpoint}/resendVerificationEmail";

        public static class Password
        {
            private const string _endpoint = $"{Account._endpoint}/password";

            public const string SendRecoveryEmail = $"{_endpoint}/recovery";
            public const string VerifyToken = $"{_endpoint}/verifyToken";
            public const string Reset = $"{_endpoint}/reset";

            public const string Change = $"{_endpoint}/change";
        }
    }

    public static class Config
    {
        private const string _endpoint = $"{_base}/config";

        public const string Get = $"{_endpoint}/get";
    }

    public static class CustomUrl
    {
        private const string _endpoint = $"{_base}/customurl";

        public const string Get = $"{_endpoint}/get";
        public const string Save = $"{_endpoint}/save";
    }

    public static class Url
    {
        private const string _endpoint = $"{_base}/url";

        public const string Get = $"{_endpoint}/get";
        public const string Save = $"{_endpoint}/save";
    }
}
