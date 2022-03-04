﻿using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using System;
using System.IO;
namespace AspNetCoreSharedComponent.HttpContext
{
    public static class HttpContextExtension
    {
        public static bool IsStatisFileRequest(this HttpRequest context)
        {
            return !string.IsNullOrEmpty(Path.GetExtension(context.Path));
        }

        public static T GetAndRemove<T>(this ISession session, string key)
        {
            var type = typeof(T);
            if (!type.IsNumberType() && !(type == typeof(string)))
            {
                throw new NotSupportedException("Provided type is not supported right now");
            }
            var stringValue = session.GetString(key);
            if (stringValue == null)
            {
                throw new NotSupportedException("Provided key have not been set to http context yet");
            }
            T value;
            if (typeof(T).IsNumberType())
            {
                value = (T)typeof(T).GetMethod("Parse")!.Invoke(null, new[]
                {
                    stringValue
                })!;
            }
            else
            {
                value = (T)Convert.ChangeType(stringValue, type);
            }
            session.Remove(key);
            return value;
        }
    }
}