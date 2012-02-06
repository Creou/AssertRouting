using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using AssertRouting.Listeners;

namespace AssertRouting
{
    /// <summary>
    /// Class to assist with test methods by disableing the assert UI, and assert excpetion convertor.
    /// </summary>
    public sealed class AssertRouter : IDisposable
    {
        private const string DefaultTraceListener = "Default";

        private bool _disposed;

        private bool _previousAssertUIEnabledValue;
        private bool _previousThrowExcpetionsValue;
        private DefaultTraceListener _defaultListener;
        private ExceptionConvertorListener _exceptionConvetor;

        private bool _restored;

        /// <summary>
        /// Creates an assert router to manage asserts. The specified behaviour will be applied for the lifetime of the object.
        /// </summary>
        /// <param name="assertRoutingBehaviour">The assert reouting behaviour required.</param>
        public AssertRouter(AssertUIBehaviour assertRoutingBehaviour)
        {
            bool disableAssertUI = assertRoutingBehaviour.HasFlag(AssertUIBehaviour.DisableUI);
            bool throwExceptions = assertRoutingBehaviour.HasFlag(AssertUIBehaviour.ThrowExceptions);

            Disable(disableAssertUI, throwExceptions);
        }

        /// <summary>
        /// Disables asserts for the duration of the provided action.
        /// </summary>
        /// <param name="routedAction">The action to disable asserts over</param>
        public static void Disable(Action routedAction)
        {
            Reroute(AssertUIBehaviour.DisableUI, routedAction);
        }

        /// <summary>
        /// Disables asserts and throws exception for the duration of the provided action.
        /// </summary>
        /// <param name="routedAction">The action to disable asserts over</param>
        public static void DisableAndThrow(Action routedAction)
        {
            Reroute(AssertUIBehaviour.DisableUIAndThrowExceptions, routedAction);
        }

        /// <summary>
        /// Reroutes asserts according to the specified behaviour for the duration of the provided action.
        /// </summary>
        /// <param name="assertRoutingBehaviour">The routing behaviour to apply.</param>
        /// <param name="routedAction">The action to disable asserts over.</param>
        public static void Reroute(AssertUIBehaviour assertRoutingBehaviour, Action routedAction)
        {
            bool disableAssertUI = assertRoutingBehaviour.HasFlag(AssertUIBehaviour.DisableUI);
            bool throwExceptions = assertRoutingBehaviour.HasFlag(AssertUIBehaviour.ThrowExceptions);

            AssertRouter router = new AssertRouter(assertRoutingBehaviour);
            try
            {
                routedAction();
            }
            finally
            {
                router.Restore();
            }
        }

        /// <summary>
        /// Method to disable the assertion UI.
        /// </summary>
        private void Disable(bool disableAssertUI, bool throwExceptions)
        {
            // Get the default listener.
            TraceListener listener = Trace.Listeners[DefaultTraceListener];
            _defaultListener = listener as DefaultTraceListener;
            if (_defaultListener != null)
            {
                // Store the listeners previous value.
                _previousAssertUIEnabledValue = _defaultListener.AssertUiEnabled;

                // Disable the assertion UI.
                _defaultListener.AssertUiEnabled = !disableAssertUI;
            }

            // Get the exception convertor listener.
            listener = Trace.Listeners[ExceptionConvertorListener.ExceptionConvertorListnerName];
            _exceptionConvetor = listener as ExceptionConvertorListener;
            if (_exceptionConvetor == null)
            {
                var convertorListener = new ExceptionConvertorListener(throwExceptions);
                _previousThrowExcpetionsValue = false;
                Trace.Listeners.Add(convertorListener);
            }
            else
            {
                _previousThrowExcpetionsValue = _exceptionConvetor.ThrowExceptions;
                _exceptionConvetor.ThrowExceptions = throwExceptions;
            }
        }

        /// <summary>
        /// Method to restore the assertion UI.
        /// </summary>
        public void Restore()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Object already disposed");
            }

            // If we've already restore the listener, don't do it again.
            if (_restored)
            {
                return;
            }

            // Check there is a default listener to restore.
            if (_defaultListener != null)
            {
                _defaultListener.AssertUiEnabled = _previousAssertUIEnabledValue;
            }

            // Restore the throw exceptions value.
            if (_exceptionConvetor != null)
            {
                _exceptionConvetor.ThrowExceptions = _previousThrowExcpetionsValue;
            }

            _restored = true;
        }

        /// <summary>
        /// Class finaliser
        /// </summary>
        ~AssertRouter()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose of the class and restore the previous AssertUIEnabled property.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes resourses
        /// </summary>
        /// <param name="disposing">Flag to indicate if we are disposing.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Ensure we restore the assertion UI when the class is disposed.
                    Restore();
                    GC.SuppressFinalize(this);
                }

                _disposed = true;

                GC.KeepAlive(this);
            }
        }
    }
}
