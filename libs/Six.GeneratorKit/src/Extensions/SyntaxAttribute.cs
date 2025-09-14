using System.Linq;

namespace Six.GeneratorKit;

[PublicAPI]
[ExcludeFromCodeCoverage]
public static class SyntaxAttribute
{
  /// <summary>
  /// Checks if the target attribute exists in the provided attribute lists.
  /// </summary>
  /// <remarks>This method is only when working with SyntaxNodes.</remarks>
  /// <param name="attributeLists">A list of attribute lists to search in.</param>
  /// <param name="attributeName">The name of the attribute to search for.</param>
  /// <returns>
  /// Returns true if the attribute is found in any of the attribute lists, otherwise false.
  /// </returns>
  public static bool HasTargetAttribute(SyntaxList<AttributeListSyntax> attributeLists, string attributeName)
  {
    // Sanitize the attribute name to ensure a consistent format
    var sanitizedAttributeName = attributeName.SanitizeAttributeName();

    // Use LINQ to flatten the attribute lists and check if any attribute matches the target attribute name
    return attributeLists
      .SelectMany(attributeList => attributeList.Attributes)
      .Any(attribute => attribute.Name.ToString() == sanitizedAttributeName.Name ||
                        attribute.Name.ToString() == sanitizedAttributeName.NameWithAttributeSuffix);
  }
}