using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace AssertRouting.Exceptions
{
    /// <summary>
    /// Exception class for exceptions triggered by assertions
    /// </summary>
    public class AssertTriggeredException : Exception
    {
        /// <summary>
        /// Creates the exception with a message.
        /// </summary>
        /// <param name="message">The message for the exception</param>
        public AssertTriggeredException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates the exception with a message and a detailed message.
        /// </summary>
        /// <param name="message">The message for the exception</param>
        /// <param name="detailMessage">The detailed message for the exception.</param>
        public AssertTriggeredException(string message, string detailMessage)
            : base(String.Format("{0} ({1})", message, detailMessage))
        {
        }

        /// <summary>
        /// Creates the excetpion with serialisation data.
        /// </summary>
        protected AssertTriggeredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
