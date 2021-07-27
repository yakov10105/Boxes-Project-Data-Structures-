
using System;
using System.Collections.Generic;

namespace Boxes.Models
{
    [Serializable]
    public class HeightData : IComparable<HeightData>
    {
        public double Y { get; set; }
        public int Quantity { get; set; } 
        public int TmpQuantity { get; set; }
        public int QtyToDelete { get; set; }
        public bool IsAlreadyChecked { get; set; } = false;

        //double linked list node pointer
        public LinkedListNode<TimeData> TimeDataNode { get; set; }
        public LengthData Father { get; set; }

        public int CompareTo(HeightData other)
        {
            if (this.Y > other.Y) return 1;
            else if (this.Y < other.Y) return -1;
            else return 0;
        }
        public override string ToString()
        {
            return $"Y: {Y} ; Quantity: {Quantity}";
        }
    }
}
