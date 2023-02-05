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
    public class EventCenter
    {
        internal readonly Dictionary<string, Event<object>> mappings = new Dictionary<string, Event<object>>(StringComparer.Ordinal);

        private static readonly EventCenter defaultInstance = new EventCenter();

        /// <summary>
        /// Add listener to the named event.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> AddListener(string eventDef, Action callback)
        {
            return defaultInstance.AddListener(eventDef, callback);
        }

        /// <summary>
        /// Add listener to the named event.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> AddListener(string eventDef, Action<object> callback)
        {
            return defaultInstance.AddListener(eventDef, callback);
        }

        /// <summary>
        /// Add listener to this event and remove it after event invoked.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> ListenOnce(string eventDef, Action callback)
        {
            return defaultInstance.ListenOnce(eventDef, callback);
        }

        /// <summary>
        /// Add listener to this event and remove it after event invoked.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> ListenOnce(string eventDef, Action<object> callback)
        {
            return defaultInstance.ListenOnce(eventDef, callback);
        }

        /// <summary>
        /// Remove a listener by handle.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="handle">Handle for the listener.</param>
        /// <returns>Remove succeed.</returns>
        public static bool Remove(string eventDef, Handle<object> handle)
        {
            return defaultInstance.Remove(eventDef, handle);
        }

        /// <summary>
        /// Remove a listener.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">The listener.</param>
        /// <returns>Count of removed listener instances.</returns>
        public static int RemoveListener(string eventDef, Action<object> callback)
        {
            return defaultInstance.RemoveListener(eventDef, callback);
        }

        /// <summary>
        /// Remove all listeners registered.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        public static void RemoveAllListeners(string eventDef)
        {
            defaultInstance.RemoveAllListeners(eventDef);
        }

        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="arg">Argument for event invoking.</param>
        public static void Invoke(string eventDef, object arg = null)
        {
            defaultInstance.Invoke(eventDef, arg);
        }

        /// <summary>
        /// Invoke the event, exceptions (if any) will be aggregated and throw once.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="arg">Argument for event invoking.</param>
        public static void InvokeAll(string eventDef, object arg = null)
        {
            defaultInstance.InvokeAll(eventDef, arg);
        }

        /// <summary>
        /// Remove all listeners registered to all events managed by <see cref="EventCenter"/>.
        /// </summary>
        public static void Clear()
        {
            defaultInstance.Clear();
        }

        internal Event<object> GetEvent(string eventDef, bool createIfNotExist)
        {
            if (string.IsNullOrEmpty(eventDef))
            {
                throw new ArgumentException($"Argument null or empty: {nameof(eventDef)}");
            }

            if (!this.mappings.TryGetValue(eventDef, out Event<object> evt) && createIfNotExist)
            {
                evt = new Event<object>();
                this.mappings.Add(eventDef, evt);
            }

            return evt;
        }
    }
}
