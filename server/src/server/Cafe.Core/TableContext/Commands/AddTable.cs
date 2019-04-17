using System;

namespace Cafe.Core.TableContext.Commands
{
    public class AddTable : ICommand
    {
        public Guid Id { get; set; }

        public int Number { get; set; }
    }
}
