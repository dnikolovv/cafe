using System;

namespace Cafe.Domain.Views
{
    public class UserView
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public bool IsManager => ManagerId.HasValue;
        public bool IsWaiter => WaiterId.HasValue;
        public string LastName { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid? WaiterId { get; set; }
    }
}