// -----------------------------------------------------------------------
// <copyright file="Performance.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyEventTests
{
    using System;
    using AillieoUtils;
    using NUnit.Framework;
    using UnityEngine.Events;
    using UnityEngine.Profiling;

    [Category("PerformanceTest")]
    public class Performance
    {
        private readonly EasyDelegate event1 = new EasyDelegate();
        private readonly UnityEvent event2 = new UnityEvent();

        private event Action event3;

        private readonly int loopCount = 100;

        private int counter = 0;
        private int result;

        private Action callback;
        private UnityAction unityCallback;

        [Test]
        [Order(100)]
        public static void TestBatch()
        {
            for (var i = 0; i < 100; ++i)
            {
                var performance = new Performance();

                Profiler.BeginSample("Perf-RegisterEasyEvent");
                performance.RegisterEasyEvent();
                Profiler.EndSample();

                Profiler.BeginSample("Perf-RunEasyEvent");
                performance.RunEasyEvent();
                Profiler.EndSample();

                Profiler.BeginSample("Perf-UnregisterEasyEvent");
                performance.UnregisterEasyEvent();
                Profiler.EndSample();

                Profiler.BeginSample("Perf-RegisterUnityEvent");
                performance.RegisterUnityEvent();
                Profiler.EndSample();

                Profiler.BeginSample("Perf-RunUnityEvent");
                performance.RunUnityEvent();
                Profiler.EndSample();

                Profiler.BeginSample("Perf-UnregisterUnityEvent");
                performance.UnregisterUnityEvent();
                Profiler.EndSample();

                Profiler.BeginSample("Perf-RegisterCSEvent");
                performance.RegisterCSEvent();
                Profiler.EndSample();

                Profiler.BeginSample("Perf-RunCSEvent");
                performance.RunCSEvent();
                Profiler.EndSample();

                Profiler.BeginSample("Perf-UnregisterCSEvent");
                performance.UnregisterCSEvent();
                Profiler.EndSample();
            }
        }

        public Performance()
        {
            this.callback = () => this.counter++;
            this.unityCallback = () => this.counter++;
        }

        [Test]
        [Order(1)]
        public void RegisterEasyEvent()
        {
            for (var i = 0; i < this.loopCount; i++)
            {
                this.event1.AddListener(this.callback);
            }
        }

        [Test]
        [Order(2)]
        public void RunEasyEvent()
        {
            this.counter = 0;
            for (var i = 0; i < this.loopCount; i++)
            {
                this.event1.Invoke();
            }

            this.result = this.counter;
        }

        [Test]
        [Order(3)]
        public void UnregisterEasyEvent()
        {
            this.event1.RemoveAllListeners();
        }

        [Test]
        [Order(1)]
        public void RegisterUnityEvent()
        {
            for (var i = 0; i < this.loopCount; i++)
            {
                this.event2.AddListener(this.unityCallback);
            }
        }

        [Test]
        [Order(2)]
        public void RunUnityEvent()
        {
            this.counter = 0;
            for (var i = 0; i < this.loopCount; i++)
            {
                this.event2.Invoke();
            }

            this.result = this.counter;
        }

        [Test]
        [Order(3)]
        public void UnregisterUnityEvent()
        {
            this.event2.RemoveAllListeners();
        }

        [Test]
        [Order(1)]
        public void RegisterCSEvent()
        {
            for (var i = 0; i < this.loopCount; i++)
            {
                this.event3 += this.callback;
            }
        }

        [Test]
        [Order(2)]
        public void RunCSEvent()
        {
            this.counter = 0;
            for (var i = 0; i < this.loopCount; i++)
            {
                this.event3.Invoke();
            }

            this.result = this.counter;
        }

        [Test]
        [Order(3)]
        public void UnregisterCSEvent()
        {
            var list = this.event3.GetInvocationList();
            foreach (var e in list)
            {
                this.event3 -= (Action)e;
            }
        }
    }
}
