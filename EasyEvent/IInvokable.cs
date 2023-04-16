// -----------------------------------------------------------------------
// <copyright file="IInvokable.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    /// <summary>
    /// An invokable object, e.g. <see cref="EasyDelegate"/>.
    /// </summary>
    public interface IInvokable
    {
        /// <summary>
        /// Invoke the event.
        /// </summary>
        void Invoke();

        /// <summary>
        /// Invoke the event, exceptions (if any) will be aggregated and throw once.
        /// </summary>
        void InvokeAll();
    }
}
