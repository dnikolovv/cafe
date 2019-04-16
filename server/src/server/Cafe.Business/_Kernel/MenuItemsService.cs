using Cafe.Core;
using Cafe.Domain;
using Cafe.Domain.Entities;
using Cafe.Persistance.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.Business
{
    public class MenuItemsService : IMenuItemsService
    {
        private readonly ApplicationDbContext _dbContext;

        public MenuItemsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Option<IList<MenuItem>, Error>> ItemsShouldExist(IList<int> menuItemNumbers)
        {
            var allMenuItems = (await _dbContext
                .MenuItems
                .ToListAsync())

                // Purposefully not using a lookup to avoid possible unique key exceptions
                // since we should never have items with conflicting menu numbers
                .ToDictionary(x => x.Number);

            var itemsToServeResult = menuItemNumbers
                .Select(menuNumber => allMenuItems
                    .GetValueOrNone(menuNumber)
                    .WithException(Error.NotFound($"Menu item with number {menuNumber} was not found.")))
                .ToList();

            var errors = itemsToServeResult
                .Exceptions()
                .SelectMany(e => e.Messages)
                .ToArray();

            return errors.Any() ?
                Option.None<IList<MenuItem>, Error>(Error.NotFound(errors)) :
                itemsToServeResult.Values().ToList().Some<IList<MenuItem>, Error>();
        }
    }
}
