using Moq;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cafe.Tests.Extensions
{
    public static class MoqExtensions
    {
        /// <summary>
        /// Periodically invokes Mock.Verify until a specified timeout is reached.
        /// </summary>
        /// <typeparam name="T">The mock type.</typeparam>
        /// <param name="mock">The mock.</param>
        /// <param name="expression">Verification expression.</param>
        /// <param name="times">Times the method should have been called.</param>
        /// <param name="timeoutInMs">Timeout in milliseconds.</param>
        public static async Task VerifyWithTimeoutAsync<T>(this Mock<T> mock, Expression<Action<T>> expression, Times times, int timeoutInMs)
            where T : class
        {
            bool hasBeenExecuted = false;
            bool hasTimedOut = false;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!hasBeenExecuted && !hasTimedOut)
            {
                if (stopwatch.ElapsedMilliseconds > timeoutInMs)
                {
                    hasTimedOut = true;
                }

                try
                {
                    mock.Verify(expression, times);
                    hasBeenExecuted = true;
                }
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
                catch (Exception)
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
                {
                }

                // Feel free to make this configurable
                await Task.Delay(20);
            }

            mock.Verify(expression, times);
        }
    }
}
