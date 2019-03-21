using System;

namespace Cafe.Domain.Entities
{
    public class MenuItem
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }
    }
}
