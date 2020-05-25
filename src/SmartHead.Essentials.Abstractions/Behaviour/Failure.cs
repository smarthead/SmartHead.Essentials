using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHead.Essentials.Abstractions.Behaviour
{
    public class Failure
    {
        public Failure(params Failure[] failures)
        {
            if (!failures.Any())
            {
                throw new ArgumentException(nameof(failures));
            }

            Message = string.Join(Environment.NewLine, failures.Select(x => x.Message));
            Data = failures.Select(x => x.Data).ToList().AsReadOnly();
        }

        public Failure(string message)
        {
            Message = message;
            Data = new List<object> { message }.AsReadOnly();
        }

        public Failure(Exception exception)
        {
            Message = exception.Message;
            Data = new List<object> { exception }.AsReadOnly();
        }

        public Failure(string message, IReadOnlyList<object> data)
        {
            Message = message;
            Data = data;
        }

        public string Message { get; }

        public IReadOnlyList<object> Data { get; protected set; }
    }
}
