using Cafe.Domain;
using Cafe.Domain.Views;
using Optional;
using System;

namespace Cafe.Core.AuthContext.Queries
{
    public class GetUser : IQuery<Option<UserView, Error>>
    {
        public Guid Id { get; set; }
    }
}
