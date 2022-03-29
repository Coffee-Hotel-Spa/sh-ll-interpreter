using System;
using System.Collections.Generic;
using System.Globalization;

namespace ShaellLang
{
    public class Number : IValue, IKeyable
    {
        private dynamic _numberRepresentation;

        private static NumberTable _numberTable = NumberTable.getInstance();
        
        public Number(long value)
        {
            _numberRepresentation = value;
        }

        public Number(double value)
        {
            _numberRepresentation = value;
        }

        public bool IsInteger => _numberRepresentation is long;

        public bool IsFloating => _numberRepresentation is double;

        public long ToInteger() => Convert.ToInt64(_numberRepresentation);
        public double ToFloating() => Convert.ToDouble(_numberRepresentation);
        public string KeyValue => Convert.ToString(_numberRepresentation);
        public string UniquePrefix => "N";

        public bool ToBool() => true;

        public Number ToNumber()
        {
            return this;
        }

        public IFunction ToFunction()
        {
            throw new Exception("Type error, number cannot be converted to function");
        }

        public SString ToSString()
        {
            if (IsFloating)
            {
                return new SString(ToFloating().ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                return new SString(ToInteger().ToString());
            }
        }

        public ITable ToTable()
        {
            _numberTable.Number = this;
            return _numberTable;
        }

        public static Number operator +(Number a, Number b)
        {
            if (a.IsFloating || b.IsFloating)
            {
                return new Number(a.ToFloating() + b.ToFloating());
            }
            else
            {
                // Does not check for overflow where it should switch to floating
                return new Number(a.ToInteger() + b.ToInteger());
            }
        }
        
        public static Number operator -(Number a, Number b)
        {
            if (a.IsFloating || b.IsFloating)
            {
                return new Number(a.ToFloating() - b.ToFloating());
            }
            else
            {
                // Does not check for overflow where it should switch to floating
                return new Number(a.ToInteger() - b.ToInteger());
            }
        }

        public static Number operator *(Number a, Number b)
        {
            if (a.IsFloating || b.IsFloating)
            {
                return new Number(a.ToFloating() * b.ToFloating());
            }
            // Does not check for overflow where it should switch to floating
            return new Number(a.ToInteger() * b.ToInteger());
        }

        public static Number operator /(Number a, Number b)
        {
            if (a.IsFloating || b.IsFloating || a.ToInteger() % b.ToInteger() != new Number(0).ToInteger())
            {
                return new Number(a.ToFloating() / b.ToFloating());
            }
            // Does not check for overflow where it should switch to floating
            return new Number(a.ToInteger() / b.ToInteger());
        }
        
        public static Number operator %(Number a, Number b)
        {
            if (a.IsFloating || b.IsFloating)
            {
                return new Number(a.ToFloating() % b.ToFloating());
            }
            // Does not check for overflow where it should switch to floating
            return new Number(a.ToInteger() % b.ToInteger());
        }

        public static Number Power(Number a, Number b)
        {
            return new Number(Math.Pow(a.ToFloating(), b.ToFloating()));
        }
        
        //overide unary - and return the negative of the number
        public static Number operator -(Number a)
        {
            if (a.IsFloating)
            {
                return new Number(-a.ToFloating());
            }
            return new Number(-a.ToInteger());
        }
        
        //Implement less than operator and convert to floating and integer comparison correctly
        public static bool operator <(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() < b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() < b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() < b.ToFloating();
            return a.ToInteger() < b.ToInteger();
        }
        
        //Implement greater than operator and convert to floating and integer comparison correctly
        public static bool operator >(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() > b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() > b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() > b.ToFloating();
            return a.ToInteger() > b.ToInteger();
        }

        public static bool operator <=(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() <= b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() <= b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() <= b.ToFloating();
            return a.ToInteger() <= b.ToInteger();
        }
        
        public static bool operator >=(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() >= b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() >= b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() >= b.ToFloating();
            return a.ToInteger() >= b.ToInteger();
        }
        
        public static bool operator ==(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() == b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() == b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() == b.ToFloating();
            return a.ToInteger() == b.ToInteger();
        }

        public static bool operator !=(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() != b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() != b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() != b.ToFloating();
            return a.ToInteger() != b.ToInteger();
        }

        public static bool operator !(Number a)
        {
            return !a.ToBool();
        }
    }
}