using System;

namespace Option
{
    public sealed class Option<T>
    {
        static public Option<T> Some(T value) => new Option<T>(value, true);

        static public Option<T> None() => new Option<T>(default(T), false);

        static public Option<T> Flatten(Option<Option<T>> o) => o.Value;

        public bool IsNone { get => !IsSome; }

        public bool IsSome { get => isValue; }

        public T Value
        {
            get
            {
                if (IsNone)
                {
                    throw new Exception();
                }
                return value;
            }
        }

        public Option<U> Map<U>(Func<T, U> func) => IsNone ?
            Option<U>.None() : Option<U>.Some(func(value));

        public override int GetHashCode() =>
            value.GetHashCode() ^ isValue.GetHashCode();

        public override bool Equals(object obj)
        {
            if ((object)this == obj)
            {
                return true;
            }
            var other = obj as Option<T>;
            if ((object)other == null)
            {
                return false;
            }
            if (IsNone == other.IsNone == true)
            {
                return true;
            }
            return other.Value.Equals(value);
        }

        private Option(T val, bool isNone)
        {
            value = val;
            isValue = isNone;
        }

        private readonly T value;

        private readonly bool isValue;
    }
}
