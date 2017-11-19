using System;
namespace Rougelike
{
    public class PlayerException : Exception
    {
        public PlayerException(string message) : base(message)
        {
            
        }

        public PlayerException()
        {
            
        }

        public PlayerException(string message, Exception e) : base(message, e)
        {
            
        }
    }
}
