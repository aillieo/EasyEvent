// -----------------------------------------------------------------------
// <copyright file="IListenable.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;

    public interface IListenable
    {
        Handle AddListener(Action callback);

        bool Remove(Handle handle);

        int RemoveListener(Action callback);

        void RemoveAllListeners();
    }
}
