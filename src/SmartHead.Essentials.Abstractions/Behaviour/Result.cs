using System;

namespace SmartHead.Essentials.Abstractions.Behaviour
{
    public class Result<T>
    {
        public Result(Exception exception)
        {
            Exception = exception;
        }

        public Result(T payload)
        {
            ExplicitSuccess = true;
            Payload = payload;
        }

        public Result(ResultType resultType)
        {
            ExplicitSuccess = resultType == ResultType.Success;
            Payload = default;
        }

        public Exception Exception { get; }

        public T Payload { get; set; }

        public bool IsSuccess => Exception == null && ExplicitSuccess;

        public bool ExplicitSuccess { get; }

        public static implicit operator bool(Result<T> r) => r.IsSuccess;
    }
}