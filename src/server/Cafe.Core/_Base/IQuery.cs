using Cafe.Domain;
using MediatR;
using Optional;

namespace Cafe.Core
{
    public interface IQuery<TResponse> : IRequest<Option<TResponse, Error>>
    {
    }
}
