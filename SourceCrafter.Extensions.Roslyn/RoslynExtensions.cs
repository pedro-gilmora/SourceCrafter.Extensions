using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceCrafter;

public static class RoslynExtensions
{
    public static bool IsNullable(this ITypeSymbol typeSymbol) =>
        typeSymbol is { NullableAnnotation: NullableAnnotation.Annotated } or INamedTypeSymbol { Name: "Nullable" };

    public static bool AllowsNull(this ITypeSymbol typeSymbol) =>
        typeSymbol is
        {
            IsValueType: false,
            IsTupleType: false
        } || typeSymbol.IsNullable();


    public static bool TryGetNameOfFromAttributeArg<TSymbol>(AttributeData attr, SemanticModel model, int index, out TSymbol symbol, Func<ISymbol, bool>? predicate = null)
        where TSymbol : class, ISymbol
    {
        return null != (symbol = attr.ApplicationSyntaxReference?.GetSyntax() is AttributeSyntax
        {
            ArgumentList.Arguments: { Count: { } count } args
        } && count > index && // It's inside the range
        args[index].Expression is InvocationExpressionSyntax
        {
            Expression: SimpleNameSyntax { Identifier.Text: "nameof" }, // Is nameof
            ArgumentList.Arguments: [{ Expression: { } expr }] // Has just one argument
        }
                ? model.GetSymbolInfo(expr) switch // Contains symbol
                {
                    { Symbol: TSymbol foundSymbol } => foundSymbol, // Directly found
                    {
                        CandidateSymbols: { Length: > 0 } symbols,
                        CandidateReason: CandidateReason.Ambiguous | CandidateReason.MemberGroup
                    } =>
                      (symbols.FirstOrDefault(predicate ?? IsTSymbol) as TSymbol)!, // The first one meeting the requirements
                    _ => default!
                }
                : default!);

        static bool IsTSymbol(ISymbol symbol) => symbol is TSymbol;
    }

}
