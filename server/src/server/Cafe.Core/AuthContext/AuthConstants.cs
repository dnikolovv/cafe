namespace Cafe.Core.AuthContext
{
    public static class AuthConstants
    {
        public static class ClaimTypes
        {
            public const string WaiterId = "waiterId";
            public const string BaristaId = "baristaId";
            public const string ManagerId = "managerId";
            public const string CashierId = "cashierId";
            public const string IsAdmin = "isAdmin";
        }

        public static class Policies
        {
            public const string IsAdminOrWaiter = "IsAdminOrWaiter";
            public const string IsAdminOrBarista = "IsAdminOrBarista";
            public const string IsAdminOrManager = "IsAdminOrManager";
            public const string IsAdminOrCashier = "IsAdminOrCashier";
            public const string IsAdmin = "IsAdmin";
        }
    }
}
