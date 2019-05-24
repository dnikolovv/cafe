using MediatR;

namespace Cafe.Core
{
    public interface IQueryHandler<in TQuery, TResponse> :
        IRequestHandler<TQuery, TResponse>
           where TQuery : IQuery<TResponse>
    {
    }
}
