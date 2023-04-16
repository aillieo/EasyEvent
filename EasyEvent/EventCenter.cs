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
        /// <summary>
        /// Default <see cref="EventCenter"/> instance.
        /// </summary>
        public static readonly EventCenter Default = new EventCenter();

        private readonly Dictionary<string, EasyDelegate<object>> mappings = new Dictionary<string, EasyDelegate<object>>(StringComparer.Ordinal);

        /// <summary>
        /// Add listener to the named event.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public EventHandle AddListener(string eventDef, Action callback)
        {
            return this.GetEvent(eventDef, true).AddListener(_ => callback());
        }

        /// <summary>
        /// Add listener to the named event.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public EventHandle AddListener(string eventDef, Action<object> callback)
        {
            return this.GetEvent(eventDef, true).AddListener(callback);
        }

        /// <summary>
        /// Add listener to this event and remove it after event invoked.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public EventHandle ListenOnce(string eventDef, Action callback)
        {
            return this.GetEvent(eventDef, true).ListenOnce(_ => callback());
        }

        /// <summary>
        /// Add listener to this event and remove it after event invoked.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public EventHandle ListenOnce(string eventDef, Action<object> callback)
        {
            return this.GetEvent(eventDef, true).ListenOnce(callback);
        }

        /// <summary>
        /// Add listener to this event and evaluate after event invoked, then remove the listener if result is true.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public EventHandle ListenUntil(string eventDef, Func<bool> callback)
        {
            return this.GetEvent(eventDef, true).ListenUntil(_ => callback());
        }

        /// <summary>
        /// Add listener to this event and evaluate after event invoked, then remove the listener if result is true.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public EventHandle ListenUntil(string eventDef, Func<object, bool> callback)
        {
            return this.GetEvent(eventDef, true).ListenUntil(callback);
        }

        /// <summary>
        /// Remove a listener by handle.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="handle">Handle for the listener.</param>
        /// <returns>Remove succeed.</returns>
        public bool Remove(string eventDef, EventHandle handle)
        {
            EasyDelegate<object> evt = this.GetEvent(eventDef, false);
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
        public int RemoveListener(string eventDef, Action<object> callback)
        {
            EasyDelegate<object> evt = this.GetEvent(eventDef, false);
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
        public void RemoveAllListeners(string eventDef)
        {
            this.GetEvent(eventDef, false)?.RemoveAllListeners();
        }

        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="arg">Argument for event invoking.</param>
        public void Invoke(string eventDef, object arg = null)
        {
            this.GetEvent(eventDef, false)?.Invoke(arg);
        }

        /// <summary>
        /// Invoke the event, exceptions (if any) will be aggregated and throw once.
        /// </summary>
        /// <param name="eventDef">Name of the event.</param>
        /// <param name="arg">Argument for event invoking.</param>
        public void InvokeAll(string eventDef, object arg = null)
        {
            this.GetEvent(eventDef, false)?.InvokeAll(arg);
        }

        /// <summary>
        /// Remove all listeners registered to all events managed by <see cref="EventCenter"/>.
        /// </summary>
        public void Clear()
        {
            foreach (var pair in this.mappings)
            {
                pair.Value.RemoveAllListeners();
            }

            this.mappings.Clear();
        }

        private EasyDelegate<object> GetEvent(string eventDef, bool createIfNotExist)
        {
            if (string.IsNullOrEmpty(eventDef))
            {
                throw new ArgumentException($"Argument null or empty: {nameof(eventDef)}");
            }

            if (!this.mappings.TryGetValue(eventDef, out EasyDelegate<object> evt) && createIfNotExist)
            {
                evt = new EasyDelegate<object>();
                this.mappings.Add(eventDef, evt);
            }

            return evt;
        }
    }
}
