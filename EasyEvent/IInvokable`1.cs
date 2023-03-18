// -----------------------------------------------------------------------
// <copyright file="IInvokable`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    public interface IInvokable<T>
    {
        int ListenerCount { get; }

        void Invoke(T arg);

        void InvokeAll(T arg);
    }
}
