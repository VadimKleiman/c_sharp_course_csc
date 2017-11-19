using System;
namespace Rougelike
{
    public class MapException : Exception
    {
        public MapException(string message) : base(message)
        {

        }

        public MapException()
        {

        }

        public MapException(string message, Exception e) : base(message, e)
        {

        }
    }
}
