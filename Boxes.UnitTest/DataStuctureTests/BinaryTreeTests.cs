using BinaryTree;
using NUnit.Framework;
using System;

namespace Boxes.UnitTest.DataStuctureTests
{
    [TestFixture]
    public class BinaryTreeTests
    {
        BST<int> tree;

        [Test]
        public void FindExistNodeTest()
        {
            tree = new BST<int>();
            tree.Add(15);
            tree.Add(7);
            tree.Add(20);
            tree.Add(11);
            tree.Add(16);
            tree.Add(18);
            tree.Add(4);
            tree.Add(8);

            var res = tree.Find(18);
            Assert.That(res != null);
        }
        [Test]
        public void FindMissingNodeTest()
        {
            tree = new BST<int>();
            tree.Add(15);
            tree.Add(7);
            tree.Add(20);
            tree.Add(11);
            tree.Add(16);
            tree.Add(18);
            tree.Add(4);
            tree.Add(8);

            var res = tree.Find(43);
            Assert.That(res == null);
        }
        [Test]
        public void FindClosestLeftNodeTest()
        {
            tree = new BST<int>();
            tree.Add(15);
            tree.Add(7);
            tree.Add(20);
            tree.Add(11);
            tree.Add(16);
            tree.Add(18);
            tree.Add(4);
            tree.Add(8);

            var res = tree.FindHigherValue(9);
            Assert.That(res==11);
        }
        [Test]
        public void FindClosestRightNodeTest()
        {
            tree = new BST<int>();
            tree.Add(15);
            tree.Add(7);
            tree.Add(20);
            tree.Add(11);
            tree.Add(16);
            tree.Add(18);
            tree.Add(4);
            tree.Add(8);

            var res = tree.FindHigherValue(19);
            Assert.That(res == 20);
        }
        [Test]
        public void FindNotExistingNodeTest()
        {
            tree = new BST<int>();
            tree.Add(15);
            tree.Add(7);
            tree.Add(20);
            tree.Add(11);
            tree.Add(16);
            tree.Add(18);
            tree.Add(4);
            tree.Add(8);

            Assert.Throws<Exception>(() => tree.FindHigherValue(43));
        }
    }
}
