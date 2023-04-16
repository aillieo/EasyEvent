// -----------------------------------------------------------------------
// <copyright file="TestCasesEventCenter.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyEventTests
{
    using AillieoUtils;
    using NUnit.Framework;

    [Category("EventCenterTest")]
    public class TestCasesEventCenter
    {
        private int memberCount = 0;

        private void CountAdd(object o)
        {
            this.memberCount++;
        }

        [Test]
        public void TestAdd0()
        {
            var counter = 0;
            var eventDef = "event01";
            EventHandle handle = EventCenter.Default.AddListener(eventDef, () => counter++);
            EventCenter.Default.Invoke(eventDef);
            Assert.AreEqual(counter, 1);
            EventCenter.Default.Invoke(eventDef);
            Assert.AreEqual(counter, 2);
        }

        [Test]
        public void TestAdd1()
        {
            var counter = 0;
            var eventDef = "event02";
            EventHandle handle = EventCenter.Default.AddListener(eventDef, n => counter += (int)n);
            EventCenter.Default.Invoke(eventDef, 1);
            Assert.AreEqual(counter, 1);
            EventCenter.Default.Invoke(eventDef, 2);
            Assert.AreEqual(counter, 3);
        }

        [Test]
        public void TestAddAndRemove0()
        {
            var counter = 0;
            var eventDef = "event03";
            EventHandle handle = EventCenter.Default.AddListener(eventDef, () => counter++);
            EventCenter.Default.Invoke(eventDef);
            Assert.AreEqual(counter, 1);
            EventCenter.Default.Remove(eventDef, handle);
            EventCenter.Default.Invoke(eventDef);
            Assert.AreEqual(counter, 1);
        }

        [Test]
        public void TestAddAndRemove1()
        {
            var counter = 0;
            var eventDef = "event04";
            EventHandle handle = EventCenter.Default.AddListener(eventDef, n => counter++);
            EventCenter.Default.Invoke(eventDef, 1);
            Assert.AreEqual(counter, 1);
            EventCenter.Default.Remove(eventDef, handle);
            EventCenter.Default.Invoke(eventDef, 1);
            Assert.AreEqual(counter, 1);
        }

        [Test]
        public void TestAddAndRemove2()
        {
            var counter = 0;
            var eventDef = "event05";
            EventHandle handle = EventCenter.Default.AddListener(eventDef, () => counter++);
            EventCenter.Default.Invoke(eventDef);
            Assert.AreEqual(counter, 1);
            handle.Unlisten();
            EventCenter.Default.Invoke(eventDef);
            Assert.AreEqual(counter, 1);
        }

        [Test]
        public void TestAddAndRemove3()
        {
            var counter = 0;
            var eventDef = "event06";
            EventHandle handle = EventCenter.Default.AddListener(eventDef, n => counter++);
            EventCenter.Default.Invoke(eventDef, 1);
            Assert.AreEqual(counter, 1);
            handle.Unlisten();
            EventCenter.Default.Invoke(eventDef, 1);
            Assert.AreEqual(counter, 1);
        }

        [Test]
        public void TestAddAndRemove4()
        {
            this.memberCount = 0;
            var eventDef = "event07";
            EventHandle handle = EventCenter.Default.AddListener(eventDef, this.CountAdd);
            EventCenter.Default.Invoke(eventDef);
            Assert.AreEqual(this.memberCount, 1);
            EventCenter.Default.RemoveListener(eventDef, this.CountAdd);
            EventCenter.Default.Invoke(eventDef);
            Assert.AreEqual(this.memberCount, 1);
        }

        [Test]
        public void TestRemoveAll0()
        {
            var counter = 0;
            var eventDef1 = "event08";
            var eventDef2 = "event09";
            EventHandle handle1 = EventCenter.Default.AddListener(eventDef1, () => counter++);
            EventHandle handle2 = EventCenter.Default.AddListener(eventDef2, () => counter += 2);
            EventCenter.Default.Invoke(eventDef1);
            Assert.AreEqual(counter, 1);
            EventCenter.Default.Invoke(eventDef2);
            Assert.AreEqual(counter, 3);
            EventCenter.Default.Clear();
            EventCenter.Default.Invoke(eventDef1);
            Assert.AreEqual(counter, 3);
            EventCenter.Default.Invoke(eventDef2);
            Assert.AreEqual(counter, 3);
        }

        [Test]
        public void TestRemoveAll1()
        {
            var counter = 0;
            var eventDef1 = "event10";
            var eventDef2 = "event11";
            EventHandle handle1 = EventCenter.Default.AddListener(eventDef1, n => counter++);
            EventHandle handle2 = EventCenter.Default.AddListener(eventDef2, s => counter += 2);
            EventCenter.Default.Invoke(eventDef1, 1);
            Assert.AreEqual(counter, 1);
            EventCenter.Default.Invoke(eventDef2, "1");
            Assert.AreEqual(counter, 3);
            EventCenter.Default.Clear();
            EventCenter.Default.Invoke(eventDef1, 1);
            Assert.AreEqual(counter, 3);
            EventCenter.Default.Invoke(eventDef2, "1");
            Assert.AreEqual(counter, 3);
        }
    }
}
