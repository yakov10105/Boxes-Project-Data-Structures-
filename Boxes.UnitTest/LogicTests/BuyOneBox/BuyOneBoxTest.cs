using Boxes.Logic;
using NUnit.Framework;

namespace Boxes.UnitTest.LogicTests
{
    [TestFixture]
    class BuyOneBoxTest
    {
        private Manager _manager;

        [Test]
        public void BuyOneExistBoxTest()
        {
            _manager = new Manager(50, 200,30);
            _manager.AddBoxes(3, 3, 1);
            _manager.FindBoxesToBuy(3, 3, 1);
            var res2 = _manager.ShowBox(3, 3);
            Assert.IsNull(res2);
        }

        [Test]
        public void BuyOneMissingBoxTest() //without bigger fittable box
        {
            _manager = new Manager(50, 200,30);
            _manager.AddBoxes(3, 3);
            _manager.FindBoxesToBuy(3, 8);
            Assert.That(_manager.FittableBoxes == 0);
        }

        [Test]
        public void BuyOneMissingYBoxTest() //with bigger Y fittable box
        {
            _manager = new Manager(50, 200,30);
            _manager.AddBoxes(3, 8);
            _manager.FindBoxesToBuy(3, 6);
            var res = _manager.ShowBox(3, 8);
            Assert.IsNull(res);
        }

        [Test]
        public void BuyOneMissingParametersBoxTest() //with bigger X and Y fittable box
        {
            _manager = new Manager(50, 200,30);
            _manager.AddBoxes(3, 8);
            _manager.FindBoxesToBuy(2, 7);
            var res = _manager.ShowBox(3, 8);
            Assert.IsNull(res);
        }

    }
}
