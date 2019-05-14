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

        public static class Table
        {
            public const string Add = "add-table";
            public const string GetAll = "get-all-tables";
            public const string CallWaiter = "call-waiter";
            public const string RequestBill = "request-bill";
        }

        public static class Waiter
        {
            public const string GetEmployedWaiters = "get-waiters";
            public const string Hire = "hire-waiter";
            public const string AssignTable = "assign-table";
        }

        public static class Menu
        {
            public const string GetAllMenuItems = "get-menu-items";
            public const string AddMenuItem = "add-menu-item";
        }

        public static class Order
        {
            public const string Get = "get-order";
            public const string Confirm = "confirm-order";
            public const string Complete = "complete-order";
            public const string New = "new-order";
            public const string GetAll = "get-all-orders";
        }
    }
}
