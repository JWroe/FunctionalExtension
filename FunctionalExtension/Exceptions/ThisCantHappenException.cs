using System;

namespace FunctionalExtension
{
    internal class ThisCantHappenException : Exception
    {
        public ThisCantHappenException() : base("This should not be possible...")
        {
            
        }
    }
}