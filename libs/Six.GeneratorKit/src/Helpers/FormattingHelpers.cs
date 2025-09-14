using System;
using System.Collections.Generic;
using System.Linq;

namespace Six.GeneratorKit;

/// <summary>
/// Provides helper methods for formatting values and fully qualified names.
/// </summary>
public static class FormattingHelpers
{
  /// <summary>
  /// Formats a value based on its type.
  /// </summary>
  /// <param name="type">The type of the value.</param>
  /// <param name="value">The value to format.</param>
  /// <returns>The formatted value as a string.</returns>
  /// <example>
  /// <para>
  /// For example, calling <c>GetValueWithFormat("float", "42")</c> will return <c>"42f"</c>.
  /// </para>
  /// <para>
  /// Or calling <c>GetValueWithFormat("string", "Hello, World!")</c> will return <c>"\"Hello, World!\""</c>.
  /// </para>
  /// </example>
  public static string GetValueWithFormat(string type, string value)
  {
    if(string.IsNullOrEmpty(value))
      return value;

    switch(type.ToLowerInvariant())
    {
      case "string":
        return $"\"{value}\"";
      case "char":
        return $"'{value}'";
      case "float":
        return $"{value}f";
      case "decimal":
        return $"{value}m";
      case "double":
        return $"{value}d";
      case "long":
        return $"{value}L";
      case "uint":
        return $"{value}U";
      case "ulong":
        return $"{value}UL";
      case "byte":
      case "sbyte":
      case "short":
      case "ushort":
        return $"({type}){value}";
      case "bool":
        return value.ToLowerInvariant();
      case "datetime":
        return $"global::System.DateTime.Parse(\"{value}\")";
      case "datetimeoffset":
        return $"global::System.DateTimeOffset.Parse(\"{value}\")";
      case "guid":
        return $"global::System.Guid.Parse(\"{value}\")";
      case "timespan":
        return $"global::System.TimeSpan.Parse(\"{value}\")";
      default:
        // If it's potentially an enum or a custom type, use it as is
        return value;
    }
  }

  /// <summary>
  /// Gets the fully qualified name of a type, adding the "global::" prefix if necessary.
  /// </summary>
  /// <param name="fullyQualifiedTypeName">The fully qualified name of the type.</param>
  /// <returns>The fully qualified name with the "global::" prefix if necessary.</returns>
  public static string GetGlobalFqn(string fullyQualifiedTypeName)
  {
    // List of primitive types and common types that don't need "global::" prefix
    HashSet<string> commonTypes =
    [
      "bool",
      "byte",
      "sbyte",
      "char",
      "decimal",
      "double",
      "float",
      "int",
      "uint",
      "long",
      "ulong",
      "short",
      "ushort",
      "object",
      "string"
    ];

    // Check if it's a common type
    if(commonTypes.Contains(fullyQualifiedTypeName))
    {
      return fullyQualifiedTypeName;
    }

    // Check if it's a generic type
    if(fullyQualifiedTypeName.Contains("<"))
    {
      // Split the generic type into its components and get global FQN for each of them
      // IMPORTANT: generic type arguments must be set as FQNs for this to work.
      var parts = fullyQualifiedTypeName.Split(['<', '>'], StringSplitOptions.RemoveEmptyEntries);
      var baseType = parts[0];
      var typeArguments = parts[1].Split(',').Select(GetGlobalFqn).ToArray();

      // Reconstruct the generic type with fully qualified type arguments
      return $"global::{baseType}<{string.Join(", ", typeArguments)}>";
    }

    // For all other types, add the "global::" prefix
    return $"global::{fullyQualifiedTypeName}";
  }
}