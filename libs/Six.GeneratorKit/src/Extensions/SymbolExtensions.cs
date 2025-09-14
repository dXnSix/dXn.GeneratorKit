namespace Six.GeneratorKit;

/// <summary>
/// Provides extension methods for the ITypeSymbol interface.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class SymbolExtensions
{
  /// <summary>
  /// Checks if the symbol is declared as partial.
  /// </summary>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <returns>True if the type is declared as partial, false otherwise.</returns>
  public static bool IsPartial(this ISymbol symbol)
  {
    foreach(SyntaxReference? syntaxReference in symbol.DeclaringSyntaxReferences)
    {
      SyntaxNode syntaxNode = syntaxReference.GetSyntax(); // Get the syntax node
      if(syntaxNode is TypeDeclarationSyntax typeDecl && typeDecl.Modifiers.Any(SyntaxKind.PartialKeyword))
      {
        return true; // Found a partial declaration
      }
    }

    return false; // No partial declaration found
  }

  /// <summary>
  /// Checks if the symbol is nullable.
  /// </summary>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <returns>True if the type is nullable, false otherwise.</returns>
  public static bool IsNullable(this ISymbol symbol)
  {
    return symbol is INamedTypeSymbol
    {
      IsGenericType: true, ConstructedFrom.SpecialType: SpecialType.System_Nullable_T
    };
  }

  /// <summary>
  /// Gets the access modifier for a given symbol.
  /// </summary>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <returns>The access modifier of the type.</returns>
  public static string GetAccessModifier(this ISymbol symbol)
  {
    return symbol.DeclaredAccessibility.ToString().ToLowerInvariant();
  }
  
  /// <summary>
  /// Gets the access modifier for a given symbol as a nicely formatted string.
  /// </summary>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <returns>The access modifier of the type as a nicely formatted string..</returns>
  public static string GetFormattedAccessModifier(this ISymbol symbol)
  {
    return symbol.DeclaredAccessibility switch
    {
      Accessibility.Public => "public",
      Accessibility.Private => "private",
      Accessibility.Protected => "protected",
      Accessibility.Internal => "internal",
      Accessibility.ProtectedOrInternal => "protected internal",
      Accessibility.ProtectedAndInternal => "protected internal",
      _ => ""
    };
  }

  /// <summary>
  /// Gets the fully qualified name for a given symbol.
  /// </summary>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <returns>The fully qualified name of the type.</returns>
  public static string GetFullyQualifiedName(this ISymbol symbol)
  {
    return symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
  }

  /// <summary>
  /// Gets the fully qualified name for a given symbol, including nullability annotations.
  /// </summary>
  /// <param name="symbol">The input <see cref="ISymbol"/> instance.</param>
  /// <returns>The fully qualified name for <paramref name="symbol"/>.</returns>
  public static string GetFullyQualifiedNameWithNullabilityAnnotations(this ISymbol symbol) =>
    symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.AddMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier));
  
  /// <summary>
  /// Gets the namespace for a given symbol.
  /// </summary>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <returns>The namespace of the type, or an empty string if the type is in the global namespace.</returns>
  public static string GetNamespace(this ISymbol symbol)
  {
    return symbol.ContainingNamespace.IsGlobalNamespace
      ? string.Empty
      : symbol.ContainingNamespace.ToString() ?? string.Empty;
  }
}