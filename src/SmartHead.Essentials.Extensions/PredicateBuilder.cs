using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SmartHead.Essentials.Extensions
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>()
            => param => true;

        public static Expression<Func<T, bool>> False<T>()
            => param => false;

        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate)
            => predicate;

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
            => first.Compose<Func<T, bool>>(second, Expression.AndAlso);

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
            => first.Compose<Func<T, bool>>(second, Expression.OrElse);

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
            => Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters);

        public static Expression<T> Compose<T>(this LambdaExpression first, LambdaExpression second,
            Func<Expression, Expression, Expression> merge)
            => Expression.Lambda<T>(merge(first.Body, ParameterRebinder.ReplaceParameters(first.Parameters
                .Select((f, i) => new {f, s = second.Parameters[i]})
                .ToDictionary(p => p.s, p => p.f), second.Body)), first.Parameters);

        private class ParameterRebinder : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

            private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
                Expression exp)
                => new ParameterRebinder(map).Visit(exp);

            protected override Expression VisitParameter(ParameterExpression p)
            {
                if (_map.TryGetValue(p, out var replacement)) p = replacement;

                return base.VisitParameter(p);
            }
        }
    }
}