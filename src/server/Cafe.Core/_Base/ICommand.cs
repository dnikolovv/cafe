using Cafe.Domain;
using MediatR;
using Optional;

namespace Cafe.Core
{
    public interface ICommand : IRequest<Option<Unit, Error>>
    {
    }

    public interface ICommand<TResult> : IRequest<Option<TResult, Error>>
    {
    }
}
