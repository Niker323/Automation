using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
    public class Integer
    {
        public int Value { get; set; }


        public Integer() { }
        public Integer(int value) { Value = value; }


        // Custom cast from "int":
        public static implicit operator Integer(int x) { return new Integer(x); }

        // Custom cast to "int":
        public static implicit operator int(Integer x) { return x == null ? 0 : x.Value; }


        public override string ToString()
        {
            return string.Format("Integer({0})", Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is Integer && Equals((Integer)obj)) || (obj is int && Value == (int)obj);
        }

        public bool Equals(Integer other)
        {
            return Value == (other is null ? 0 : other.Value);
        }

        public static bool operator ==(Integer left, Integer right)
        {
            if (left is null)
                return right is null || right.Value == 0;
            return left.Equals(right);
        }

        public static bool operator !=(Integer left, Integer right)
        {
            return !(left == right);
        }
    }
}
