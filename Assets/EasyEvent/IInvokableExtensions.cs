// -----------------------------------------------------------------------
// <copyright file="IInvokableExtensions.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;

    /// <summary>
    /// Extension methods for <see cref="IInvokable"/> and <see cref="IInvokable{T}"/>.
    /// </summary>
    public static class IInvokableExtensions
    {
        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="invokable">The <see cref="IInvokable"/> instance.</param>
        /// <param name="errorHandler">Handler for exceptions.</param>
        /// <returns>Invocation succeeds with no exceptions.</returns>
        public static bool SafeInvoke(this IInvokable invokable, Action<Exception> errorHandler = null)
        {
            try
            {
                invokable.Invoke();
            }
            catch (Exception exception)
            {
                if (errorHandler == null)
                {
                    UnityEngine.Debug.LogException(exception);
                }
                else
                {
                    try
                    {
                        errorHandler(exception);
                    }
                    catch (Exception handlerException)
                    {
                        UnityEngine.Debug.LogException(handlerException);
                    }
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Invoke the event, exceptions (if any) will be aggregated and throw once.
        /// </summary>
        /// <param name="invokable">The <see cref="IInvokable"/> instance.</param>
        /// <param name="errorHandler">Handler for exceptions.</param>
        /// <returns>Invocation succeeds with no exceptions.</returns>
        public static bool SafeInvokeAll(this IInvokable invokable, Action<Exception> errorHandler = null)
        {
            try
            {
                invokable.InvokeAll();
            }
            catch (Exception exception)
            {
                if (errorHandler == null)
                {
                    UnityEngine.Debug.LogException(exception);
                }
                else
                {
                    try
                    {
                        errorHandler(exception);
                    }
                    catch (Exception handlerException)
                    {
                        UnityEngine.Debug.LogException(handlerException);
                    }
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="invokable">The <see cref="IInvokable{T}"/> instance.</param>
        /// <param name="arg">Argument for event invoking.</param>
        /// <param name="errorHandler">Handler for exceptions.</param>
        /// <typeparam name="T">Event arg for <see cref="IInvokable{T}"/>.</typeparam>
        /// <returns>Invocation succeeds with no exceptions.</returns>
        public static bool SafeInvoke<T>(this IInvokable<T> invokable, T arg, Action<Exception> errorHandler = null)
        {
            try
            {
                invokable.Invoke(arg);
            }
            catch (Exception exception)
            {
                if (errorHandler == null)
                {
                    UnityEngine.Debug.LogException(exception);
                }
                else
                {
                    try
                    {
                        errorHandler(exception);
                    }
                    catch (Exception handlerException)
                    {
                        UnityEngine.Debug.LogException(handlerException);
                    }
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Invoke the event, exceptions (if any) will be aggregated and throw once.
        /// </summary>
        /// <param name="invokable">The <see cref="IInvokable{T}"/> instance.</param>
        /// <param name="arg">Argument for event invoking.</param>
        /// <param name="errorHandler">Handler for exceptions.</param>
        /// <typeparam name="T">Event arg for <see cref="IInvokable{T}"/>.</typeparam>
        /// <returns>Invocation succeeds with no exceptions.</returns>
        public static bool SafeInvokeAll<T>(this IInvokable<T> invokable, T arg, Action<Exception> errorHandler = null)
        {
            try
            {
                invokable.InvokeAll(arg);
            }
            catch (Exception exception)
            {
                if (errorHandler == null)
                {
                    UnityEngine.Debug.LogException(exception);
                }
                else
                {
                    try
                    {
                        errorHandler(exception);
                    }
                    catch (Exception handlerException)
                    {
                        UnityEngine.Debug.LogException(handlerException);
                    }
                }

                return false;
            }

            return true;
        }
    }
}
