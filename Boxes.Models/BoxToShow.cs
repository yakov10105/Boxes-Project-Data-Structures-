using System;

namespace Boxes.Models
{
    public class BoxToShow
    {
        public Box Box { get; set; }
        public DateTime AddedToStore { get; set; }
        public DateTime LastDeal { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            if (LastDeal != default)
                return $"Width: {Box.Length} , Height: {Box.Height}\nQTY: {Quantity}\nAdded: {AddedToStore:dd-MM-yyyy}\nLast Deal: {LastDeal:dd-MM-yyyy}";
            else
                return $"Width: {Box.Length} , Height: {Box.Height}\nQTY: {Quantity}\nAdded: {AddedToStore:dd-MM-yyyy}";
        }
    }
}
