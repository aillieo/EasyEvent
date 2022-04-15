namespace AillieoUtils
{
    using System;
    using System.Collections.Generic;

    public static class EventCenter
    {
        private static readonly Dictionary<string, Event> mappings = new Dictionary<string, Event>();
        private static readonly Dictionary<Type, Dictionary<string, object>> mappings_1 = new Dictionary<Type, Dictionary<string, object>>();

        public static Handle AddListener(string eventDef, Action callback)
        {
            return GetEvent(eventDef, true).AddListener(callback);
        }

        public static Handle<T> AddListener<T>(string eventDef, Action<T> callback)
        {
            return GetEvent<T>(eventDef, true).AddListener(callback);
        }

        public static Handle ListenOnece(string eventDef, Action callback)
        {
            return GetEvent(eventDef, true)?.ListenOnce(callback);
        }

        public static Handle<T> ListenOnece<T>(string eventDef, Action<T> callback)
        {
            return GetEvent<T>(eventDef, true)?.ListenOnce(callback);
        }

        public static bool Remove(string eventDef, Handle handle)
        {
            Event evt = GetEvent(eventDef, false);
            if (evt != null)
            {
                return evt.Remove(handle);
            }

            return false;
        }

        public static bool Remove<T>(string eventDef, Handle<T> handle)
        {
            Event<T> evt = GetEvent<T>(eventDef, false);
            if (evt != null)
            {
                return evt.Remove(handle);
            }

            return false;
        }

        public static int RemoveListener(string eventDef, Action callback)
        {
            Event evt = GetEvent(eventDef, false);
            if (evt != null)
            {
                return evt.RemoveListener(callback);
            }

            return 0;
        }

        public static int RemoveListener<T>(string eventDef, Action<T> callback)
        {
            Event<T> evt = GetEvent<T>(eventDef, false);
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

        public static void RemoveAllListeners<T>(string eventDef)
        {
            GetEvent<T>(eventDef, false)?.RemoveAllListeners();
        }

        public static void Invoke(string eventDef)
        {
            GetEvent(eventDef, false)?.Invoke();
        }

        public static void Invoke<T>(string eventDef, T arg)
        {
            GetEvent<T>(eventDef, false)?.Invoke(arg);
        }

        public static void Clear()
        {
            mappings.Clear();
            mappings_1.Clear();
        }

        private static Event GetEvent(string eventDef, bool createIfNotExist)
        {
            if (!mappings.TryGetValue(eventDef, out Event evt) && createIfNotExist)
            {
                evt = new Event();
                mappings.Add(eventDef, evt);
            }

            return evt;
        }

        private static Event<T> GetEvent<T>(string eventDef, bool createIfNotExist)
        {
            if (!mappings_1.TryGetValue(typeof(T), out Dictionary<string, object> innerMap) && createIfNotExist)
            {
                innerMap = new Dictionary<string, object>();
                mappings_1.Add(typeof(T), innerMap);
            }

            if (!innerMap.TryGetValue(eventDef, out object evt) && createIfNotExist)
            {
                evt = new Event<T>();
                innerMap.Add(eventDef, evt);
            }

            return evt as Event<T>;
        }
    }
}
