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
                error.Type.ShouldBe(
                    errorType,
                    $"Expected an error of type {errorType.ToString()}, but got {error.Type.ToString()}: {string.Join(", ", error.Messages)}");
                error.Messages.ShouldNotBeEmpty();
            });
        }
    }
}
