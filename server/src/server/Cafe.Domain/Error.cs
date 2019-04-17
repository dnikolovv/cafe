using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.Domain
{
    public readonly struct Error
    {
        private Error(ErrorType errorType, IEnumerable<string> messages)
            : this(errorType, messages.ToArray())
        {
        }

        private Error(ErrorType errorType, params string[] messages)
        {
            Type = errorType;
            Date = DateTime.Now;
            Messages = messages;
        }

        public DateTime Date { get; }
        public IReadOnlyList<string> Messages { get; }

        [JsonIgnore]
        public ErrorType Type { get; }

        public static Error Validation(string error) =>
            new Error(ErrorType.Validation, error);

        public static Error Validation(IEnumerable<string> errors) =>
            new Error(ErrorType.Validation, errors);

        public static Error Unauthorized(string error) =>
            new Error(ErrorType.Unauthorized, error);

        public static Error Critical(string error) =>
            new Error(ErrorType.Critical, error);

        public static Error Conflict(string error) =>
            new Error(ErrorType.Conflict, error);

        public static Error NotFound(string error) =>
            new Error(ErrorType.NotFound, error);

        public static Error NotFound(IEnumerable<string> errors) =>
            new Error(ErrorType.NotFound, errors);
    }
}