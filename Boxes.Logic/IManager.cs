using Boxes.Models;

namespace Boxes.Logic
{
    public interface IManager
    {
        void AddBoxes(double x, double y, int QTY=1);
        BoxToShow ShowBox(double x, double y);
        void FindBoxesToBuy(double x, double y, int QTY = 1);
        void CheckExpires();
    }
}