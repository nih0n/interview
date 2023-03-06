using Solution.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Solution.Application
{
    public class Result
    {
        public bool Success { get; }
        public IReadOnlyCollection<INotification> Errors { get; }

        protected Result(bool success, IReadOnlyCollection<INotification> errors)
            => (Success, Errors) = (success, errors);

        public static Result Ok() => new(true, Array.Empty<INotification>());

        public static Result Error(IReadOnlyCollection<INotification> errors) => new(false, errors);

    }

    public class Result<T> : Result
    {
        public T Data { get; }

        private Result(bool success, T data, IReadOnlyCollection<INotification> errors)
            : base(success, errors) => (Data) = (data);

        public static Result<T> Ok(T data) => new(true, data, Array.Empty<INotification>());

        public new static Result<T> Error(IReadOnlyCollection<INotification> errors) => new(false, default, errors);
    }
}
