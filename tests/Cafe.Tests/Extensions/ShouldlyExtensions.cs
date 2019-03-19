using Cafe.Domain;
using Optional;
using Shouldly;

namespace Cafe.Tests.Extensions
{
    public static class ShouldlyExtensions
    {
        public static void ShouldHaveErrorOfType<T>(this Option<T, Error> option, ErrorType errorType)
        {
            option.HasValue.ShouldBeFalse();
            option.MatchNone(error =>
            {
                error.Type.ShouldBe(errorType);
                error.Messages.ShouldNotBeEmpty();
            });
        }
    }
}
