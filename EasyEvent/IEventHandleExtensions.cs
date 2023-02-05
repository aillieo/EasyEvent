// -----------------------------------------------------------------------
// <copyright file="IEventHandleExtensions.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    /// <summary>
    /// Extension methods for <see cref="IEventHandle"/>.
    /// </summary>
    public static class IEventHandleExtensions
    {
        /// <summary>
        /// Invoke <see cref="IEventHandle.Unlisten"/> with null check.
        /// </summary>
        /// <param name="eventHandle">The <see cref="IEventHandle"/> instance.</param>
        /// <returns>Remove succeed.</returns>
        public static bool SafeUnlisten(this IEventHandle eventHandle)
        {
            if (eventHandle != null)
            {
                return eventHandle.Unlisten();
            }

            return false;
        }
    }
}
