namespace AillieoUtils
{
    using System;
    using System.Collections.Generic;

    public class Event<T>
    {
        private int lockCount;

        private Handle<T> head;

        public int ListenerCount { get; private set; }

        public Handle<T> AddListener(Action<T> callback)
        {
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
        
        public Handle<T> ListenOnce(Action<T> callback)
        {
            Handle<T> handle = default;
            handle = AddListener(arg =>
            {
                Remove(handle);
                callback?.Invoke(arg);
            });
            return handle;
        }
        
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

        public int RemoveListener(Action<T> callback)
        {
            if (callback == null)
            {
                return 0;
            }

            Handle<T> handle = this.head;
            int oldListenerCount = ListenerCount;

            if (handle != null)
            {
                while (true)
                {
                    if (handle.callback == callback)
                    {
                        handle.callback = null;
                        ListenerCount--;
                    }

                    handle = handle.next;
                    
                    if (handle == null || handle == this.head)
                    {
                        break;
                    }
                }
            }

            return oldListenerCount - ListenerCount;
        }

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

        public void Invoke(T arg)
        {
            InternalInvoke(arg, false);
        }

        public void SafeInvoke(T arg)
        {
            InternalInvoke(arg, true);
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
                    catch(Exception e)
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
                            continue;
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

            if(exceptions != null)
            {
                throw new AggregateException(exceptions);
            }
        }

        //public static Event<T> operator + (Event<T> evt, Action<T> callback)
        //{
        //    evt.AddListener(callback);
        //    return evt;
        //}

        //public static Event<T> operator - (Event<T> evt, Handle<T> handle)
        //{
        //    evt.Remove(handle);
        //    return evt;
        //}

        //public static Event<T> operator - (Event<T> evt, Action<T> callback)
        //{
        //    evt.RemoveListener(callback);
        //    return evt;
        //}
    }

    public class Handle<T>
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
