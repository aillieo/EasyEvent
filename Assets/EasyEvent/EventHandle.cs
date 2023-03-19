// -----------------------------------------------------------------------
// <copyright file="EventHandle.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    /// <summary>
    /// Interface for event handles.
    /// </summary>
    public abstract class EventHandle
    {
        /// <summary>
        /// Remove the associated listener from event.
        /// </summary>
        /// <returns>Remove succeed.</returns>
        public abstract bool Unlisten();
    }
}
