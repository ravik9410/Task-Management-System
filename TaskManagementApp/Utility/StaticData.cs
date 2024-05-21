namespace TaskManagementApp.Utility
{
    public static class StaticData
    {
        public enum ApiType
        {
            GET, PUT, POST, DELETE
        }
        public static string JwtToken = "Token";
        public const string Status_Pending = "Pending";
        public const string Status_Completed = "Completed";
        public const string Status_Assigned = "Assigned";
        public const string Status_Cancelled = "Cancelled";
    }
}
