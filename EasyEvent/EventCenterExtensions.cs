// -----------------------------------------------------------------------
// <copyright file="EventCenterExtensions.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;

    /// <summary>
    /// Extension methods for <see cref="EventCenter"/>.
    /// </summary>
    public static class EventCenterExtensions
    {
        /// <summary>
        /// Add listener to the named event.
        /// </summary>
        /// <param name="eventCenter">Instance of the <see cref="EventCenter"/>.</param>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> AddListener(this EventCenter eventCenter, string eventDef, Action callback)
        {
            return eventCenter.GetEvent(eventDef, true).AddListener(_ => callback());
        }

        /// <summary>
        /// Add listener to the named event.
        /// </summary>
        /// <param name="eventCenter">Instance of the <see cref="EventCenter"/>.</param>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> AddListener(this EventCenter eventCenter, string eventDef, Action<object> callback)
        {
            return eventCenter.GetEvent(eventDef, true).AddListener(callback);
        }

        /// <summary>
        /// Add listener to this event and remove it after event invoked.
        /// </summary>
        /// <param name="eventCenter">Instance of the <see cref="EventCenter"/>.</param>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> ListenOnce(this EventCenter eventCenter, string eventDef, Action callback)
        {
            return eventCenter.GetEvent(eventDef, true).ListenOnce(_ => callback());
        }

        /// <summary>
        /// Add listener to this event and remove it after event invoked.
        /// </summary>
        /// <param name="eventCenter">Instance of the <see cref="EventCenter"/>.</param>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static Handle<object> ListenOnce(this EventCenter eventCenter, string eventDef, Action<object> callback)
        {
            return eventCenter.GetEvent(eventDef, true).ListenOnce(callback);
        }

        /// <summary>
        /// Remove a listener by handle.
        /// </summary>
        /// <param name="eventCenter">Instance of the <see cref="EventCenter"/>.</param>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="handle">Handle for the listener.</param>
        /// <returns>Remove succeed.</returns>
        public static bool Remove(this EventCenter eventCenter, string eventDef, Handle<object> handle)
        {
            Event<object> evt = eventCenter.GetEvent(eventDef, false);
            if (evt != null)
            {
                return evt.Remove(handle);
            }

            return false;
        }

        /// <summary>
        /// Remove a listener.
        /// </summary>
        /// <param name="eventCenter">Instance of the <see cref="EventCenter"/>.</param>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">The listener.</param>
        /// <returns>Count of removed listener instances.</returns>
        public static int RemoveListener(this EventCenter eventCenter, string eventDef, Action<object> callback)
        {
            Event<object> evt = eventCenter.GetEvent(eventDef, false);
            if (evt != null)
            {
                return evt.RemoveListener(callback);
            }

            return 0;
        }

        /// <summary>
        /// Remove all listeners registered.
        /// </summary>
        /// <param name="eventCenter">Instance of the <see cref="EventCenter"/>.</param>
        /// <param name="eventDef">Name of the event.</param>
        public static void RemoveAllListeners(this EventCenter eventCenter, string eventDef)
        {
            eventCenter.GetEvent(eventDef, false)?.RemoveAllListeners();
        }

        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="eventCenter">Instance of the <see cref="EventCenter"/>.</param>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="arg">Argument for event invoking.</param>
        public static void Invoke(this EventCenter eventCenter, string eventDef, object arg = null)
        {
            eventCenter.GetEvent(eventDef, false)?.Invoke(arg);
        }

        /// <summary>
        /// Invoke the event, exceptions (if any) will be aggregated and throw once.
        /// </summary>
        /// <param name="eventCenter">Instance of the <see cref="EventCenter"/>.</param>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="arg">Argument for event invoking.</param>
        public static void InvokeAll(this EventCenter eventCenter, string eventDef, object arg = null)
        {
            eventCenter.GetEvent(eventDef, false)?.InvokeAll(arg);
        }

        /// <summary>
        /// Remove all listeners registered to all events managed by <see cref="EventCenter"/>.
        /// </summary>
        /// <param name="eventCenter">Instance of the <see cref="EventCenter"/>.</param>
        public static void Clear(this EventCenter eventCenter)
        {
            foreach (var pair in eventCenter.mappings)
            {
                pair.Value.RemoveAllListeners();
            }

            eventCenter.mappings.Clear();
        }
    }
}
