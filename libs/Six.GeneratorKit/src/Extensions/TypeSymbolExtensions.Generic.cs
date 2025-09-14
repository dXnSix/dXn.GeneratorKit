using System.Linq;

namespace Six.GeneratorKit;

public static partial class TypeSymbolExtensions
{
  /// <summary>
  /// Checks if <see cref="ITypeSymbol"/> is a generic type.
  /// </summary>
  /// <param name="typeSymbol"><see cref="ITypeSymbol"/> to check if it is generic.</param>
  /// <returns>Returns true if symbol is generic.</returns>
  public static bool IsGenericType(this ITypeSymbol typeSymbol)
  {
    return typeSymbol is INamedTypeSymbol { IsGenericType: true };
  }
  
  /// <summary>
  /// <para>
  /// Gets the generic parameters string for the type including nested generic types.
  /// </para>
  /// <para>
  /// The generic parameters will be rendered as fully qualified type names with <c>global::</c>
  /// prefix for non-primitive types.
  /// </para>
  /// </summary>
  /// <remarks>
  /// This recursive approach will handle nested generic types correctly, such as <c>List{Dictionary{string, int}}</c>.
  /// </remarks>
  /// <param name="typeSymbol"><see cref="ITypeSymbol"/> from which to extract generic parameters.</param>
  /// <returns>Correctly formatted string with generic parameters in angle brackets.</returns>
  public static string GetGenericParametersString(this ITypeSymbol typeSymbol)
  {
    if (typeSymbol is INamedTypeSymbol { IsGenericType: true } namedTypeSymbol)
    {
      var typeArgs = namedTypeSymbol.TypeArguments.Select(ta => 
        ta is INamedTypeSymbol { IsGenericType: true } nestedNamedType 
          ? GetGenericParametersString(nestedNamedType) 
          : ta.ToString());
      return $"<{string.Join(", ", typeArgs)}>";
    }
    return string.Empty;
  }
}