// -----------------------------------------------------------------------
// <copyright file="IListenable.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;

    /// <summary>
    /// A listenable object, e.g. <see cref="EasyDelegate"/> and <see cref="EasyEvent"/>.
    /// </summary>
    public interface IListenable
    {
        /// <summary>
        /// Add listener to this event.
        /// </summary>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        EventHandle AddListener(Action callback);

        /// <summary>
        /// Remove a listener by eventHandle.
        /// </summary>
        /// <param name="eventHandle">Handle for the listener.</param>
        /// <returns>Remove succeed.</returns>
        bool Remove(EventHandle eventHandle);

        /// <summary>
        /// Remove a listener.
        /// </summary>
        /// <param name="callback">The listener.</param>
        /// <returns>Count of removed listener instances.</returns>
        int RemoveListener(Action callback);
    }
}
