using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.Utilities;

internal static partial class InternalLogMessages
{
    [LoggerMessage(LogLevel.Error, "{caller}: Invalid ExpressionType {exprType} (called by {parentCaller})")]
    internal static partial void InvalidExpression(
        this ILogger logger,
        ExpressionType exprType,
        string parentCaller,
        [ CallerMemberName ] string caller = ""
    );

    [LoggerMessage(LogLevel.Error, "{caller}: Property {propName} does not have a set method (called by {parentCaller})")]
    internal static partial void MissingSetMethod(
        this ILogger logger,
        string propName,
        string parentCaller,
        [CallerMemberName] string caller = ""
    );

    [LoggerMessage(LogLevel.Error, "{caller}: Empty PropertyInfo path collection for expression '{propExpr}'")]
    internal static partial void EmptyPropertyInfoPath(
        this ILogger logger,
        string propExpr,
        [ CallerMemberName ] string caller = ""
    );

    [LoggerMessage(LogLevel.Error, "{caller}: Property {propName} in expression '{propExpr}' has no declaring type")]
    internal static partial void UndefinedDeclaringType(
        this ILogger logger,
        string propName,
        string propExpr,
        [ CallerMemberName ] string caller = ""
    );

    [LoggerMessage(LogLevel.Error, "{caller}: Property {propName} in expression '{propExpr}' has no set method")]
    internal static partial void UndefinedSetMethod(
        this ILogger logger,
        string propName,
        string propExpr,
        [CallerMemberName] string caller = ""
    );
}
