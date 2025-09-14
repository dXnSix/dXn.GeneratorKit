using System;
using System.Collections.Generic;
using System.Linq;

namespace Six.GeneratorKit;

[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class TypeSymbolExtensions
{
  /// <summary>
  /// Gets the keyword for the kind of the type.
  /// </summary>
  /// <param name="typeSymbol">The type to get the keyword for.</param>
  /// <returns>The keyword for the kind of the type.</returns>
  public static string GetKindKeyword(this ITypeSymbol typeSymbol)
  {
    if(typeSymbol.IsRecord)
    {
      return typeSymbol.TypeKind == TypeKind.Class ? "record class" :
        typeSymbol.TypeKind == TypeKind.Struct ? "record struct" :
        "record"; // Default to "record" for unspecified cases
    }

    return typeSymbol.TypeKind == TypeKind.Class ? "class" : "struct";
  }

  /// <summary>
  /// Gets the properties of the type.
  /// </summary>
  /// <param name="typeSymbol">The type to get the properties for.</param>
  /// <param name="includeInherited">Whether to include inherited properties.</param>
  /// <returns>The properties of the type.</returns>
  public static IEnumerable<IPropertySymbol> GetProperties(this ITypeSymbol typeSymbol, bool includeInherited = false)
  {
    IEnumerable<IPropertySymbol> properties = typeSymbol.GetMembers().OfType<IPropertySymbol>();
    if(includeInherited)
    {
      properties = properties.Concat(typeSymbol.BaseType?.GetProperties(true) ?? []);
    }

    return properties;
  }

  /// <summary>
  /// Checks if the type implements a specific interface.
  /// </summary>
  /// <param name="typeSymbol">The type to check.</param>
  /// <param name="interfaceName">The name of the interface to check for.</param>
  /// <returns>True if the type implements the interface, false otherwise.</returns>
  public static bool HasInterface(this ITypeSymbol typeSymbol, string interfaceName)
  {
    return typeSymbol.AllInterfaces.Any(i => i.MetadataName.Equals(interfaceName));
  }

  /// <summary>
  /// Checks whether or not a given <see cref="ITypeSymbol"/> implements an interface with a specified name.
  /// </summary>
  /// <param name="typeSymbol">The target <see cref="ITypeSymbol"/> instance to check.</param>
  /// <param name="name">The full name of the type to check for interface implementation.</param>
  /// <returns>Whether or not <paramref name="typeSymbol"/> has an interface with the specified name.</returns>
  public static bool HasInterfaceWithFullyQualifiedMetadataName(this ITypeSymbol typeSymbol, string name)
  {
    foreach(var interfaceType in typeSymbol.AllInterfaces)
    {
      if(interfaceType.HasFullyQualifiedMetadataName(name))
      {
        return true;
      }
    }

    return false;
  }

  /// <summary>
  /// Checks whether or not a given <see cref="ITypeSymbol"/> has or inherits a specified attribute.
  /// </summary>
  /// <param name="typeSymbol">The target <see cref="ITypeSymbol"/> instance to check.</param>
  /// <param name="predicate">The predicate used to match available attributes.</param>
  /// <returns>Whether or not <paramref name="typeSymbol"/> has an attribute matching <paramref name="predicate"/>.</returns>
  public static bool HasOrInheritsAttribute(this ITypeSymbol typeSymbol, Func<AttributeData, bool> predicate)
  {
    for(var currentType = typeSymbol; currentType is not null; currentType = currentType.BaseType)
    {
      if(currentType.GetAttributes().Any(predicate))
      {
        return true;
      }
    }

    return false;
  }

  /// <summary>
  /// Checks whether or not a given <see cref="ITypeSymbol"/> has or inherits a specified attribute.
  /// </summary>
  /// <param name="typeSymbol">The target <see cref="ITypeSymbol"/> instance to check.</param>
  /// <param name="name">The name of the attribute to look for.</param>
  /// <returns>Whether or not <paramref name="typeSymbol"/> has an attribute with the specified type name.</returns>
  public static bool HasOrInheritsAttributeWithFullyQualifiedMetadataName(this ITypeSymbol typeSymbol, string name)
  {
    for(var currentType = typeSymbol; currentType is not null; currentType = currentType.BaseType)
    {
      if(currentType.HasAttributeWithFullyQualifiedMetadataName(name))
      {
        return true;
      }
    }

    return false;
  }

  /// <summary>
  /// Checks whether or not a given <see cref="ITypeSymbol"/> has or inherits from a specified type.
  /// </summary>
  /// <param name="typeSymbol">The target <see cref="ITypeSymbol"/> instance to check.</param>
  /// <param name="name">The full name of the type to check for inheritance.</param>
  /// <returns>Whether or not <paramref name="typeSymbol"/> is or inherits from <paramref name="name"/>.</returns>
  public static bool HasOrInheritsFromFullyQualifiedMetadataName(this ITypeSymbol typeSymbol, string name)
  {
    for(var currentType = typeSymbol; currentType is not null; currentType = currentType.BaseType)
    {
      if(currentType.HasFullyQualifiedMetadataName(name))
      {
        return true;
      }
    }

    return false;
  }

  /// <summary>
  /// Checks whether or not a given <see cref="ITypeSymbol"/> inherits from a specified type.
  /// </summary>
  /// <param name="typeSymbol">The target <see cref="ITypeSymbol"/> instance to check.</param>
  /// <param name="name">The full name of the type to check for inheritance.</param>
  /// <returns>Whether or not <paramref name="typeSymbol"/> inherits from <paramref name="name"/>.</returns>
  public static bool InheritsFromFullyQualifiedMetadataName(this ITypeSymbol typeSymbol, string name)
  {
    var baseType = typeSymbol.BaseType;

    while(baseType is not null)
    {
      if(baseType.HasFullyQualifiedMetadataName(name))
      {
        return true;
      }

      baseType = baseType.BaseType;
    }

    return false;
  }

  /// <summary>
  ///   Checks if certain type inherits from another type
  /// </summary>
  /// <param name="typeSymbol">Type symbol to check if inherits.</param>
  /// <param name="baseType">Base type to perform inheritance against.</param>
  public static bool InheritsFromType(this ITypeSymbol typeSymbol, ITypeSymbol baseType)
  {
    SymbolEqualityComparer comparer = SymbolEqualityComparer.Default;
    ITypeSymbol? currentType = typeSymbol;
    while(currentType != null)
    {
      if(comparer.Equals(currentType, baseType))
      {
        return true;
      }

      currentType = currentType.BaseType;
    }

    return false;
  }

  /// <summary>
  /// Checks whether or not a given type symbol has a specified fully qualified metadata name.
  /// </summary>
  /// <param name="symbol">The input <see cref="ITypeSymbol"/> instance to check.</param>
  /// <param name="name">The full name to check.</param>
  /// <returns>Whether <paramref name="symbol"/> has a full name equals to <paramref name="name"/>.</returns>
  public static bool HasFullyQualifiedMetadataName(this ITypeSymbol symbol, string name)
  {
    using var builder = ImmutableArrayBuilder<char>.Rent();

    symbol.AppendFullyQualifiedMetadataName(builder);

    return builder.WrittenSpan.SequenceEqual(name.AsSpan());
  }

  /// <summary>
  /// Gets the fully qualified metadata name for a given <see cref="ITypeSymbol"/> instance.
  /// </summary>
  /// <param name="symbol">The input <see cref="ITypeSymbol"/> instance.</param>
  /// <returns>The fully qualified metadata name for <paramref name="symbol"/>.</returns>
  public static string GetFullyQualifiedMetadataName(this ITypeSymbol symbol)
  {
    using var builder = ImmutableArrayBuilder<char>.Rent();

    symbol.AppendFullyQualifiedMetadataName(builder);

    return builder.ToString();
  }


  // PRIVATE METHODS
  // ----------------------------------------------------------------------------------------

  /// <summary>
  /// Appends the fully qualified metadata name for a given symbol to a target builder.
  /// </summary>
  /// <param name="symbol">The input <see cref="ITypeSymbol"/> instance.</param>
  /// <param name="builder">The target <see cref="ImmutableArrayBuilder{T}"/> instance.</param>
  static void AppendFullyQualifiedMetadataName(this ITypeSymbol symbol, ImmutableArrayBuilder<char> builder)
  {
    static void BuildFrom(ISymbol? symbol, ImmutableArrayBuilder<char> builder)
    {
      switch(symbol)
      {
        // Namespaces that are nested also append a leading '.'
        case INamespaceSymbol { ContainingNamespace.IsGlobalNamespace: false }:
          BuildFrom(symbol.ContainingNamespace, builder);
          builder.Add('.');
          builder.AddRange(symbol.MetadataName.AsSpan());
          break;

        // Other namespaces (ie. the one right before global) skip the leading '.'
        case INamespaceSymbol { IsGlobalNamespace: false }:
          builder.AddRange(symbol.MetadataName.AsSpan());
          break;

        // Types with no namespace just have their metadata name directly written
        case ITypeSymbol { ContainingSymbol: INamespaceSymbol { IsGlobalNamespace: true } }:
          builder.AddRange(symbol.MetadataName.AsSpan());
          break;

        // Types with a containing non-global namespace also append a leading '.'
        case ITypeSymbol { ContainingSymbol: INamespaceSymbol namespaceSymbol }:
          BuildFrom(namespaceSymbol, builder);
          builder.Add('.');
          builder.AddRange(symbol.MetadataName.AsSpan());
          break;

        // Nested types append a leading '+'
        case ITypeSymbol { ContainingSymbol: ITypeSymbol typeSymbol }:
          BuildFrom(typeSymbol, builder);
          builder.Add('+');
          builder.AddRange(symbol.MetadataName.AsSpan());
          break;
      }
    }

    BuildFrom(symbol, builder);
  }
}