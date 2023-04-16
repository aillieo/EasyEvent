// -----------------------------------------------------------------------
// <copyright file="IInvokable`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    /// <summary>
    /// An invokable object, e.g. <see cref="EasyDelegate{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of argument.</typeparam>
    public interface IInvokable<in T>
    {
        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="arg">Argument for event invoking.</param>
        void Invoke(T arg);

        /// <summary>
        /// Invoke the event, exceptions (if any) will be aggregated and throw once.
        /// </summary>
        /// <param name="arg">Argument for event invoking.</param>
        void InvokeAll(T arg);
    }
}
