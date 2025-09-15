# Six.GeneratorKit

## Introduction
**Six GeneratorKit** is a helper library designed to simplify the development of incremental source generators in .NET.

Source generators are a powerful feature in .NET that allow you to generate code at compile time.
However, working with the Roslyn APIs directly can be complex and error-prone.

It provides a set of utilities, extensions, and services that streamline the process of working with Roslyn's
incremental source generation APIs like:

- **Type-safe collections** with value equality support
- **Comprehensive symbol extensions** for easier code analysis
- **Attribute data utilities** for processing attributes
- **Error handling services** for robust generator development
- **Performance-optimized helpers** for efficient code generation


## Key Features

### 🚀 **Performance Optimized**
- Built on .NET Standard 2.0 for maximum compatibility
- Uses pooled buffers and efficient memory management
- Optimized for incremental source generation scenarios

### 🛡️ **Type Safe**
- Strongly typed APIs with nullable reference types enabled
- Comprehensive XML documentation
- Compile-time safety with C# 13 features

### 🔧 **Developer Friendly**
- Rich extension methods for common operations
- Comprehensive error handling and diagnostics

### 📦 **Source-Only Package**
- Delivered as contentFiles to avoid runtime dependencies
- No additional assemblies in your output
- Clean integration with your source generators



## Architecture

**Six GeneratorKit** is organized into several key areas:

### Collections
- `EquatableArray<T>` - Immutable array with value equality
- `ImmutableArrayBuilder<T>` - Efficient builder for immutable arrays
- `HashCode` utilities for consistent hashing

### Extensions
- **Symbol Extensions** - Rich extensions for `ISymbol` and derived types
- **Attribute Extensions** - Utilities for working with `AttributeData`
- **Type Extensions** - Helpers for type analysis and code generation
- **Incremental Provider Extensions** - Advanced incremental generation patterns

### Services
- **Generator Service** - Centralized error handling and diagnostics
- **External Init** - Compatibility shims for older target frameworks

### Helpers
- **Code Header Helpers** - Generate consistent file headers
- **Documentation Helpers** - XML documentation utilities
- **Formatting Helpers** - Code formatting and indentation
- **Name Helpers** - Identifier and naming utilities


## Credits
Some of the code in this library is based on the great articles by [Andrew Locke](https://andrewlock.net/)