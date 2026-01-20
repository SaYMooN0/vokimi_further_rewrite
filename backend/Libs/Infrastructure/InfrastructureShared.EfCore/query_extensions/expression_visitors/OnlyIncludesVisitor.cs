using System.Linq.Expressions;
using SharedKernel.exceptions;

namespace InfrastructureShared.EfCore.query_extensions.expression_visitors;

public sealed class OnlyIncludesVisitor : ExpressionVisitor
{
    private bool HasForbiddenCalls { get; set; }
    private string? FirstForbiddenMethodName { get; set; }

    protected override Expression VisitMethodCall(MethodCallExpression node) {
        if (HasForbiddenCalls)
            return node;

        string name = node.Method.Name;

        if (name is "Include" or "ThenInclude") {
            return base.VisitMethodCall(node);
        }

        HasForbiddenCalls = true;
        FirstForbiddenMethodName = name;
        return node;
    }

    public static void EnsureOnlyIncludes<T>(
        IQueryable<T> query,
        string caller,
        string callerFilePath,
        int callerLineNumber
    ) {
        var v = new OnlyIncludesVisitor();
        v.Visit(query.Expression);

        if (v.HasForbiddenCalls) {
            UnexpectedBehaviourException.ThrowErr(
                err: ErrFactory.ProgramBug(
                    $"Only Include/ThenInclude calls are allowed here. Forbidden includes: {v.FirstForbiddenMethodName} passed to '{caller}' method. File: {callerFilePath}. Line number: {callerLineNumber}"
                ),
                userMessage: "Something went wrong while processing your request. Please try again late",
                caller: caller
            );
        }
    }
}