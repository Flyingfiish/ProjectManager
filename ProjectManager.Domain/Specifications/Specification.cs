using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications
{
    public class Specification<T> : ISpecification<T>
    {
        private Func<T, bool> _function;

        private Func<T, bool> Function => _function ??= Predicate.Compile();

        protected Expression<Func<T, bool>> Predicate;

        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes { get; set; }

        protected Specification() { }

        public Specification(Expression<Func<T, bool>> predicate)
        {
            Predicate = predicate;
        }

        public bool IsSatisfiedBy(T entity)
        {
            return Function.Invoke(entity);
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            return Predicate;
        }

        public static implicit operator Func<T, bool>(Specification<T> spec)
        {
            return spec.Function;
        }

        public static implicit operator Expression<Func<T, bool>>(Specification<T> spec)
        {
            return spec.Predicate;
        }

        public static bool operator true(Specification<T> spec)
        {
            return false;
        }

        public static bool operator false(Specification<T> spec)
        {
            return false;
        }

        public static Specification<T> operator !(Specification<T> spec)
        {
            return new Specification<T>(
                Expression.Lambda<Func<T, bool>>(
                    Expression.Not(spec.Predicate.Body),
                    spec.Predicate.Parameters));
        }

        public static Specification<T> operator &(Specification<T> left, Specification<T> right)
        {
            var leftExpr = left.Predicate;
            var rightExpr = right.Predicate;
            var leftParam = leftExpr.Parameters[0];
            var rightParam = rightExpr.Parameters[0];

            return new Specification<T>(
                Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(
                        leftExpr.Body,
                        new ParameterReplacer(rightParam, leftParam).Visit(rightExpr.Body)),
                    leftParam));
        }

        public static Specification<T> operator |(Specification<T> left, Specification<T> right)
        {
            var leftExpr = left.Predicate;
            var rightExpr = right.Predicate;
            var leftParam = leftExpr.Parameters[0];
            var rightParam = rightExpr.Parameters[0];

            return new Specification<T>(
                Expression.Lambda<Func<T, bool>>(
                    Expression.OrElse(
                        leftExpr.Body,
                        new ParameterReplacer(rightParam, leftParam).Visit(rightExpr.Body)),
                    leftParam));
        }
    }
}
