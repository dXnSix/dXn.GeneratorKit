using System;
using System.Linq;
using System.Text;

namespace Six.GeneratorKit;

public static class DocumentationHelpers
{
  /// <summary>
  /// Prepares the XML documentation comment retrieved with <c>ISymbol.GetDocumentationCommentXml()</c> method
  /// for rendering in the generated code.
  /// </summary>
  /// <param name="xmlComment">Xml documentation comment retrieved with <c>ISymbol.GetDocumentationCommentXml()</c></param>
  /// <returns>Returns properly formatted xml doc comments ready to be used with source generation.</returns>
  /// <remarks>
  /// Xml documentation comments retrieved with <c>ISymbol.GetDocumentationCommentXml()</c> method
  /// have a format that is not suitable for direct use in source generation.
  /// The first line is a <c>member</c> tag, and all other lines are missing the <c>///</c> prefix.
  /// </remarks>
  public static string FormatXmlDocumentationComment(string? xmlComment)
  {
    if (string.IsNullOrWhiteSpace(xmlComment))
      return string.Empty;

    var lines = xmlComment!.Split(["\r\n", "\r", "\n"], StringSplitOptions.None)
      .Skip(1) // Skip the first line with <member>
      .TakeWhile(line => !line.Trim().StartsWith("</member>")) // Stop at </member>
      .Select(line => line.Trim()) // Trim each line
      .Where(line => !string.IsNullOrWhiteSpace(line)); // Remove empty lines
    
    var sb = new StringBuilder();
    foreach (var line in lines)
    {
      sb.AppendLine($"/// {line}");
    }

    return sb.ToString().TrimEnd(); // Remove trailing newline
  }
}