using System;

namespace Cafe.Domain.Entities
{
    public class Table
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public Guid? WaiterId { get; set; }

        public Waiter Waiter { get; set; }
    }
}
