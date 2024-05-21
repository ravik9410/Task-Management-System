namespace TaskManagementApp.Utility
{
    public static class StaticData
    {
        public enum ApiType
        {
            GET, PUT, POST, DELETE
        }
        public static string AuthUrl = string.Empty;
        public static string TaskDashboardUrl = string.Empty;

        public static string JwtToken = "Token";
        public const string Status_Pending = "Pending";
        public const string Status_Completed = "Completed";
        public const string Status_Assigned = "Assigned";
        public const string Status_Cancelled = "Cancelled";
        public static string RoleAdmin = "ADMIN";
        public static string RoleCustomer = "CUSTOMER";
    }
}
