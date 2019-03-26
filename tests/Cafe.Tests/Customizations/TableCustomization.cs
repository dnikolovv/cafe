using AutoFixture;
using Cafe.Domain.Entities;

namespace Cafe.Tests.Customizations
{
    public class TableCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Table>(composer =>
                composer.With(t => t.Waiter, fixture.Build<Waiter>().Without(w => w.ServedTables).Create()));
        }
    }
}
