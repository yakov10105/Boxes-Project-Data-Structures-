using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryTree;

namespace treeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BST<double> tree = new BST<double>();
            tree.Add(5);
            tree.Add(2);
            tree.Add(10);
            tree.Add(1);
            tree.Add(3);
            tree.Add(7);
            tree.Add(11);
            tree.Add(0);
            tree.Add(1.5);
            tree.Add(2.5);
            tree.Add(4);
            tree.Add(6);
            tree.Add(8);
            tree.Add(10.5);
            tree.Add(12);

            tree.Remove(2);
        }
    }
}
