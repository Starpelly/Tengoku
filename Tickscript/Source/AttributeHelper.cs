using System.Collections.ObjectModel;
using System.Reflection;

namespace Tickscript
{
    public static class AttributeHelper
    {
        public static TAttribute? GetClassAttribute<TTarget, TAttribute>() where TAttribute : Attribute
            => typeof(TTarget).GetAttribute<TAttribute>();

        public static object[]? GetClassAttributeValues<TTarget, TAttribute>() where TAttribute : Attribute
            => typeof(TTarget).GetAttributeValues<TAttribute>();

        public static TAttribute? GetMethodAttribute<TTarget, TAttribute>(string methodName) where TAttribute : Attribute
            => typeof(TTarget).GetMethod(methodName)?.GetAttribute<TAttribute>();

        public static object[]? GetMethodAttributeValues<TTarget, TAttribute>(string methodName) where TAttribute : Attribute
            => typeof(TTarget).GetMethod(methodName)?.GetAttributeValues<TAttribute>();

        private static TAttribute? GetAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
            => memberInfo?.GetCustomAttributes(true).OfType<TAttribute>().FirstOrDefault();

        public static object[]? GetAttributeValues<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
            => memberInfo
                ?.GetCustomAttributesData()
                .FirstOrDefault(d => d.AttributeType == typeof(TAttribute))
                ?.ConstructorArguments
                .Select(argument => argument.Value)
                .SelectMany(a => a is ReadOnlyCollection<CustomAttributeTypedArgument> p ? p.Select(c => c.Value) : new[] { a })
                .ToArray();
    }
}
