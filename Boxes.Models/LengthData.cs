using BinaryTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Boxes.Models
{
    [Serializable]
    public class LengthData : IComparable<LengthData>
    {
        public double X { get; set; }
        public BST<HeightData> SubTree { get; set; }
        public bool IsAlreadyChecked { get; set; } = false;
        public LengthData()
        {
            SubTree = new BST<HeightData>();
        }

        public int CompareTo(LengthData other)
        {
            if (this.X > other.X) return 1;
            else if (this.X < other.X) return -1;
            else return 0;
        }
        public override string ToString()
        {
            var list = new List<HeightData>();
            SubTree.ActInOrder(list.Add);
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendLine("\t>>>"+item.ToString());
            }
            return $"X: {X}\n{sb}";
        }
    }
}
