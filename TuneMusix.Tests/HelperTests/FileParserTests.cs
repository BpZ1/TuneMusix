using Microsoft.VisualStudio.TestTools.UnitTesting;
using TuneMusix.Helpers;

namespace TuneMusix.Tests.HelperTests
{
    [TestClass]
    public class FileParserTests
    {
        [TestMethod]
        public void RemoveControlCharactersTest()
        {
           
            string TestString1 = "Pete\u0012r h\u001Fat\u0005 vie\u001Dl Sp\u0003aß.";
            string TestString1Normal = "Peter hat viel Spaß.";

            string TestString2 = "Hans würgt se\u0012ine Oma \u0003wenn sie ihm \u001Dden Döner n\u0003icht gibt.";
            string TestString2Normal = "Hans würgt seine Oma wenn sie ihm den Döner nicht gibt.";

            string TestString3 = null;

            string TestString4 = "\u0003\u0003\u0003\u0003\u0012\u0012\u0012\u0012\u001D\u001D";

            FileParser fp = new FileParser();

            Assert.AreEqual(fp.RemoveControlCharacters(TestString1), TestString1Normal);
            Assert.AreEqual(fp.RemoveControlCharacters(TestString2), TestString2Normal);
            Assert.AreEqual(fp.RemoveControlCharacters(TestString3),"");
            Assert.AreEqual(fp.RemoveControlCharacters(TestString4),"");
        }
    }
}
