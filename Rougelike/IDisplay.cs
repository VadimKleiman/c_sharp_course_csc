using System;
namespace Rougelike
{
    public interface IDisplay
    {
        void View(World w, Player p);
    }
}
