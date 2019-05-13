using Cafe.Core.OrderContext.Commands;
using Cafe.Core.OrderContext.Queries;
using Cafe.Domain.Entities;
using Cafe.Domain.Views;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Tests.Business.OrderContext.Helpers
{
    public class ToGoOrderTestsHelper
    {
        private readonly AppFixture _fixture;

        public ToGoOrderTestsHelper(AppFixture fixture)
        {
            _fixture = fixture;
        }

        public async Task AddBarista(Barista barista)
        {
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.Baristas.Add(barista);
                await dbContext.SaveChangesAsync();
            });
        }

        public async Task AddMenuItems(params MenuItem[] items)
        {
            await _fixture.ExecuteDbContextAsync(async dbContext =>
            {
                dbContext.MenuItems.AddRange(items);
                await dbContext.SaveChangesAsync();
            });
        }

        public async Task CreateConfirmedOrder(Guid orderId, MenuItem[] items)
        {
            await OrderToGo(orderId, items);
            await _fixture.SendAsync(new ConfirmToGoOrder
            {
                OrderId = orderId,
                PricePaid = items.Sum(i => i.Price)
            });
        }

        /// <summary>
        /// Creates a new order. Takes care of adding the menu items prior to sending an OrderToGo command.
        /// </summary>
        public async Task OrderToGo(Guid orderId, MenuItem[] items)
        {
            await AddMenuItems(items);

            var command = new OrderToGo
            {
                Id = orderId,
                ItemNumbers = items.Select(i => i.Number).ToArray()
            };

            await _fixture.SendAsync(command);
        }

        public async Task AssertOrderExists(Guid orderId, Func<ToGoOrderView, bool> predicate)
        {
            var orderView = await _fixture.SendAsync(new GetToGoOrder { Id = orderId });
            orderView.Exists(predicate).ShouldBeTrue();
        }
    }
}
