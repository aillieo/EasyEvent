namespace AillieoUtils
{
    using System;
    using System.Collections.Generic;

    public class Event
    {
        private int lockCount;

        private Handle head;

        public int ListenerCount { get; private set; }

        public Handle AddListener(Action callback)
        {
            Handle newHandle = new Handle(callback, this);

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

        public Handle ListenOnce(Action callback)
        {
            Handle handle = default;
            handle = AddListener(() =>
            {
                Remove(handle);
                callback?.Invoke();
            });
            return handle;
        }
        
        public bool Remove(Handle handle)
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

        public int RemoveListener(Action callback)
        {
            if (callback == null)
            {
                return 0;
            }

            Handle handle = this.head;
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

        public void Invoke()
        {
            InternalInvoke(false);
        }

        public void SafeInvoke()
        {
            InternalInvoke(true);
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
                    catch(Exception e)
                    {
                        if(!continueOnException)
                        {
                            this.lockCount--;
                            throw;
                        }

                        if(exceptions == null)
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

            if(exceptions != null)
            {
                throw new AggregateException(exceptions);
            }
        }

        //public static Event operator + (Event evt, Action callback)
        //{
        //    evt.AddListener(callback);
        //    return evt;
        //}

        //public static Event operator - (Event evt, Handle handle)
        //{
        //    evt.Remove(handle);
        //    return evt;
        //}

        //public static Event operator - (Event evt, Action callback)
        //{
        //    evt.RemoveListener(callback);
        //    return evt;
        //}
    }

    public class Handle
    {
        internal readonly Event owner;

        internal Action callback;

        internal Handle next;

        internal Handle previous;

        internal Handle(Action callback, Event owner)
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
