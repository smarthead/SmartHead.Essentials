using System;
using System.Linq.Expressions;
using SmartHead.Essentials.Extensions;

namespace SmartHead.Essentials.Abstractions.Specifications
{
    public static class SpecExtenions
    {
        public static Spec<T> ToSpec<T>(this Expression<Func<T, bool>> expr)
            where T : class
            => new Spec<T>(expr);

        public static bool Satisfy<T>(this T obj, Func<T, bool> spec)
            => spec(obj);
        
        public static bool SatisfyExpresion<T>(this T obj, Expression<Func<T, bool>> spec)
            => spec.AsFunc()(obj);

        public static bool IsSatisfiedBy<T>(this Func<T, bool> spec, T obj)
            => spec(obj);

        public static bool IsSatisfiedBy<T>(this Expression<Func<T, bool>> spec, T obj)
            => spec.AsFunc()(obj);
    }
}