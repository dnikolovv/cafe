using System;

namespace Cafe.Core.AuthContext.Commands
{
    public class AssignBaristaToAccount : ICommand
    {
        public Guid AccountId { get; set; }

        public Guid BaristaId { get; set; }
    }
}
