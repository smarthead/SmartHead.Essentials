using System;
using System.Linq.Expressions;
using SmartHead.Essentials.Extensions;

namespace SmartHead.Essentials.Abstractions.Specifications
{
    public class Spec<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public Spec(Expression<Func<T, bool>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public static bool operator false(Spec<T> spec) 
            => false;

        public static bool operator true(Spec<T> spec)
            => false;

        public static Spec<T> operator &(Spec<T> spec1, Spec<T> spec2)
            => new Spec<T>(spec1._expression.And(spec2._expression));

        public static Spec<T> operator |(Spec<T> spec1, Spec<T> spec2)
            => new Spec<T>(spec1._expression.Or(spec2._expression));

        public static Spec<T> operator !(Spec<T> spec)
            => new Spec<T>(spec._expression.Not());

        public static implicit operator Expression<Func<T, bool>>(Spec<T> spec)
            => spec._expression;

        public static implicit operator Spec<T>(Expression<Func<T, bool>> expression)
            => new Spec<T>(expression);

        public bool IsSatisfiedBy(T obj) 
            => _expression.AsFunc()(obj);

        public Spec<TParent> From<TParent>(Expression<Func<TParent, T>> mapFrom)
            => _expression.From(mapFrom);
    }
}