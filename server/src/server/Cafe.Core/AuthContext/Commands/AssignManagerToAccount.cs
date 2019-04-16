using System;

namespace Cafe.Core.AuthContext.Commands
{
    public class AssignManagerToAccount : ICommand
    {
        public Guid AccountId { get; set; }

        public Guid ManagerId { get; set; }
    }
}
