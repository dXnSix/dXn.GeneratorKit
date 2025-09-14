namespace Six.GeneratorKit;

/// <summary>
/// Provides extension methods for the IPropertySymbol interface.
/// </summary>
public static partial class PropertySymbolExtensions
{
  /// <summary>
  /// Checks if the property is of a collection type.
  /// </summary>
  /// <param name="propertySymbol">The property to check.</param>
  /// <param name="collectionType">Out parameter that returns the type of collection if the property is a collection type.</param>
  /// <returns>True if the property is a collection type, false otherwise.</returns>
  public static bool IsCollectionType(this IPropertySymbol propertySymbol, out CollectionType collectionType)
  {
    return propertySymbol.Type.IsCollectionType(out collectionType);
  }
}