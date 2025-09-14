using System.Globalization;

namespace Six.GeneratorKit;

[PublicAPI]
[ExcludeFromCodeCoverage]
public static class NameHelpers
{
  /// <summary>
  /// Sometimes the attribute name is not ending with "Attribute" so we need to add it.
  /// </summary>
  /// <param name="attributeName">ClassName of the attribute</param>
  /// <returns>Returns the fully qualified attribute name (with "Attribute" suffix).</returns>
  public static (string Name, string NameWithAttributeSuffix) SanitizeAttributeName(this string attributeName)
  {
    string name;
    string nameWithAttributeSuffix;

    if(attributeName.ToLowerInvariant().Contains("attribute") == false)
    {
      name = attributeName;
      nameWithAttributeSuffix = $"{attributeName}Attribute";
    }
    else
    {
      name = attributeName.Replace("Attribute", "");
      nameWithAttributeSuffix = attributeName;
    }

    return (name, nameWithAttributeSuffix);
  }


  /// <summary>
  ///   Removes the namespace from a string.
  /// </summary>
  /// <param name="source">Source string (probably) with namespace.</param>
  /// <param name="namespace">String without the namespace.</param>
  /// <returns>Returns cleaned string or same string if namespace don't match or exists in the source string.</returns>
  public static string RemoveNamespace(this string source, string @namespace)
  {
    if(source.Contains(@namespace))
    {
      return source.Replace(@namespace, "").Trim('.');
    }

    return source;
  }
  
  
  /// <summary>
  /// Get the name of the member in PascalCase (for example, to convert the _memberName to MemberName property name)
  /// </summary>
  /// <returns>The member name in PascalCase (as property)</returns>
  public static string ConvertMemberNameToPascalCase(this string propertyName)
  {
    if (propertyName.StartsWith("m_"))
    {
      propertyName = propertyName.Substring(2);
    }
    else if (propertyName.StartsWith("_"))
    {
      propertyName = propertyName.TrimStart('_');
    }

    return $"{char.ToUpper(propertyName[0], CultureInfo.InvariantCulture)}{propertyName.Substring(1)}";
  }
}