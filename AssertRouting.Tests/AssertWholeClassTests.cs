using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using AssertRouting.Exceptions;

namespace AssertRouting.Tests
{
    [TestClass]
    public class AssertWholeClassTests
    {
        private static AssertRouter _exceptionConvertor;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _exceptionConvertor = new AssertRouter(AssertUIBehaviour.DisableUIAndThrowExceptions);
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            _exceptionConvertor.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertTriggeredException))]
        public void AssertConvertedToException()
        {
            Debug.Fail("Assert");
        }

        [TestMethod]
        [ExpectedException(typeof(AssertTriggeredException))]
        public void LocalSupressorRestoresPreviousSameSettings()
        {
            bool localSupressorTriggersException = false;
            try
            {
                using (AssertRouter localSupressor = new AssertRouter(AssertUIBehaviour.DisableUIAndThrowExceptions))
                {
                    Debug.Fail("Assert");
                }
            }
            catch (AssertTriggeredException)
            {
                localSupressorTriggersException = true;
            }
            Assert.IsTrue(localSupressorTriggersException, "The local supressor should have triggered an exception");
            
            // The local supressor should restore the original settings,
            // (which is that of the global supressor), so this should 
            // still throw an exception.
            Debug.Fail("Assert");
        }


        [TestMethod]
        [ExpectedException(typeof(AssertTriggeredException))]
        public void LocalSupressorRestoresPreviousOffSettings()
        {
            bool localSupressorTriggersException = false;
            try
            {
                using (AssertRouter localSupressor = new AssertRouter(AssertUIBehaviour.DisableUI))
                {
                    Debug.Fail("Assert");
                }
            }
            catch (AssertTriggeredException)
            {
                localSupressorTriggersException = true;
            }
            Assert.IsFalse(localSupressorTriggersException, "The local supressor should not have triggered an exception");

            // The local supressor should restore the original settings,
            // (which is that of the global supressor), so this should 
            // still throw an exception.
            Debug.Fail("Assert");
        }
    }
}
