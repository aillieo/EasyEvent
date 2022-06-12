namespace AillieoUtils
{
    using System;
    using System.Collections.Generic;

    public static class EventCenter
    {
        private static readonly Dictionary<string, Event<object>> mappings = new Dictionary<string, Event<object>>();

        public static Handle<object> AddListener(string eventDef, Action callback)
        {
            return GetEvent(eventDef, true).AddListener(_ => callback());
        }

        public static Handle<object> AddListener(string eventDef, Action<object> callback)
        {
            return GetEvent(eventDef, true).AddListener(callback);
        }

        public static Handle<object> ListenOnece(string eventDef, Action callback)
        {
            return GetEvent(eventDef, true)?.ListenOnce(_ => callback());
        }

        public static Handle<object> ListenOnece(string eventDef, Action<object> callback)
        {
            return GetEvent(eventDef, true)?.ListenOnce(callback);
        }

        public static bool Remove(string eventDef, Handle<object> handle)
        {
            Event<object> evt = GetEvent(eventDef, false);
            if (evt != null)
            {
                return evt.Remove(handle);
            }

            return false;
        }

        public static int RemoveListener(string eventDef, Action<object> callback)
        {
            Event<object> evt = GetEvent(eventDef, false);
            if (evt != null)
            {
                return evt.RemoveListener(callback);
            }

            return 0;
        }

        public static void RemoveAllListeners(string eventDef)
        {
            GetEvent(eventDef, false)?.RemoveAllListeners();
        }

        public static void Invoke(string eventDef, object arg = null)
        {
            GetEvent(eventDef, false)?.Invoke(arg);
        }

        public static void Clear()
        {
            foreach (var pair in mappings)
            {
                pair.Value.RemoveAllListeners();
            }

            mappings.Clear();
        }

        private static Event<object> GetEvent(string eventDef, bool createIfNotExist)
        {
            if (string.IsNullOrEmpty(eventDef))
            {
                throw new ArgumentException($"Argument null or empty: {nameof(eventDef)}");
            }

            if (!mappings.TryGetValue(eventDef, out Event<object> evt) && createIfNotExist)
            {
                evt = new Event<object>();
                mappings.Add(eventDef, evt);
            }

            return evt;
        }
    }
}
