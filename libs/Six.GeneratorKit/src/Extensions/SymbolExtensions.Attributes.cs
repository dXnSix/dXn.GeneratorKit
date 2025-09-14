using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Six.GeneratorKit;

public static partial class SymbolExtensions
{
  /// <summary>
  ///   Checks if <see cref="ISymbol"/> has a specific attribute.
  /// </summary>
  /// <remarks>
  ///   It will check both the attribute name and the attribute name with "Attribute" suffix.
  /// </remarks>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <param name="attributeName">ClassName of the attribute</param>
  /// <returns></returns>
  public static bool HasAttribute(this ISymbol symbol, string attributeName)
  {
    var nameTuple = attributeName.SanitizeAttributeName();
    return _HasAttribute(symbol, nameTuple.NameWithAttributeSuffix) || _HasAttribute(symbol, nameTuple.Name);
  }

  /// <summary>
  /// Checks if <see cref="ISymbol"/> has a specific attribute.
  /// </summary>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <param name="attributeSymbol"></param>
  /// <returns></returns>
  public static bool HasAttribute(this ISymbol symbol, INamedTypeSymbol attributeSymbol)
  {
    return symbol.GetAttributes().Any(attr =>
      SymbolEqualityComparer.Default.Equals(attr.AttributeClass, attributeSymbol));
  }


  /// <summary>
  /// Gets all attribute data as <see cref="ImmutableArray{AttributeData}"/>
  /// of an attribute name attached to a <see cref="ISymbol" />
  /// </summary>
  /// <remarks>It will automatically search with sanitized attribute name with and without 'Attribute' suffix.</remarks>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <param name="attributeName">ClassName of the attribute (must end with "Attribute" keyword)</param>
  /// <returns>Returns a list of <see cref="AttributeDataExtensions" /> objects or empty list.</returns>
  public static ImmutableArray<AttributeData> GetAttributeData(this ISymbol symbol, string attributeName)
  {
    var nameTuple = attributeName.SanitizeAttributeName();

    ImmutableArray<AttributeData> data = _GetAttributeData(symbol, nameTuple.Name);
    if(data.Length > 0) return data;

    return _GetAttributeData(symbol, nameTuple.NameWithAttributeSuffix);
  }


  /// <summary>
  /// Executes a given action for each AttributeData of a specific attribute attached to a type attributeData.
  /// </summary>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <param name="attributeName">The name of the attribute to check for.</param>
  /// <param name="action">The action to be performed on each AttributeData.</param>
  public static void ForEachAttributeData(this ISymbol symbol, string attributeName, Action<AttributeData> action)
  {
    var nameTuple = attributeName.SanitizeAttributeName();

    ImmutableArray<AttributeData> data = _GetAttributeData(symbol, nameTuple.NameWithAttributeSuffix);
    if(data.Length > 0)
    {
      foreach(var attrData in data)
      {
        action(attrData);
      }

      return;
    }

    ImmutableArray<AttributeData> dataWithSuffix = _GetAttributeData(symbol, nameTuple.Name);
    if(dataWithSuffix.Length > 0)
    {
      foreach(var attrData in dataWithSuffix)
      {
        action(attrData);
      }
    }
  }


  /// <summary>
  ///   Find if any of the attribute names in the passed list are applied to the <see cref="ISymbol" />
  /// </summary>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <param name="attributeNames">List of attribute names to check for.</param>
  /// <returns>Returns an attribute name which is applied on the class or null.</returns>
  public static string? FindMatchingAttribute(this ISymbol symbol, List<string> attributeNames)
  {
    ImmutableArray<AttributeData> attrs = symbol.GetAttributes();
    foreach(string? attr in attributeNames)
    {
      if(attrs.Any(ad => ad.AttributeClass?.Name.Contains(attr) ?? false))
      {
        return attr;
      }
    }

    return null;
  }


  /// <summary>
  ///   Gets all properties and their values of a specific attribute from a <see cref="ISymbol" />
  /// </summary>
  /// <param name="symbol">
  ///   Given a SyntaxNode, you can retrieve its corresponding <see cref="ISymbol"/> using the SemanticModel.
  ///   Obtain the SemanticModel: If you're inside a method where you're processing syntax nodes in your generator, you might
  ///   have a SyntaxNode and the current Compilation. You can use the Compilation to get the SemanticModel for any syntax
  ///   tree:
  ///   <br></br>
  ///   <code>SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);</code>
  ///   <br></br>
  ///   Get the Symbol: Once you have the SemanticModel and a SyntaxNode, you can get the ISymbol for that node:
  ///   <br></br>
  ///   <code>ISymbol attributeData = semanticModel.GetDeclaredSymbol(syntaxNode);</code>
  ///   <br></br>
  ///   For <see cref="TypeDeclarationSyntax" /> (classes, structs, enums, etc.), GetDeclaredSymbol will return a
  ///   <see cref="INamedTypeSymbol" />.
  ///   <br></br>
  ///   For <see cref="MethodDeclarationSyntax" />, it will return an <see cref="IMethodSymbol" />.
  ///   <br></br>
  ///   For <see cref="PropertyDeclarationSyntax" />, it will return an <see cref="IPropertySymbol" />.
  ///   <br></br>
  ///   ...and so on.
  /// </param>
  /// <param name="attributeName">ClassName of the attribute</param>
  /// <returns>Returns the Dictionary of argumentName->Data or null.</returns>
  public static Dictionary<string, object>? GetAttributeArguments(this ISymbol symbol, string attributeName)
  {
    var attrName = attributeName.SanitizeAttributeName();
    
    return _GetAttributeArguments(symbol, attrName.NameWithAttributeSuffix)
           ?? _GetAttributeArguments(symbol, attrName.Name);
  }


