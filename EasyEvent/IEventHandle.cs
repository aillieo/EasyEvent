// -----------------------------------------------------------------------
// <copyright file="IEventHandle.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    /// <summary>
    /// Interface for event handles.
    /// </summary>
    public interface IEventHandle
    {
        /// <summary>
        /// Remove the associated listener from event.
        /// </summary>
        /// <returns>Remove succeed.</returns>
        bool Unlisten();
    }
}
