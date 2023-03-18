// -----------------------------------------------------------------------
// <copyright file="IListenable`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;

    public interface IListenable<T>
    {
        Handle<T> AddListener(Action<T> callback);

        bool Remove(Handle<T> handle);

        int RemoveListener(Action<T> callback);

        void RemoveAllListeners();
    }
}
