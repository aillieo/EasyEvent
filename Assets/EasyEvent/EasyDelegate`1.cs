// -----------------------------------------------------------------------
// <copyright file="EasyDelegate`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Delegate with one generic argument and no return value.
    /// </summary>
    /// <typeparam name="T">Type of argument.</typeparam>
    public class EasyDelegate<T> : IListenable<T>, IInvokable<T>
    {
        private int lockCount;

        private Handle head;

        /// <summary>
        /// Gets the count of listeners currently registered to this event.
        /// </summary>
        public int ListenerCount { get; private set; }

        public static implicit operator EasyEvent<T>(EasyDelegate<T> del)
        {
            return new EasyEvent<T>(del);
        }

        /// <summary>
        /// Add listener to this event.
        /// </summary>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public EventHandle AddListener(Action<T> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var newHandle = new Handle(callback, this);

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
        /// Remove a listener by handle.
        /// </summary>
        /// <param name="eventHandle">Handle for the listener.</param>
        /// <returns>Remove succeed.</returns>
        public bool Remove(EventHandle eventHandle)
        {
            if (eventHandle == null)
            {
                throw new ArgumentNullException(nameof(eventHandle));
            }

            Handle handle = eventHandle as Handle;
            if (handle == null)
            {
                throw new InvalidOperationException($"Type not match: {eventHandle.GetType()} expected {typeof(Handle)}");
            }

            if (this.head == null)
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
                throw new ArgumentNullException(nameof(callback));
            }

            Handle handle = this.head;
            var oldListenerCount = this.ListenerCount;

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
            Handle handle = this.head;

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
        /// Invoke the event, exceptions (if any) will be aggregated and throw once.
        /// </summary>
        /// <param name="arg">Argument for event invoking.</param>
        public void InvokeAll(T arg)
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
            Handle handle = this.head;
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
                            Handle next = handle.next;
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

        internal class Handle : EventHandle
        {
            internal readonly EasyDelegate<T> owner;

            internal Action<T> callback;

            internal Handle next;

            internal Handle previous;

            internal Handle(Action<T> callback, EasyDelegate<T> owner)
            {
                this.callback = callback;
                this.owner = owner;
            }

            public override bool Unlisten()
            {
                if (this.callback != null)
                {
                    return this.owner.Remove(this);
                }

                return false;
            }
        }
    }
}
