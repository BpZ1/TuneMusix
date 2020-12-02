using NUnit.Framework;
using TuneMusix.Helpers;

namespace TuneMusix.Tests.HelperTests
{
    [TestFixture]
    public class ObservableValueTests
    {
        [Test]
        public void ValueChanged_NotifyInvoked()
        {
            string value = "test";
            string value2 = "test2";
            bool wasNotified = false;
            var testValue = new ObservableValue<string>( value );
            Assert.AreEqual( value, testValue.Value );
            testValue.PropertyChanged += ( obj, args ) =>
            {
                wasNotified = true;
            };
            testValue.Value = value2;
            Assert.IsTrue( wasNotified );
            Assert.AreEqual( value2, testValue.Value );
        }

        [Test]
        public void ValueChanged_SetterActionWasInvoked()
        {
            string value = "test";
            string value2 = "test2";
            bool wasInvoked = false;
            var testValue = new ObservableValue<string>( value, () =>
            {
                wasInvoked = true;
            } );
            testValue.Value = value2;
            Assert.AreEqual( value2, testValue.Value );
            Assert.IsTrue( wasInvoked );
        }
    }
}
