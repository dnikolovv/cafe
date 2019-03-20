using Cafe.Domain;
using MediatR;
using Optional;

namespace Cafe.Core
{
    public interface IQueryHandler<in TQuery, TResponse> :
        IRequestHandler<TQuery, Option<TResponse, Error>>
           where TQuery : IQuery<TResponse>
    {
    }
}
