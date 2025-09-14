namespace Six.GeneratorKit;

/// <summary>
/// Enum representing the different types of collections.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum CollectionType
{
  Unknown,
  IList,
  IDictionary,
  ISet
}

/// <summary>
/// Provides extension methods for the ITypeSymbol interface.
/// </summary>
public static partial class TypeSymbolExtensions
{
  /// <summary>
  /// Checks if the type is a collection type and returns the type of collection.
  /// </summary>
  /// <param name="typeSymbol">The type to check.</param>
  /// <param name="collectionType">Out parameter that returns the type of collection if the type is a collection type.</param>
  /// <returns>True if the type is a collection type, false otherwise.</returns>
  public static bool IsCollectionType(this ITypeSymbol typeSymbol, out CollectionType collectionType)
  {
    collectionType = CollectionType.Unknown;
    if(typeSymbol is INamedTypeSymbol { IsGenericType: true } namedTypeSymbol)
    {
      // Check for IList<T>
      if(typeSymbol.HasInterface("IList`1"))
      {
        collectionType = CollectionType.IList;
        return true;
      }

      // Check for IDictionary<TKey, TValue>
      if(typeSymbol.HasInterface("IDictionary`2"))
      {
        collectionType = CollectionType.IDictionary;
        return true;
      }

      // Check for ISet<T>
      if(typeSymbol.HasInterface("ISet`1"))
      {
        collectionType = CollectionType.ISet;
        return true;
      }
    }

    return false;
  }
}