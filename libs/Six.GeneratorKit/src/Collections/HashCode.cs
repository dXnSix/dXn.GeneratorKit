// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace Six.GeneratorKit;

#pragma warning disable CS0809

/// <summary>
/// A polyfill type that mirrors some methods from <see cref="HashCode"/> on .NET 6.
/// </summary>
[PublicAPI]
public struct HashCode
{
  const uint Prime1 = 2654435761U;
  const uint Prime2 = 2246822519U;
  const uint Prime3 = 3266489917U;
  const uint Prime4 = 668265263U;
  const uint Prime5 = 374761393U;

  static readonly uint _seed = GenerateGlobalSeed();

  uint _v1, _v2, _v3, _v4;
  uint _queue1, _queue2, _queue3;
  uint _length;

  /// <summary>
  /// Initializes the default seed.
  /// </summary>
  /// <returns>A random seed.</returns>
  static unsafe uint GenerateGlobalSeed()
  {
    byte[] bytes = new byte[4];

    using(RandomNumberGenerator generator = RandomNumberGenerator.Create())
    {
      generator.GetBytes(bytes);
    }

    return BitConverter.ToUInt32(bytes, 0);
  }

  /// <summary>
  /// Adds a single value to the current hash.
  /// </summary>
  /// <typeparam name="T">The type of the value to add into the hash code.</typeparam>
  /// <param name="value">The value to add into the hash code.</param>
  public void Add<T>(T value)
  {
    Add(value?.GetHashCode() ?? 0);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  static void Initialize(out uint v1, out uint v2, out uint v3, out uint v4)
  {
    v1 = _seed + Prime1 + Prime2;
    v2 = _seed + Prime2;
    v3 = _seed;
    v4 = _seed - Prime1;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  static uint Round(uint hash, uint input)
  {
    return RotateLeft(hash + input * Prime2, 13) * Prime1;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  static uint QueueRound(uint hash, uint queuedValue)
  {
    return RotateLeft(hash + queuedValue * Prime3, 17) * Prime4;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  static uint MixState(uint v1, uint v2, uint v3, uint v4)
  {
    return RotateLeft(v1, 1) + RotateLeft(v2, 7) + RotateLeft(v3, 12) + RotateLeft(v4, 18);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  static uint MixEmptyState()
  {
    return _seed + Prime5;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  static uint MixFinal(uint hash)
  {
    hash ^= hash >> 15;
    hash *= Prime2;
    hash ^= hash >> 13;
    hash *= Prime3;
    hash ^= hash >> 16;

    return hash;
  }

  void Add(int value)
  {
    uint val = (uint)value;
    uint previousLength = _length++;
    uint position = previousLength % 4;

    if(position == 0)
    {
      _queue1 = val;
    }
    else if(position == 1)
    {
      _queue2 = val;
    }
    else if(position == 2)
    {
      _queue3 = val;
    }
    else
    {
      if(previousLength == 3)
      {
        Initialize(out this._v1, out this._v2, out this._v3, out this._v4);
      }

      _v1 = Round(_v1, _queue1);
      _v2 = Round(_v2, _queue2);
      _v3 = Round(_v3, _queue3);
      _v4 = Round(_v4, val);
    }
  }

  /// <summary>
  /// Gets the resulting hashcode from the current instance.
  /// </summary>
  /// <returns>The resulting hashcode from the current instance.</returns>
  public int ToHashCode()
  {
    uint locaLength = _length;
    uint position = locaLength % 4;
    uint hash = locaLength < 4 ? MixEmptyState() : MixState(this._v1, this._v2, this._v3, this._v4);

    hash += locaLength * 4;

    if(position > 0)
    {
      hash = QueueRound(hash, _queue1);

      if(position > 1)
      {
        hash = QueueRound(hash, _queue2);

        if(position > 2)
        {
          hash = QueueRound(hash, _queue3);
        }
      }
    }

    hash = MixFinal(hash);

    return (int)hash;
  }

  /// <summary>
  /// Rotates the specified value left by the specified number of bits.
  /// Similar in behavior to the x86 instruction ROL.
  /// </summary>
  /// <param name="value">The value to rotate.</param>
  /// <param name="offset">The number of bits to rotate by.
  /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
  /// <returns>The rotated value.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  static uint RotateLeft(uint value, int offset)
  {
    return (value << offset) | (value >> (32 - offset));
  }
}