using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using AssertRouting.Exceptions;

namespace AssertRouting.Listeners
{
    /// <summary>
    /// Trace listner that will convert assetion fails to exceptions.
    /// </summary>
    public class ExceptionConvertorListener : TraceListener
    {
        /// <summary>
        /// The trace listners name.
        /// </summary>
        public const string ExceptionConvertorListnerName = "ExceptionConvertorListner";

        private bool _throwExceptions = true;

        /// <summary>
        /// Default constructor for the Exception convertor listner.
        /// </summary>
        public ExceptionConvertorListener(bool throwExceptions)
        {
            base.Name = ExceptionConvertorListnerName;
            this.ThrowExceptions = throwExceptions;
        }

        /// <summary>
        /// Gets/sets flag to indicate if exceptions should be thrown.
        /// </summary>
        public bool ThrowExceptions
        {
            get
            {
                return _throwExceptions;
            }
            set
            {
                _throwExceptions = value;
            }
        }

        /// <summary>
        /// Emits an exception when an assertion fails.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public override void Fail(string message)
        {
            if (_throwExceptions)
            {
                throw new AssertTriggeredException(message);
            }
        }

        /// <summary>
        /// Emits an exception when an assertion fails.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="detailMessage">A more detailed message to be included in the exception.</param>
        public override void Fail(string message, string detailMessage)
        {
            if (_throwExceptions)
            {
                throw new AssertTriggeredException(message, detailMessage);
            }
        }

        /// <summary>
        /// Does nothing. This trace listner only handles assertion failures.
        /// </summary>
        /// <param name="message">The message to be written</param>
        public override void Write(string message)
        {
            // Do nothing with normal trace messages, this listner is only interested in assertion failures.
        }

        /// <summary>
        /// Does nothing. This trace listner only handles assertion failures.
        /// </summary>
        /// <param name="message">The message to be written</param>
        public override void WriteLine(string message)
        {
            // Do nothing with normal trace messages, this listner is only interested in assertion failures.
        }
    }
}
