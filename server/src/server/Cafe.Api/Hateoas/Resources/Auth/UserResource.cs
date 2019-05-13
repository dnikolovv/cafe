using System;

namespace Cafe.Api.Hateoas.Resources.Auth
{
    public class UserResource : Resource
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // TODO: This definitely seems like a bad pattern
        public Guid? ManagerId { get; set; }
        public Guid? WaiterId { get; set; }
        public Guid? BaristaId { get; set; }
        public Guid? CashierId { get; set; }

        public bool IsManager => ManagerId.HasValue;
        public bool IsWaiter => WaiterId.HasValue;
        public bool IsBarista => BaristaId.HasValue;
        public bool IsCashier => CashierId.HasValue;
    }
}
