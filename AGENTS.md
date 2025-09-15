# AGENTS.md

A comprehensive guide for AI coding agents working with the **Six.GeneratorKit** repository. This library provides
essential utilities and helpers for building robust Roslyn source generators in .NET C# projects.

## 📦 Six.GeneratorKit Library Overview

**Six.GeneratorKit** is a source-only NuGet package that provides a comprehensive set of utilities, extensions, and helpers specifically designed for building incremental source generators with Roslyn. The library targets .NET Standard 2.0 for maximum compatibility and includes content files that are embedded directly into consuming projects.

### 🎯 Purpose and Goals

- **Simplify Source Generator Development**: Provides common patterns and utilities to reduce boilerplate code
- **Enhance Performance**: Includes optimized collections and caching mechanisms for generator efficiency  
- **Improve Developer Experience**: Offers helpful extensions and services for common generator tasks
- **Ensure Compatibility**: Targets .NET Standard 2.0 with support for .NET 8, 9, and 10
- **Production Ready**: Built with nullable reference types, comprehensive error handling, and robust APIs

### 🔧 Key Components

#### **Collections**
- **`EquatableArray<T>`**: Immutable array with value equality support for efficient generator caching
- **`ImmutableArrayBuilder<T>`**: High-performance builder for constructing immutable arrays with pooled buffers
- **`HashCode`**: Utilities for generating consistent hash codes across different scenarios

#### **Extensions**
- **Symbol Extensions**: Rich extension methods for `ITypeSymbol`, `INamedTypeSymbol`, `IPropertySymbol`
- **Attribute Extensions**: Simplified attribute data access and validation
- **Context Extensions**: Enhanced `SourceProductionContext` with better source file handling
- **Provider Extensions**: Utilities for working with `IncrementalValuesProvider<T>`

#### **Services**
- **`GeneratorService`**: Centralized error collection and reporting for generator diagnostics
- **`ExternalInit`**: Support for init-only properties in older target frameworks

#### **Helpers**
- **`NameHelpers`**: Attribute name sanitization, namespace removal, and naming conventions
- **`CodeHeaderHelpers`**: Standardized code header generation with copyright and tool information
- **`DocumentationHelpers`**: XML documentation comment generation and formatting
- **`FormattingHelpers`**: Code formatting and indentation utilities

### 📋 Package Configuration

- **Package Type**: Source-only NuGet package (development dependency)
- **Target Framework**: .NET Standard 2.0
- **Language Version**: C# 13 with nullable reference types enabled
- **Distribution**: Content files embedded in consuming projects under `SixGeneratorKit` namespace
- **Dependencies**: Microsoft.CodeAnalysis.CSharp, JetBrains.Annotations, System.Collections.Immutable

### 🚀 Usage Guidelines

#### **Installation**
```xml
<PackageReference Include="Six.GeneratorKit" Version="1.0.0-alpha.33" PrivateAssets="all" />
```

#### **Key Features for Generators**
- Use `EquatableArray<T>` for generator model properties to enable proper incremental compilation caching
- Leverage `GeneratorService` for centralized error collection and reporting
- Utilize symbol extensions for simplified syntax tree analysis
- Apply name helpers for consistent attribute and type name handling

#### **Best Practices**
- Always use `EquatableArray<T>` instead of `ImmutableArray<T>` in generator models
- Collect all errors using `GeneratorService` and report them at the end of generation
- Use `ImmutableArrayBuilder<T>` for efficient collection building in hot paths
- Leverage extension methods for cleaner, more readable generator code

### 🔍 Integration Notes

This library is specifically designed for source generator projects and provides the foundational utilities needed for building robust, performant, and maintainable generators. All types are marked with appropriate attributes for code coverage exclusion and public API documentation.

## 🏗️ Project Overview

**Six.GeneratorKit** is a specialized .NET library project focused on providing utilities for Roslyn source generator development. The project follows professional development practices with automated tooling, comprehensive testing, and standardized packaging for NuGet distribution.

### Key Components

