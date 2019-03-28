namespace Cafe.Core.AuthContext
{
    public static class AuthConstants
    {
        public static class ClaimTypes
        {
            public const string WaiterId = "waiterId";
            public const string ManagerId = "managerId";
            public const string IsAdmin = "isAdmin";
        }

        public static class Policies
        {
            public const string IsWaiter = "IsWaiter";
            public const string IsManager = "IsManager";
            public const string IsAdmin = "IsAdmin";
        }
    }
}
