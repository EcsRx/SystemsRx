using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemsRx.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetMatchingInterfaceGenericTypes(this Type type, Type genericType)
        {
            return type
                .GetInterfaces()
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericType);
        }
    }
}