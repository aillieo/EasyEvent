// -----------------------------------------------------------------------
// <copyright file="EasyEvent.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;

    /// <summary>
    /// Event with no argument.
    /// </summary>
    public readonly struct EasyEvent : IListenable, IEquatable<EasyEvent>
    {
        private readonly EasyDelegate associatedDelegate;

        internal EasyEvent(EasyDelegate del)
        {
            this.associatedDelegate = del;
        }

        /// <summary>
        /// Gets a value indicating whether that the event is valid.
        /// </summary>
        public bool Valid => this.associatedDelegate != null;

        public EventHandle AddListener(Action callback)
        {
            this.EnsureValid();
            return this.associatedDelegate.AddListener(callback);
        }

        public bool Remove(EventHandle eventHandle)
        {
            this.EnsureValid();
            return this.associatedDelegate.Remove(eventHandle);
        }

        public int RemoveListener(Action callback)
        {
            this.EnsureValid();
            return this.associatedDelegate.RemoveListener(callback);
        }

        public bool Equals(EasyEvent other)
        {
            this.EnsureValid();
            return Equals(this.associatedDelegate, other.associatedDelegate);
        }

        public override bool Equals(object obj)
        {
            return obj is EasyEvent other && this.Equals(other);
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
