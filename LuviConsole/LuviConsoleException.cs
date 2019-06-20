using System;

namespace LuviKunG
{
    public class LuviConsoleException : Exception
    {
        public LuviConsoleException() { }
        public LuviConsoleException(string message) : base(message) { }
    }
}