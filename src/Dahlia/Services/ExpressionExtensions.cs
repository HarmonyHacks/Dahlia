using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Dahlia.Services
{
    public static class IgnoreArgument<T>
    {
        public static T Value { get { return default(T); } }
    }

    public static class ExpressionExtensions
    {
        public static void ForEachParameter(this LambdaExpression methodCallExpression, Action<ParameterInfo, object> handler)
        {
            var methodCall = methodCallExpression.Body as MethodCallExpression;
            if (methodCall == null)
                return;

            var parameterInfos = methodCall.Method.GetParameters();
            for (var index = 0; index < methodCall.Arguments.Count; index++)
            {
                var argument = methodCall.Arguments[index];
                if (IsIgnoreArgumentOfT(argument))
                    continue;
                var paramInfo = parameterInfos[index];
                var argumentValue = Expression.Lambda(argument).Compile().DynamicInvoke();
                handler(paramInfo, argumentValue);
            }
        }

        private static bool IsIgnoreArgumentOfT(Expression argument)
        {
            var expression = argument as MemberExpression;
            if (expression == null)
                return false;

            var declaringType = expression.Member.DeclaringType;
            if(!declaringType.IsGenericType)
                return false;

            return declaringType.GetGenericTypeDefinition() == typeof(IgnoreArgument<>);
        }

        public static string GetMethodName(this LambdaExpression methodCallExpression)
        {
            return methodCallExpression.GetMethodInfo().Name;
        }

        public static MethodInfo GetMethodInfo(this LambdaExpression methodCallExpression)
        {
            var methodCall = methodCallExpression.Body as MethodCallExpression;
            if (methodCall == null)
                throw new InvalidOperationException("Expected a method call expression");

            return methodCall.Method;
        }
    }

}