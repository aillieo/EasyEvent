// -----------------------------------------------------------------------
// <copyright file="EasyDelegate.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Delegate with no argument and no return value.
    /// </summary>
    public class EasyDelegate : IListenable, IInvokable
    {
        private int lockCount;

        private Handle head;

        /// <summary>
        /// Gets the count of listeners currently registered to this event.
        /// </summary>
        public int ListenerCount { get; private set; }

        public static implicit operator EasyEvent(EasyDelegate del)
        {
            return new EasyEvent(del);
        }

        /// <inheritdoc/>
        public EventHandle AddListener(Action callback)
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

        /// <inheritdoc/>
        public bool Remove(EventHandle eventHandle)
        {
            if (eventHandle == null)
            {
                throw new ArgumentNullException(nameof(eventHandle));
            }

            if (!(eventHandle is Handle handle))
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
                if (handle.next == eventHandle)
                {
                    // 1. handle是唯一handle
                    this.head = null;
                }
                else if (this.head == eventHandle)
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

        /// <inheritdoc/>
        public int RemoveListener(Action callback)
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

        /// <inheritdoc/>
        public void Invoke()
        {
            this.InternalInvoke(false);
        }

        /// <inheritdoc/>
        public void InvokeAll()
        {
            this.InternalInvoke(true);
        }

        private void InternalInvoke(bool continueOnException)
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
                        handle.callback();
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
            internal readonly EasyDelegate owner;

            internal Action callback;

            internal Handle next;

            internal Handle previous;

            internal Handle(Action callback, EasyDelegate owner)
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