- **Library Project**: Six.GeneratorKit located in `libs/Six.GeneratorKit/` directory
  - **Source Code**: Core utilities in `src/` with organized subdirectories:
    - `Collections/`: Specialized collection types for generators
    - `Extensions/`: Extension methods for Roslyn symbols and contexts
    - `Helpers/`: Utility classes for common generator tasks
    - `Services/`: Core services like error reporting and initialization
- **Build Scripts**: **C# file-based apps** in `scripts/` directory (requires .NET 10)
- **Configuration**: Project configuration files for versioning, packaging, and code quality

## 🚀 Setup Commands

### Prerequisites

- **Git**: For version control and GitVersion integration
- **.NET 10.0.100-rc.1 SDK or later**: Required for C# file-based apps (build scripts); projects target .NET 9.0
- **Node.js**: For documentation site build
- **pnpm/npm**: For documentation site build
- **Docker**: For containerized testing (optional but recommended)


## 🧪 Testing Instructions

### Running Tests

- **All Tests**: `dotnet run scripts/tests-run.cs`
- **With Coverage**: `dotnet run scripts/tests-coverage.cs`
- **Coverage Reports**: `dotnet run scripts/tests-report.cs`

### Test Structure

- This library focuses on source generator utilities
- Testing is primarily done through integration with actual generator projects
- The library includes comprehensive code coverage exclusions for generator-specific code

### Coverage Requirements

- Generate coverage reports before committing
- Ensure all new utilities have corresponding integration tests
- Coverage reports are automatically generated in CI/CD

## 🔧 Build and Development

### Project Structure

```
Six.GeneratorKit/
├── libs/Six.GeneratorKit/   # Main library project
│   ├── src/                 # Source code organized by functionality
│   │   ├── Collections/     # EquatableArray, ImmutableArrayBuilder, HashCode
│   │   ├── Extensions/      # Symbol, Context, and Provider extensions
│   │   ├── Helpers/         # Name, Code, Documentation, and Formatting helpers
│   │   └── Services/        # GeneratorService, ExternalInit
│   ├── bin/                 # Build outputs
│   ├── obj/                 # Build intermediates
│   └── *.csproj            # Project configuration
├── scripts/                 # C# build scripts for packaging and testing
└── *.json, *.yml           # Configuration files (global.json, GitVersion.yml, etc.)
```

### Build Scripts

The `scripts/` directory contains C# file-based apps for common development tasks:

- **nuget-pack.cs**: NuGet packaging and publishing
- **tests-run.cs**: Test execution
- **tests-coverage.cs**: Coverage collection
- **tests-report.cs**: Coverage reporting

## ⚙️ Configuration Files

The template includes several configuration files that ensure consistent development, versioning, and packaging:

### Core Configuration Files

**`global.json`** - .NET SDK Version Management
- Specifies the exact .NET SDK version (10.0.100-rc.1) required for build scripts
- Ensures consistent development environment across team members
- Enables rollForward to latestMinor for patch updates

**`Directory.Build.props`** - Master Build Configuration
- Centralized build settings for all projects
- Enables C# 13, nullable reference types, implicit usings
- Configures StyleCop, code analysis, and warning settings
- Sets up CI/CD optimizations for deterministic builds

**`stylecop.json`** - Code Style Enforcement
- Defines coding standards and style rules
- Integrated with build process to ensure consistent code formatting
- Configurable rules for code quality and maintainability

### Versioning & Dependencies

**`GitVersion.yml`** - Semantic Versioning Configuration
- Automated version calculation based on Git history and branch naming
- Supports conventional commits for automatic incrementing
- Configures different versioning strategies for main, release, feature, and hotfix branches
- Integrates with CI/CD pipelines for consistent versioning

**`dependabot.yml`** - Automated Dependency Updates
- Weekly checks for NuGet package updates
- Automated pull requests for dependency updates
- Ensures security updates and latest features are available
- Reduces manual maintenance overhead

### Package Management

