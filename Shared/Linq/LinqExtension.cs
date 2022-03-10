using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Linq
{
    public static class LinqExtension
    {
        public static IQueryable<TEntity> Where<TEntity, TField>(
            this IQueryable<TEntity> entities, string fieldName, string methodName, params object[] args)
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity));
            MemberExpression member = Expression.Property(param, fieldName);
            if (member.Member.GetType() != typeof(TField))
                throw new ArgumentException("Type of selected field does not match TEntity");
            MethodInfo method = typeof(TField).GetMethod(methodName)!;
            ConstantExpression[] constants = args.Select(arg => Expression.Constant(arg)).ToArray();
            Expression call = Expression.Call(member, method, constants);
            var whereClause = Expression.Lambda<Func<TEntity, bool>>(call, param).Compile();
            return (IQueryable<TEntity>) entities.Where(whereClause);
        }
    }
}
