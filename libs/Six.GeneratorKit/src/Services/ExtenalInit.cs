// For shared projects used with source generators, especially when you want
// to utilize C# 9.0 features like init accessors while maintaining compatibility
// with projects that target different frameworks or versions, you can manually
// define the IsExternalInit class as mentioned before.
//
// Shared projects don't produce output themselves but are included in other projects,
// so the compatibility of features like init setters depends on the target frameworks
// of the consuming projects.

namespace System.Runtime.CompilerServices;

// Define IsExternalInit only if targeting frameworks that don't already include it
#if NETSTANDARD2_0 || NETCOREAPP3_1
[PublicAPI]
[ExcludeFromCodeCoverage]
public static class IsExternalInit { }
#endif