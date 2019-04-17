using Cafe.Domain.Views;
using System;

namespace Cafe.Core.AuthContext.Queries
{
    public class GetUser : IQuery<UserView>
    {
        public Guid Id { get; set; }
    }
}
