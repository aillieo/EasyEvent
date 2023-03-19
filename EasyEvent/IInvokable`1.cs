// -----------------------------------------------------------------------
// <copyright file="IInvokable`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    public interface IInvokable<in T>
    {
        void Invoke(T arg);

        void InvokeAll(T arg);
    }
}