**`nuget.config`** - NuGet Package Sources Configuration
- Defines multiple package sources (Local, Development, Staging, Production)
- Environment variable-based configuration for security
- Supports different deployment environments
- Comprehensive documentation in comments for setup


## 📦 NuGet Packaging

### Environment Configuration

Configure NuGet feeds using environment variables:

```bash
# Local development feed
export NUGET_FEED_LOCAL_URL="/path/to/local/feed"

# Development feed
export NUGET_FEED_DEV_URL="https://nuget.pkg.github.com/NAMESPACE/index.json"
export NUGET_FEED_DEV_USERNAME="your-username"
export NUGET_FEED_DEV_PAT="your-dev-personal-access-token"

# Staging feed
export NUGET_FEED_STAGING_URL="https://nuget.pkg.github.com/NAMESPACE/index.json"
export NUGET_FEED_STAGING_USERNAME="your-username"
export NUGET_FEED_STAGING_PAT="your-staging-personal-access-token"

# Production feed (NuGet.org for example)
export NUGET_FEED_PRODUCTION_URL="https://api.nuget.org/v3/index.json"
export NUGET_FEED_PRODUCTION_USERNAME="your-username"
export NUGET_FEED_PRODUCTION_PAT="your-production-api-key"
```

### Packaging Commands

```bash
# Package to local feed (default)
dotnet run scripts/nuget-pack.cs
dotnet run scripts/nuget-pack.cs --local

# Package to development feed
dotnet run scripts/nuget-pack.cs --dev
dotnet run scripts/nuget-pack.cs --development

# Package to staging feed
dotnet run scripts/nuget-pack.cs --staging

# Package to production feed (NuGet.org)
dotnet run scripts/nuget-pack.cs --prod
dotnet run scripts/nuget-pack.cs --production

# Package with debug configuration
dotnet run scripts/nuget-pack.cs --debug --local
```

### Environment Requirements

- .NET 10 SDK in CI environment for build scripts
- .NET Standard 2.0 compatibility for library builds
- NuGet publishing credentials for package distribution

## 🎨 Code Style and Standards

### Language Features

- **C# 13**: Latest language version with preview features enabled
- **Nullable Reference Types**: Enabled by default for type safety
- **Implicit Usings**: Disabled for explicit control in generator contexts
- **Target Framework**: .NET Standard 2.0 for maximum compatibility; build scripts require .NET 10
- **Unsafe Code**: Enabled for high-performance collection operations

### Code Quality Tools

- **StyleCop**: Automated code style enforcement
- **EditorConfig**: Consistent coding standards
- **Code Analysis**: Built-in rule sets and analysis
- **Treat Warnings as Errors**: High quality standards


## 📝 Commit and PR Guidelines

### Commit Message Format

Follow the **conventional commit format**:

```
type(scope): description

[optional body]

[optional footer]
```

Types: `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`

### Pull Request Process

1. Create a feature branch from the main
2. Make changes following code style guidelines
3. Add/update tests for new functionality
4. Ensure all tests pass and coverage is adequate
5. Update documentation if needed
6. Submit PR with a clear description and linked issues

### Development Workflow

**For New Projects:**
1. Clone the repository
2. Run `dotnet run setup.cs` to configure the project
3. Review and customize the generated configuration files
4. Start development

**Daily Development:**
- Use direct script commands for testing and building
- Run `dotnet run scripts/tests-run.cs` before committing
- Check coverage with `dotnet run scripts/tests-coverage.cs`

### Before Committing

- Run `dotnet build` to ensure solution builds
- Run `dotnet run scripts/tests-run.cs` to verify tests pass
- Check code coverage with `dotnet run scripts/tests-coverage.cs`
- Validate NuGet package builds correctly (`dotnet run scripts/nuget-pack.cs --local`)
- Ensure no sensitive information is included in commits


**Note**: This AGENTS.md file is designed to help AI coding agents work effectively with the Six.GeneratorKit
library. It provides comprehensive guidance for understanding, developing, testing, and packaging source generator utilities while
maintaining the high-quality standards expected in enterprise .NET development and Roslyn source generator ecosystems.
