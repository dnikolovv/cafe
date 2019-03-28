using Cafe.Domain.Views;
using System.Collections.Generic;

namespace Cafe.Core.AuthContext.Queries
{
    public class GetAllUserAccounts : IQuery<IEnumerable<UserView>>
    {
    }
}
