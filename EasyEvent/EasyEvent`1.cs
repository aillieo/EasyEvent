// -----------------------------------------------------------------------
// <copyright file="EasyEvent`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;

    /// <summary>
    /// Event with one generic argument.
    /// </summary>
    /// <typeparam name="T">Type of argument.</typeparam>
    public readonly struct EasyEvent<T> : IListenable<T>, IEquatable<EasyEvent<T>>
    {
        private readonly EasyDelegate<T> associatedDelegate;

        internal EasyEvent(EasyDelegate<T> del)
        {
            this.associatedDelegate = del;
        }

        /// <summary>
        /// Gets a value indicating whether that the event is valid.
        /// </summary>
        public bool Valid => this.associatedDelegate != null;

        public EventHandle AddListener(Action<T> callback)
        {
            this.EnsureValid();
            return this.associatedDelegate.AddListener(callback);
        }

        public bool Remove(EventHandle handle)
        {
            this.EnsureValid();
            return this.associatedDelegate.Remove(handle);
        }

        public int RemoveListener(Action<T> callback)
        {
            this.EnsureValid();
            return this.associatedDelegate.RemoveListener(callback);
        }

        public bool Equals(EasyEvent<T> other)
        {
            return Equals(this.associatedDelegate, other.associatedDelegate);
        }

        public override bool Equals(object obj)
        {
            return obj is EasyEvent<T> other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.associatedDelegate != null ? this.associatedDelegate.GetHashCode() : 0;
        }

        private void EnsureValid()
        {
            if (!this.Valid)
            {
                throw new InvalidOperationException("Invalid EasyEvent instance");
            }
        }
    }
}
