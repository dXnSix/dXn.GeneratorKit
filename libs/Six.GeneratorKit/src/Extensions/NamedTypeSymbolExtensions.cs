namespace Six.GeneratorKit;

[PublicAPI]
[ExcludeFromCodeCoverage]
public static class NamedTypeSymbolExtensions
{
  /// <summary>
  /// Gets the underlying type of the enum.
  /// </summary>
  /// <param name="symbol">The INamedTypeSymbol instance this method extends.</param>
  /// <returns>The underlying type of the enum as a string. If the enum does not have
  /// an explicit underlying type, "int" is returned.
  /// </returns>
  public static string GetEnumUndelyingType(this INamedTypeSymbol symbol)
  {
    return symbol.EnumUnderlyingType?.ToString() ?? "int";
  }
}