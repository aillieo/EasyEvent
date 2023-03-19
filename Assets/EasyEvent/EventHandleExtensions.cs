// -----------------------------------------------------------------------
// <copyright file="EventHandleExtensions.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    /// <summary>
    /// Extension methods for <see cref="EventHandle"/>.
    /// </summary>
    public static class EventHandleExtensions
    {
        /// <summary>
        /// Invoke <see cref="EventHandle.Unlisten"/> with null check.
        /// </summary>
        /// <param name="eventHandle">The <see cref="EventHandle"/> instance.</param>
        /// <returns>Remove succeed.</returns>
        public static bool SafeUnlisten(this EventHandle eventHandle)
        {
            if (eventHandle != null)
            {
                return eventHandle.Unlisten();
            }

            return false;
        }
    }
}
