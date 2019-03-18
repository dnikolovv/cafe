using Cafe.Domain;
using MediatR;
using Optional;

namespace Cafe.Core.CQRS
{
    public interface ICommand : IRequest<Option<Unit, Error>>
    {
    }

    public interface ICommand<TResult> : IRequest<Option<TResult, Error>>
    {
    }
}
