// -----------------------------------------------------------------------
// <copyright file="IInvokable.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    public interface IInvokable
    {
        int ListenerCount { get; }

        void Invoke();

        void InvokeAll();
    }
}