  /// <summary>
  ///   Gets the value of a named argument from a specific attribute attached to a type attributeData.
  /// </summary>
  /// <typeparam name="T">The expected type of the argument value.</typeparam>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <param name="namespace">The namespace of the attribute to check for.</param>
  /// <param name="attributeName">The name of the attribute to check for.</param>
  /// <param name="argumentName">The name of the argument to retrieve the value for.</param>
  /// <returns>The value of the named argument if it exists and can be cast to type T, otherwise default(T).</returns>
  public static T? GetAttributeNamedArgumentValue<T>(
    this ISymbol symbol,
    string @namespace,
    string attributeName,
    string argumentName)
  {
    foreach(AttributeData? attributeData in symbol.GetAttributes())
    {
      string attrFqn = $"{@namespace}.{attributeName.SanitizeAttributeName().NameWithAttributeSuffix}";
      if(attributeData.AttributeClass?.ToDisplayString() == attrFqn)
      {
        foreach(KeyValuePair<string, TypedConstant> arg in attributeData.NamedArguments)
        {
          if(arg.Key.Equals(argumentName))
          {
            if(arg.Value.Value is T value)
            {
              return value;
            }

            // Break the loop if the argument is found but the type does not match.
            break;
          }
        }
      }
    }

    // Return the default value for the type if the attribute or argument is not found.
    return default;
  }


  /// <summary>
  ///   Gets the value of a named argument from a specific attribute attached to a type attributeData.
  /// </summary>
  /// <typeparam name="T">The expected type of the argument value.</typeparam>
  /// <param name="symbol">The <see cref="ISymbol"/> to operate on.</param>
  /// <param name="attributeSymbol">The attributeData of the attribute to check for.</param>
  /// <param name="argumentName">The name of the argument to retrieve the value for.</param>
  /// <returns>The value of the named argument if it exists and can be cast to type T, otherwise default(T).</returns>
  public static T? GetAttributeNamedArgumentValue<T>(
    this ISymbol symbol,
    INamedTypeSymbol attributeSymbol,
    string argumentName)
  {
    foreach(AttributeData? attributeData in symbol.GetAttributes())
    {
      if(SymbolEqualityComparer.Default.Equals(attributeData.AttributeClass, attributeSymbol))
      {
        foreach(KeyValuePair<string, TypedConstant> arg in attributeData.NamedArguments)
        {
          if(arg.Key.Equals(argumentName))
          {
            if(arg.Value.Value is T value)
            {
              return value;
            }
          }
        }
      }
    }

    return default;
  }

  /// <summary>
  /// Checks whether or not a given symbol has an attribute with the specified fully qualified metadata name.
  /// </summary>
  /// <param name="symbol">The input <see cref="ISymbol"/> instance to check.</param>
  /// <param name="name">The attribute name to look for.</param>
  /// <returns>Whether or not <paramref name="symbol"/> has an attribute with the specified name.</returns>
  public static bool HasAttributeWithFullyQualifiedMetadataName(this ISymbol symbol, string name)
  {
    foreach (var attribute in symbol.GetAttributes())
    {
      if (attribute.AttributeClass?.HasFullyQualifiedMetadataName(name) == true)
      {
        return true;
      }
    }

    return false;
  }

  /// <summary>
  /// Executes a given action for each named argument in the provided AttributeData.
  /// </summary>
  /// <param name="attributeData">The AttributeData instance this method extends.</param>
  /// <param name="action">The action to be performed on each named argument.</param>
  public static void ForEachAttributeNamedArgument(
    this AttributeData? attributeData,
    Action<KeyValuePair<string, TypedConstant>> action)
  {
    if(attributeData is null)
    {
      throw new ArgumentNullException(nameof(attributeData));
    }

    foreach(KeyValuePair<string, TypedConstant> namedArgument in attributeData.NamedArguments)
    {
      action(namedArgument);
    }
  }


  // PRIVATE
  // -------------------------------------------------------------------------------------------------------

  static bool _HasAttribute(this ISymbol symbol, string attributeName)
  {
    return symbol.GetAttributes().Any(ad => ad.AttributeClass?.Name == attributeName);
  }

  static ImmutableArray<AttributeData> _GetAttributeData(ISymbol symbol, string attributeName)
  {
    return symbol.GetAttributes().Where(attr => attr.AttributeClass?.Name == attributeName).ToImmutableArray();
  }


  static Dictionary<string, object>? _GetAttributeArguments(ISymbol symbol, string attributeName)
  {
    AttributeData? attributeData =
      symbol.GetAttributes().FirstOrDefault(ad => ad.AttributeClass?.Name == attributeName);

    if(attributeData == null) return null;

    Dictionary<string, object> arguments = new();

    // Named arguments
    foreach(KeyValuePair<string, TypedConstant> namedArg in attributeData.NamedArguments)
    {
      if(namedArg.Value.Value != null)
      {
        arguments[namedArg.Key] = namedArg.Value.Value;
      }
    }

    // Constructor arguments
    ImmutableArray<IParameterSymbol>? constructorParameters = attributeData.AttributeConstructor?.Parameters;
    if(!constructorParameters.HasValue || constructorParameters.Value.IsDefault)
    {
      return null; // or you can continue with other logic if needed
    }

    for(int i = 0; i < constructorParameters.Value.Length; i++)
    {
      string parameterName = constructorParameters.Value[i].Name;
      object? parameterValue = attributeData.ConstructorArguments[i].Value;
      if(parameterValue != null)
      {
        arguments[parameterName] = parameterValue;
      }
    }

    return arguments;
  }
}