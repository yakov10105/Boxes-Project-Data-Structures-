using Boxes.Logic;
using NUnit.Framework;

namespace Boxes.UnitTest.LogicTests
{
    [TestFixture]
    class ShowBoxTest
    {
        private Manager _manager;

        [Test]
        public void ShowExistingBoxTest()
        {
            _manager = new Manager(50, 200,30);
            _manager.AddBoxes(3, 3, 1);
            var res = _manager.ShowBox(3, 3);
            Assert.IsNotNull(res);
        }
        [Test]
        public void ShowMissingBoxTest()
        {
            _manager = new Manager(50, 200,30);
            _manager.AddBoxes(3, 3, 1);
            var res = _manager.ShowBox(3, 8);
            Assert.IsNull(res);
        }
    }
}
