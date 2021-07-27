using Boxes.Logic;
using NUnit.Framework;
using System;

namespace Boxes.UnitTest
{
    [TestFixture]
    public class AddItemsTest
    {
        private Manager _manager;

        [Test]
        public void AddNewWidthAndNewHeightTest()
        {
            _manager = new Manager(50, 200,30);
            _manager.AddBoxes(3, 3);
            var res = _manager.LengthTree.root.data.SubTree.root.data.Quantity;
            Assert.That(res == 1);
        }
        [Test]
        public void AddNewExistingItemTest()
        {
            _manager = new Manager(50, 200,30);
            _manager.AddBoxes(3, 3);
            _manager.AddBoxes(3, 3);
            var res = _manager.LengthTree.root.data.SubTree.root.data.Quantity;
            Assert.That(res == 2);
        }
        [Test]
        public void AddNewItemWithDifferentYTest()
        {
            _manager = new Manager(50, 200,30);
            _manager.AddBoxes(3, 3);
            _manager.AddBoxes(3, 8);
            var res = _manager.LengthTree.root.data.SubTree.GetDepth();
            Assert.That(res == 2);
        }
        [Test]
        public void AddNewFewSameItemsTest()
        {
            _manager = new Manager(50, 200,30);
            try
            {
                _manager.AddBoxes(3, 3, 300);
            }
            catch (Exception)
            {
                var res = _manager.LengthTree.root.data.SubTree.root.data.Quantity;
                Assert.That(res == 200);
            }
           
        }
    }
}
