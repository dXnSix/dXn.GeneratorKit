using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Six.GeneratorKit;

/// <summary>
/// Extension method for the INamedTypeSymbol interface.
/// </summary>
public static partial class TypeSymbolExtensions
{
  /// <summary>
  /// Gets the members of the enum.
  /// </summary>
  /// <param name="symbol">The ITypeSymbol instance this method extends.</param>
  /// <returns>An ImmutableArray of ISymbol representing the members of the enum.</returns>
  public static ImmutableArray<ISymbol> GetEnumMembers(this ITypeSymbol symbol)
  {
    List<ISymbol> enumMembers = [];

    var symbolMembers = symbol.GetMembers();
    foreach(var member in symbolMembers)
    {
      if(member is not IFieldSymbol field || field.ConstantValue is null)
      {
        continue;
      }

      enumMembers.Add(member);
    }

    // ReSharper disable once UseCollectionExpression
    return enumMembers.ToImmutableArray();
  }

  /// <summary>
  /// Performs an action for each member of the enum.
  /// </summary>
  /// <param name="symbol">The ITypeSymbol instance this method extends.</param>
  /// <param name="action">The action to be performed on each member of the enum.</param>
  public static void ForEachEnumMember(this ITypeSymbol symbol, Action<ISymbol> action)
  {
    var symbolMembers = symbol.GetMembers();
    foreach(var member in symbolMembers)
    {
      if(member is not IFieldSymbol field || field.ConstantValue is null)
      {
        continue;
      }

      action(member);
    }
  }
}