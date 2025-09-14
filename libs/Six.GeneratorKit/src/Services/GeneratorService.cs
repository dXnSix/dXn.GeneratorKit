using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Six.GeneratorKit;

/// <summary>
/// <see cref="GeneratorService"/> is a service that can be used to provider generator
/// results and report errors from a source generator.
/// The main idea is to capture all errors and report them at the end of the generation process.
/// </summary>
/// <remarks>
/// This class is excluded from code coverage.
/// </remarks>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static class GeneratorService
{
  static readonly List<Diagnostic> _errors = [];

  /// <summary>
  /// Adds a diagnostic to the list of errors.
  /// </summary>
  /// <param name="descriptor">The <see cref="DiagnosticDescriptor"/>.</param>
  /// <param name="location">The location of the diagnostic. Optional.</param>
  public static void Add(DiagnosticDescriptor descriptor, Location? location = null)
  {
    _errors.Add(Diagnostic.Create(descriptor, location));
  }

  /// <summary>
  /// Clears the list of errors.
  /// </summary>
  public static void Clear()
  {
    _errors.Clear();
  }

  /// <summary>
  /// Reports the list of errors as a <see cref="GeneratorResult{T}"/>.
  /// </summary>
  /// <remarks>
  /// Use it when you don't have a value (fatal errors prior to the generation of the value).
  /// </remarks>
  /// <typeparam name="TValue">The type of the value in the <see cref="GeneratorResult{T}"/>.</typeparam>
  /// <returns>A <see cref="GeneratorResult{T}"/> with the DEFAULT value and list of errors.</returns>
  public static GeneratorResult<TValue> Report<TValue>()
    where TValue : IEquatable<TValue>?
  {
    return new GeneratorResult<TValue>(default!, new EquatableArray<Diagnostic>([.._errors]));
  }

  /// <summary>
  /// Adds a diagnostic to the list of errors and reports the list of errors as a <see cref="GeneratorResult{T}"/>.
  /// </summary>
  /// <remarks>
  /// Use it when you don't have a value (fatal errors prior to the generation of the value) and this would be the
  /// only error since you are cancelling generation.
  /// </remarks>
  /// <typeparam name="TValue">The type of the value in the <see cref="GeneratorResult{T}"/>.</typeparam>
  /// <param name="descriptor">The diagnostic descriptor.</param>
  /// <param name="location">The location of the diagnostic. Optional.</param>
  /// <returns>A <see cref="GeneratorResult{T}"/> with the list of errors.</returns>
  public static GeneratorResult<TValue> Report<TValue>(DiagnosticDescriptor descriptor, Location? location = null)
    where TValue : IEquatable<TValue>?
  {
    _errors.Add(Diagnostic.Create(descriptor, location));
    return new GeneratorResult<TValue>(default!, new EquatableArray<Diagnostic>([.._errors]));
  }

  /// <summary>
  /// Reports a value and the list of cached errors as a <see cref="GeneratorResult{T}"/>.
  /// </summary>
  /// <remarks>
  /// Use it when you have a generated value and you want to report it along with the errors.
  /// This is in most cases the last (return) statement in the generation code.
  /// </remarks>
  /// <typeparam name="TValue">The type of the value in the <see cref="GeneratorResult{T}"/>.</typeparam>
  /// <param name="value">The value to report.</param>
  /// <returns>A <see cref="GeneratorResult{T}"/> with the value and the list of cached errors.</returns>
  public static GeneratorResult<TValue> Report<TValue>(TValue value)
    where TValue : IEquatable<TValue>?
  {
    return new GeneratorResult<TValue>(value, new EquatableArray<Diagnostic>([.._errors]));
  }
}

/// <summary>
/// Represents the result of a generator operation.
/// <br/>
/// Use it as the output of the code generation code so that you can return a value and report
/// errors later in the execution part of the source generator.
/// </summary>
/// <remarks>
/// You should not use this class directly. Use <see cref="GeneratorService"/> instead.
/// </remarks>
/// <typeparam name="TValue">The type of the value in the result.</typeparam>
/// <param name="Value">The value of the result.</param>
/// <param name="Errors">The list of diagnostics that represent errors.</param>
[PublicAPI]
[ExcludeFromCodeCoverage]
public sealed record GeneratorResult<TValue>(TValue Value, EquatableArray<Diagnostic> Errors)
  where TValue : IEquatable<TValue>?;