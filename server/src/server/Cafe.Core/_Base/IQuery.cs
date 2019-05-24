using MediatR;

namespace Cafe.Core
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
