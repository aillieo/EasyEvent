// -----------------------------------------------------------------------
// <copyright file="IListenableExtensions.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils
{
    using System;

    /// <summary>
    /// Extension methods for <see cref="IListenable"/> and <see cref="IListenable{T}"/>.
    /// </summary>
    public static class IListenableExtensions
    {
        /// <summary>
        /// Add listener to this event and remove it after event invoked.
        /// </summary>
        /// <param name="listenable">The <see cref="IListenable"/> instance.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static EventHandle ListenOnce(this IListenable listenable, Action callback)
        {
            if (listenable == null)
            {
                throw new ArgumentNullException(nameof(listenable));
            }

            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            EventHandle handle = default;
            handle = listenable.AddListener(() =>
            {
                listenable.Remove(handle);
                callback.Invoke();
            });
            return handle;
        }

        /// <summary>
        /// Add listener to this event and evaluate after event invoked, then remove the listener if result is true.
        /// </summary>
        /// <param name="listenable">The <see cref="IListenable"/> instance.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static EventHandle ListenUntil(this IListenable listenable, Func<bool> callback)
        {
            if (listenable == null)
            {
                throw new ArgumentNullException(nameof(listenable));
            }

            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            EventHandle handle = default;
            handle = listenable.AddListener(() =>
            {
                if (callback.Invoke())
                {
                    listenable.Remove(handle);
                }
            });
            return handle;
        }

        /// <summary>
        /// Add listener to this event and remove it after event invoked.
        /// </summary>
        /// <typeparam name="T">Event arg for <see cref="IListenable{T}"/>.</typeparam>
        /// <param name="listenable">The <see cref="IListenable{T}"/> instance.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static EventHandle ListenOnce<T>(this IListenable<T> listenable, Action<T> callback)
        {
            if (listenable == null)
            {
                throw new ArgumentNullException(nameof(listenable));
            }

            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            EventHandle handle = default;
            handle = listenable.AddListener(arg =>
            {
                listenable.Remove(handle);
                callback.Invoke(arg);
            });
            return handle;
        }

        /// <summary>
        /// Add listener to this event and evaluate after event invoked, then remove the listener if result is true.
        /// </summary>
        /// <typeparam name="T">Event arg for <see cref="IListenable{T}"/>.</typeparam>
        /// <param name="listenable">The <see cref="IListenable"/> instance.</param>
        /// <param name="callback">Callback for this event.</param>
        /// <returns>Handle for this listener.</returns>
        public static EventHandle ListenUntil<T>(this IListenable<T> listenable, Func<T, bool> callback)
        {
            if (listenable == null)
            {
                throw new ArgumentNullException(nameof(listenable));
            }

            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            EventHandle handle = default;
            handle = listenable.AddListener(arg =>
            {
                if (callback.Invoke(arg))
                {
                    listenable.Remove(handle);
                }
            });
            return handle;
        }
    }
}
