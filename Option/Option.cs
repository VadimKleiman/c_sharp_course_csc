using System;

namespace OptionNS
{
    public sealed class Option<T>
    {
        public static Option<T> Some(T value) => new Option<T>(value, true);

        public static Option<T> None() => _none;

        public static Option<T> Flatten(Option<Option<T>> o)
        {
            if (o == null)
            {
                return _none;
            }
            return o.Value;
        }

        public bool IsNone => !IsSome; 

        public bool IsSome => _hasValue;

        public T Value
        {
            get
            {
                if (IsNone)
                {
                    throw new OptionException("Value is none!");
                }
                return _value;
            }
        }

        public Option<TResult> Map<TResult>(Func<T, TResult> func) => IsNone ?
            Option<TResult>.None() : Option<TResult>.Some(func(_value));

        public override int GetHashCode() =>
            _value.GetHashCode() ^ _hasValue.GetHashCode();

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            var other = obj as Option<T>;
            if ((object)other == null)
            {
                return false;
            }
            if (IsNone == other.IsNone)
            {
                return true;
            }
            return other.Value.Equals(_value);
        }

        private Option(T val, bool isNone)
        {
            _value = val;
            _hasValue = isNone;
        }

        private readonly T _value;

        private readonly bool _hasValue;

        private static readonly Option<T> _none = new Option<T>(default(T), false);
    }
}
