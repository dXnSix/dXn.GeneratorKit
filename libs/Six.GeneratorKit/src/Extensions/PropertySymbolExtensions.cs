using System.Linq;

namespace Six.GeneratorKit;

/// <summary>
/// Provides extension methods for the IPropertySymbol interface.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class PropertySymbolExtensions
{
  /// <summary>
  /// Checks if the property has a specific attribute.
  /// </summary>
  /// <param name="propertySymbol">The property to check.</param>
  /// <param name="attributeSymbol">The attribute to check for.</param>
  /// <returns>True if the property has the attribute, false otherwise.</returns>
  public static bool HasAttribute(this IPropertySymbol propertySymbol, INamedTypeSymbol attributeSymbol)
  {
    return propertySymbol.Type.GetAttributes().Any(attr =>
      SymbolEqualityComparer.Default.Equals(attr.AttributeClass, attributeSymbol));
  }

  /// <summary>
  /// Checks if the property is nullable.
  /// </summary>
  /// <param name="propertySymbol">The property to check.</param>
  /// <returns>True if the property is nullable, false otherwise.</returns>
  public static bool IsNullable(this IPropertySymbol propertySymbol)
  {
    return propertySymbol.NullableAnnotation == NullableAnnotation.Annotated;
  }
}