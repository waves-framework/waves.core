// Copyright 2013-2015 Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;

// General-purpose type; not all features are used here.
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global

namespace Fluid.Core.Logging.Context
{
    internal class ImmutableStack<T> : IEnumerable<T>
    {
        private readonly ImmutableStack<T> _under;

        private ImmutableStack()
        {
        }

        private ImmutableStack(ImmutableStack<T> under, T top)
        {
            _under = under ?? throw new ArgumentNullException(nameof(under));
            Count = under.Count + 1;
            Top = top;
        }

        public int Count { get; }

        public static ImmutableStack<T> Empty { get; } = new ImmutableStack<T>();

        public bool IsEmpty => _under == null;

        public T Top { get; }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public ImmutableStack<T> Push(T t)
        {
            return new ImmutableStack<T>(this, t);
        }

        internal struct Enumerator : IEnumerator<T>
        {
            private readonly ImmutableStack<T> _stack;
            private ImmutableStack<T> _top;

            public Enumerator(ImmutableStack<T> stack)
            {
                _stack = stack;
                _top = stack;
                Current = default;
            }

            public bool MoveNext()
            {
                if (_top.IsEmpty)
                    return false;
                Current = _top.Top;
                _top = _top._under;
                return true;
            }

            public void Reset()
            {
                _top = _stack;
                Current = default;
            }

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}