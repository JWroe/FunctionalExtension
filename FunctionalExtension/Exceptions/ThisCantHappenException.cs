using System;

namespace FunctionalExtension.Exceptions
{
    internal class ThisCantHappenException : Exception
    {
        public ThisCantHappenException() : base("This should not be possible...")
        {
        }
    }
}