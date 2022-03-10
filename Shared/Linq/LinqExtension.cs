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
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            if (propertyType != typeof(TField))
                throw new ArgumentException(
                    $"Type of {fieldName} is {propertyType.FullName} does not match TField, TField is {typeof(TField).FullName}");
            Type[] types = args.Select(arg => arg.GetType()).ToArray();
            MethodInfo method = typeof(TField).GetMethod(methodName, types);
            if (method == null)
                throw new ArgumentException(
                    $"Method {methodName} does not found in class {typeof(TField).FullName}" +
                    $" with arguments types [{string.Join(",", types.Select(type => type.FullName))}]"
                );
            ConstantExpression[] constants = args.Select(arg => Expression.Constant(arg)).ToArray();
            Expression call = Expression.Call(member, method, constants);
            var whereClause = Expression.Lambda<Func<TEntity, bool>>(call, param);
            return entities.Where(whereClause);
        }
    }
}
