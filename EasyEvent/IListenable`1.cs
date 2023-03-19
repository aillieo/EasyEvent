// -----------------------------------------------------------------------
// <copyright file="IListenable`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;

    public interface IListenable<out T>
    {
        EventHandle AddListener(Action<T> callback);

        bool Remove(EventHandle handle);

        int RemoveListener(Action<T> callback);
    }
}
