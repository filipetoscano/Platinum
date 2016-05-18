namespace Platinum.Algorithm
{
    internal class LP
    {
        public LP( char digit, int value, int f1, int f2 )
        {
            Digit = digit;
            Value = value;
            F1 = f1;
            F2 = f2;
        }

        public char Digit
        {
            get;
            private set;
        }

        public int Value
        {
            get;
            private set;
        }

        public int F1
        {
            get;
            private set;
        }

        public int F2
        {
            get;
            private set;
        }
    }
}

/* eof */