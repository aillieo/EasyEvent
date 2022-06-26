// -----------------------------------------------------------------------
// <copyright file="Event`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Event with one generic argument.
    /// </summary>
    /// <typeparam name="T">Type of argument.</typeparam>
    public class Event<T>
    {
        private int lockCount;

        private Handle<T> head;

        /// <summary>
        /// Gets the count of listeners currently registered to this event.
        /// </summary>
        public int ListenerCount { get; private set; }

        /// <summary>
        /// Add listener to this event.
        /// </summary>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public Handle<T> AddListener(Action<T> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            Handle<T> newHandle = new Handle<T>(callback, this);

            if (this.head == null)
            {
                this.head = newHandle;
                this.head.next = this.head;
                this.head.previous = this.head;
            }
            else
            {
                newHandle.next = this.head;
                newHandle.previous = this.head.previous;
                this.head.previous.next = newHandle;
                this.head.previous = newHandle;
            }

            this.ListenerCount++;

            return newHandle;
        }

        /// <summary>
        /// Add listener to this event and remove it after event invoked.
        /// </summary>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public Handle<T> ListenOnce(Action<T> callback)
        {
            Handle<T> handle = default;
            handle = this.AddListener(arg =>
            {
                this.Remove(handle);
                callback?.Invoke(arg);
            });
            return handle;
        }

        /// <summary>
        /// Remove a listener by handle.
        /// </summary>
        /// <param name="handle">Handle for the listener.</param>
        /// <returns>Remove succeed.</returns>
        public bool Remove(Handle<T> handle)
        {
            if (this.head == null)
            {
                return false;
            }

            if (handle == null)
            {
                return false;
            }

            if (handle.owner != this)
            {
                return false;
            }

            if (handle.callback == null)
            {
                return false;
            }

            handle.callback = null;

            if (this.lockCount == 0)
            {
                // 需要考虑3种情况
                if (handle.next == handle)
                {
                    // 1. handle是唯一handle
                    this.head = null;
                }
                else if (this.head == handle)
                {
                    // 2. handle是head
                    handle.next.previous = handle.previous;
                    handle.previous.next = handle.next;
                    this.head = handle.next;
                }
                else
                {
                    // 3. 其它情况
                    handle.next.previous = handle.previous;
                    handle.previous.next = handle.next;
                }

                handle.next = null;
                handle.previous = null;
            }

            this.ListenerCount--;
            return true;
        }

        /// <summary>
        /// Remove a listener.
        /// </summary>
        /// <param name="callback">The listener.</param>
        /// <returns>Count of removed listener instances.</returns>
        public int RemoveListener(Action<T> callback)
        {
            if (callback == null)
            {
                return 0;
            }

            Handle<T> handle = this.head;
            int oldListenerCount = this.ListenerCount;

            if (handle != null)
            {
                while (true)
                {
                    if (handle.callback == callback)
                    {
                        handle.callback = null;
                        this.ListenerCount--;
                    }

                    handle = handle.next;

                    if (handle == null || handle == this.head)
                    {
                        break;
                    }
                }
            }

            return oldListenerCount - this.ListenerCount;
        }

        /// <summary>
        /// Remove all listeners registered.
        /// </summary>
        public void RemoveAllListeners()
        {
            Handle<T> handle = this.head;

            if (handle != null)
            {
                while (true)
                {
                    handle.callback = null;
                    handle = handle.next;
                    if (handle == null || handle == this.head)
                    {
                        break;
                    }
                }
            }

            this.ListenerCount = 0;
        }

        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="arg">Argument for event invoking.</param>
        public void Invoke(T arg)
        {
            this.InternalInvoke(arg, false);
        }

        /// <summary>
        /// Invoke the event, exceptions (if any) will aggregate and throw once.
        /// </summary>
        /// <param name="arg">Argument for event invoking.</param>
        public void SafeInvoke(T arg)
        {
            this.InternalInvoke(arg, true);
        }

        private void InternalInvoke(T arg, bool continueOnException)
        {
            if (this.head == null)
            {
                return;
            }

            List<Exception> exceptions = null;

            this.lockCount++;
            Handle<T> handle = this.head;
            while (true)
            {
                if (handle.callback != null)
                {
                    try
                    {
                        handle.callback(arg);
                    }
                    catch (Exception e)
                    {
                        if (!continueOnException)
                        {
                            this.lockCount--;
                            throw;
                        }

                        if (exceptions == null)
                        {
                            exceptions = new List<Exception>();
                        }

                        exceptions.Add(e);
                    }
                    finally
                    {
                        handle = handle.next;
                    }

                    if (handle == null || handle == this.head)
                    {
                        break;
                    }
                }
                else
                {
                    if (this.lockCount == 1)
                    {
                        // 需要考虑3种情况
                        if (handle.next == handle)
                        {
                            // 1. handle是唯一handle
                            this.head = null;
                            break;
                        }
                        else if (this.head == handle)
                        {
                            // 2. handle是head
                            handle.next.previous = handle.previous;
                            handle.previous.next = handle.next;
                            this.head = handle.next;
                            handle.next = null;
                            handle.previous = null;
                            handle = this.head;
                        }
                        else
                        {
                            // 3. 其它情况
                            handle.next.previous = handle.previous;
                            handle.previous.next = handle.next;
                            Handle<T> next = handle.next;
                            handle.next = null;
                            handle.previous = null;
                            handle = next;
                            if (handle == null || handle == this.head)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            this.lockCount--;

            if (exceptions != null)
            {
                throw new AggregateException(exceptions);
            }
        }
    }

    /// <summary>
    /// A Handle records a registration of a listener.
    /// </summary>
    /// <typeparam name="T">Type of argument.</typeparam>
    public class Handle<T> : IEventHandle
    {
        internal readonly Event<T> owner;

        internal Action<T> callback;

        internal Handle<T> next;

        internal Handle<T> previous;

        internal Handle(Action<T> callback, Event<T> owner)
        {
            this.callback = callback;
            this.owner = owner;
        }

        /// <inheritdoc/>
        public bool Unlisten()
        {
            if (this.callback != null)
            {
                return this.owner.Remove(this);
            }

            return false;
        }
    }
}
