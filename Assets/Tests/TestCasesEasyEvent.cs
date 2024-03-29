// -----------------------------------------------------------------------
// <copyright file="TestCasesEasyEvent.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyEventTests
{
    using System;
    using AillieoUtils;
    using NUnit.Framework;

    [Category("EventTest")]
    public class TestCasesEasyEvent
    {
        [Test]
        public void Test1()
        {
            var del = new EasyDelegate();
            EasyEvent evt1 = del;
            EasyEvent evt2 = del;
            Assert.AreEqual(evt1, evt2);
            Assert.IsTrue(evt1.Valid);
            EasyEvent evt3 = evt1;
            Assert.AreEqual(evt1, evt3);
        }

        [Test]
        public void Test2()
        {
            EasyEvent evt = default;
            Assert.IsFalse(evt.Valid);
        }

        [Test]
        public void Test3()
        {
            EasyEvent evt = default;
            Assert.Throws<InvalidOperationException>(() => evt.AddListener(() => UnityEngine.Debug.Log("Never happen")));
        }
    }
}
