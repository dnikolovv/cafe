using AutoFixture;
using Cafe.Domain.Entities;
using System.Linq;

namespace Cafe.Tests.Customizations
{
    public class WaiterCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Waiter>(composer => composer
                .FromFactory(() =>
                {
                    var waiter = fixture
                        .Build<Waiter>()
                        .Without(w => w.ServedTables)
                        .Create();

                    waiter.ServedTables = Enumerable
                        .Range(1, 3)
                        .Select(_ => CreateTableForWaiter(waiter, fixture))
                        .ToList();

                    return waiter;
                }));
        }

        private static Table CreateTableForWaiter(Waiter waiter, IFixture fixture) =>
            fixture
                .Build<Table>()
                .With(w => w.Id, waiter.Id)
                .With(w => w.Waiter, waiter)
                .Create();
    }
}
