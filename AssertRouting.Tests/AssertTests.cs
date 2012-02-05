﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;
using AssertRouting.Exceptions;

namespace AssertRouting.Tests
{
    [TestClass]
    public class AssertTests
    {
        [TestMethod]
        public void SupressAssert()
        {
            // This should supress the assertion UI.
            // If it doesn't the test will display the UI and hang.
            using (AssertRouter s = new AssertRouter(AssertUIBehaviour.DisableUI))
            {
                Debug.Fail("Assert");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AssertTriggeredException))]
        public void ConvertAssertToException()
        {
            using (AssertRouter e = new AssertRouter(AssertUIBehaviour.DisableUIAndThrowExceptions))
            {
                Debug.Fail("Assert");
            }
        }

        [TestMethod]
        public void InnerSupressorRestoresPreviousOffSettings()
        {
            using (AssertRouter outerSupressor = new AssertRouter(AssertUIBehaviour.DisableUI))
            {
                bool innerSupressorTriggersException = false;
                try
                {
                    using (AssertRouter innerSupressor = new AssertRouter(AssertUIBehaviour.DisableUIAndThrowExceptions))
                    {
                        Debug.Fail("Assert");
                    }
                }
                catch (AssertTriggeredException)
                {
                    innerSupressorTriggersException = true;
                }
                Assert.IsTrue(innerSupressorTriggersException, "The local supressor should have triggered an exception");

                // The inner supressor should restore the original settings,
                // so this should not throw an exception.
                Debug.Fail("Assert");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AssertTriggeredException))]
        public void InnerSupressorRestoresPreviousSameSettings()
        {
            using (AssertRouter outerSupressor = new AssertRouter(AssertUIBehaviour.DisableUIAndThrowExceptions))
            {
                bool innerSupressorTriggersException = false;
                try
                {
                    using (AssertRouter innerSupressor = new AssertRouter(AssertUIBehaviour.DisableUIAndThrowExceptions))
                    {
                        Debug.Fail("Assert");
                    }
                }
                catch (AssertTriggeredException)
                {
                    innerSupressorTriggersException = true;
                }
                Assert.IsTrue(innerSupressorTriggersException, "The local supressor should have triggered an exception");

                // The inner supressor should restore the original settings,
                // so this should still throw an exception.
                Debug.Fail("Assert");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AssertTriggeredException))]
        public void InnerSupressorRestoresPreviousOnSettings()
        {
            using (AssertRouter outerSupressor = new AssertRouter(AssertUIBehaviour.DisableUIAndThrowExceptions))
            {
                bool innerSupressorTriggersException = false;
                try
                {
                    using (AssertRouter innerSupressor = new AssertRouter(AssertUIBehaviour.DisableUI))
                    {
                        Debug.Fail("Assert");
                    }
                }
                catch (AssertTriggeredException)
                {
                    innerSupressorTriggersException = true;
                }
                Assert.IsFalse(innerSupressorTriggersException, "The local supressor should not have triggered an exception");

                // The inner supressor should restore the original settings,
                // so this should still throw an exception.
                Debug.Fail("Assert");
            }
        }
    }
}
