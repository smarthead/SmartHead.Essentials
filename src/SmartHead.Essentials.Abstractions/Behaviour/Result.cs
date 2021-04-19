using System;

namespace SmartHead.Essentials.Abstractions.Behaviour
{
    public class Result
    {
        public static Result Success() => new Result(true);

        public static Result Fail() => new Result(false);

        public static Result Fail(Exception exception) => new Result(exception);

        public static Result Fail(Failure failure) => new Result(failure);

        public static Result Fail(Failure[] failures) => new Result(failures);

        protected Result(Exception exception)
        {
            ExplicitSuccess = false;
            Failure = new Failure(exception);
        }

        protected Result(Failure failure)
        {
            ExplicitSuccess = false;
            Failure = failure;
        }

        protected Result(Failure[] failures)
        {
            ExplicitSuccess = false;
            Failure = new Failure(failures);
        }

        protected Result(bool success)
        {
            ExplicitSuccess = success;
        }

        public Failure Failure { get; protected set; }

        public static implicit operator bool(Result r) => r.IsSuccess;

        public bool IsSuccess => Failure == null && ExplicitSuccess;

        public bool ExplicitSuccess { get; protected set; }
    }

    public class Result<T> : Result
    {
        public static Result<T> Success(T payload) => new Result<T>(payload);

        public new static Result<T> Success() => new Result<T>(true);

        public new static Result<T> Fail() => new Result<T>(false);

        public new static Result<T> Fail(Exception exception) => new Result<T>(exception);

        public new static Result<T> Fail(Failure failure) => new Result<T>(failure);

        public new static Result<T> Fail(Failure[] failures) => new Result<T>(failures);

        protected Result(T payload) : base(true)
        {
            Payload = payload;
        }

        protected Result(Exception exception) : base(exception)
        {
            Payload = default;
        }

        protected Result(Failure failure) : base(failure)
        {
            Payload = default;
        }

        protected Result(Failure[] failures): base(failures)
        {
            Payload = default;
        }

        protected Result(bool success) : base(success)
        {
            Payload = default;
        }

        public T Payload { get; set; }
    }
}