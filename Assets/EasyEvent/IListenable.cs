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
        EventHandle AddListener(Action callback);

        bool Remove(EventHandle eventHandle);

        int RemoveListener(Action callback);
    }
}
