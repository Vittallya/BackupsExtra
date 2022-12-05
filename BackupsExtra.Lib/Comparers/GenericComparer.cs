using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Backups.Comparers
{
    public class GenericComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, object> func;

        public GenericComparer(Func<T, object> func)
        {
            this.func = func;
        }

        public bool Equals([AllowNull] T x, [AllowNull] T y)
        {
            return func(x ?? throw new ArgumentException()).Equals(func(y ?? throw new ArgumentException()));
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return func(obj).GetHashCode();
        }
    }
}
