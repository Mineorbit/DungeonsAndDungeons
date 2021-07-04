using System;
using JetBrains.Annotations;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class CustomEnum
    {
        public int cardinal;


        public string Value;

        public CustomEnum(string val, int card = 0)
        {
            Value = val;
            cardinal = card;
        }

        public CustomEnum(int card)
        {
            Value = "";
            cardinal = card;
        }

        public int Cardinal()
        {
            return cardinal;
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals([CanBeNull] System.Object customEnum)
        {
            if (customEnum.GetType() != this.GetType())
            {
                return false;
            }
            return this.cardinal == ((CustomEnum) customEnum).cardinal;
        }

        public override int GetHashCode()
        {
            return cardinal;
        }
        
        public static bool operator ==(CustomEnum a, CustomEnum b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(CustomEnum a, CustomEnum b)
        {
            return !a.Equals(b);
        }
    }
}