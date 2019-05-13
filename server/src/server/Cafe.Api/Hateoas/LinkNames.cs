namespace Cafe.Api.Hateoas
{
    public static class LinkNames
    {
        public const string Self = "self";

        public static class Auth
        {
            public const string Login = "login";
            public const string Register = "register";
            public const string GetCurrentUser = "get-current-user";
            public const string Logout = "logout";
        }

        public static class Tab
        {
            public const string GetTab = "view";
            public const string OrderItems = "order-items";
            public const string ServeItems = "serve-items";
            public const string RejectItems = "reject-items";
            public const string Open = "open-tab";
            public const string Close = "close-tab";
        }

        public static class Waiter
        {
            public const string GetEmployedWaiters = "get-waiters";
            public const string Hire = "hire-waiter";
            public const string AssignTable = "assign-table";
        }
    }
}
