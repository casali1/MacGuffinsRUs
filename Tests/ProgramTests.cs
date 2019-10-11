using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewMacGuffinsRUs;

namespace Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void GetCategoryTest()
        {

            var category = Program.GetCategories("");
        }
    }
}
