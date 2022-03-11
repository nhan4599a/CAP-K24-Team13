﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Shared.Linq
{
    public static class LinqExtension
    {
        public static IQueryable<TEntity> Where<TEntity, TField>(
            this IQueryable<TEntity> entities, string field, string methodName, params object[] args)
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity));
            var member = param.BuildMemberExpression(field);
            var propertyType = member.Type;
            if (propertyType != typeof(TField))
                throw new ArgumentException(
                    $"Type of {field} is {propertyType.FullName} does not match TField," +
                    $" TField is {typeof(TField).FullName}");
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

        public static IQueryable<TEntity> Where<TEntity>(
            this IQueryable<TEntity> entities, string field, Operator @operator, object arg, Type type)
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity));
            MemberExpression member = param.BuildMemberExpression(field);
            var propertyType = member.Type;
            if (propertyType != type)
                throw new ArgumentException(
                    $"Type of {field} is {propertyType.FullName} does not match provided type," +
                    $" Provided type is {type.FullName}");
            if (arg.GetType() != type)
                throw new ArgumentException(
                    $"Type of arg is {arg.GetType().FullName} does not match provided type," +
                    $" arg type is {type.FullName}");
            ConstantExpression constant = Expression.Constant(arg);
            Expression equalExpression = @operator switch
            {
                Operator.Equal => Expression.Equal(member, constant),
                Operator.LessThan => Expression.LessThan(member, constant),
                Operator.GreaterThan => Expression.GreaterThan(member, constant),
                Operator.LessThanOrEqual => Expression.LessThanOrEqual(member, constant),
                Operator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(member, constant),
                Operator.NotEqual => Expression.NotEqual(member, constant),
                _ => throw new NotImplementedException()
            };
            var whereClause = Expression.Lambda<Func<TEntity, bool>>(equalExpression, param);
            return entities.Where(whereClause);
        }

        private static MemberExpression BuildMemberExpression(this Expression expression, string field)
        {
            if (!IsValidField(field))
                throw new ArgumentException($"\"{field}\" is not a valid field");
            var fieldNames = field.Contains('.') ? new[] { field } : field.Split(".");
            MemberExpression member = Expression.Property(expression, fieldNames[0]);
            for (int i = 1; i < fieldNames.Length; i++)
                member = Expression.Property(member, fieldNames[i]);
            return member;
        }

        public static Type GetExpressionType(this Type type, string field)
        {
            if (!IsValidField(field))
                throw new ArgumentException($"\"{field}\" is not a valid field");
            return Expression.Parameter(type).BuildMemberExpression(field).Type;
        }

        private static bool IsValidField(string field)
        {
            return Regex.IsMatch(field, @"^([[:alpha:]]+\d*\.?)+([[:alpha:]]\d*)+$");
        }
    }

    public enum Operator
    {
        Equal, LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual, NotEqual
    }
}
