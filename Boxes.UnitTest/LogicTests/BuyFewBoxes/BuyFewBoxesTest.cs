using NUnit.Framework;
using Boxes.Logic;


namespace Boxes.UnitTest.LogicTests.BuyOneBox
{
    [TestFixture]
    public class BuyFewBoxesTest
    {
        private Manager _manager;

        [Test]
        public void BuyFewExistBoxesTest()
        {
            _manager = new Manager(50, 200, 30);
            _manager.AddBoxes(3, 3, 10);
            _manager.FindBoxesToBuy(3, 3, 10);
            var res2 = _manager.ShowBox(3, 3);
            Assert.IsNull(res2);
        }

        [Test]
        public void BuyFewWithDifferentY()
        {
            _manager = new Manager(50, 200, 30);
            _manager.AddBoxes(3, 3, 3);
            _manager.AddBoxes(3, 4, 3);
            _manager.AddBoxes(3, 4.3, 3);
            _manager.FindBoxesToBuy(3, 3, 9);
            var res1 = _manager.ShowBox(3, 3);
            var res2 = _manager.ShowBox(3, 4);
            var res3 = _manager.ShowBox(3, 4.3);
            Assert.That(res1 == null && res2 == null && res3 == null);
        }
        [Test]
        public void BuyFewWithDifferentX_AndY()
        {
            _manager = new Manager(50, 200, 30);
            _manager.AddBoxes(3, 3, 3);
            _manager.AddBoxes(4, 4, 3);
            _manager.AddBoxes(4.1, 4.3, 3);
            _manager.FindBoxesToBuy(3, 3, 9);
            var res1 = _manager.ShowBox(3, 3);
            var res2 = _manager.ShowBox(4, 4);
            var res3 = _manager.ShowBox(4.1, 4.3);
            Assert.That(res1 == null && res2 == null && res3 == null);
        }

    }
}
