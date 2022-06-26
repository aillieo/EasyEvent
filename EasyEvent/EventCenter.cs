// -----------------------------------------------------------------------
// <copyright file="EventCenter.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Manage events by string key.
    /// </summary>
    public static class EventCenter
    {
        private static readonly Dictionary<string, Event<object>> mappings = new Dictionary<string, Event<object>>();

        /// <summary>
        /// Add listener to the named event.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> AddListener(string eventDef, Action callback)
        {
            return GetEvent(eventDef, true).AddListener(_ => callback());
        }

        /// <summary>
        /// Add listener to the named event.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> AddListener(string eventDef, Action<object> callback)
        {
            return GetEvent(eventDef, true).AddListener(callback);
        }

        /// <summary>
        /// Add listener to this event and remove it after event invoked.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> ListenOnece(string eventDef, Action callback)
        {
            return GetEvent(eventDef, true)?.ListenOnce(_ => callback());
        }

        /// <summary>
        /// Add listener to this event and remove it after event invoked.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> ListenOnece(string eventDef, Action<object> callback)
        {
            return GetEvent(eventDef, true)?.ListenOnce(callback);
        }

        /// <summary>
        /// Remove a listener by handle.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="handle">Handle for the listener.</param>
        /// <returns>Remove succeed.</returns>
        public static bool Remove(string eventDef, Handle<object> handle)
        {
            Event<object> evt = GetEvent(eventDef, false);
            if (evt != null)
            {
                return evt.Remove(handle);
            }

            return false;
        }

        /// <summary>
        /// Remove a listener.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">The listener.</param>
        /// <returns>Count of removed listener instances.</returns>
        public static int RemoveListener(string eventDef, Action<object> callback)
        {
            Event<object> evt = GetEvent(eventDef, false);
            if (evt != null)
            {
                return evt.RemoveListener(callback);
            }

            return 0;
        }

        /// <summary>
        /// Remove all listeners registered.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        public static void RemoveAllListeners(string eventDef)
        {
            GetEvent(eventDef, false)?.RemoveAllListeners();
        }

        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="arg">Argument for event invoking.</param>
        public static void Invoke(string eventDef, object arg = null)
        {
            GetEvent(eventDef, false)?.Invoke(arg);
        }

        /// <summary>
        /// Remove all listeners registered to all events managed by <see cref="EventCenter"/>.
        /// </summary>
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
