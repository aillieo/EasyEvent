// -----------------------------------------------------------------------
// <copyright file="TestCasesEasyDelegate.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyEventTests
{
    using System;
    using AillieoUtils;
    using NUnit.Framework;

    [Category("DelegateTest")]
    public class TestCasesEasyDelegate
    {
        private int memberCount = 0;

        private void CountAdd()
        {
            this.memberCount++;
        }

        [Test]
        public void TestRemoveAll()
        {
            var count1 = 0;
            var count2 = 0;
            var count3 = 0;

            var evt = new EasyDelegate();
            evt.AddListener(() =>
            {
                count1++;
            });
            evt.AddListener(() =>
            {
                count2++;
            });
            evt.Invoke();

            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(count1, 1);
            Assert.AreEqual(count2, 1);
            Assert.AreEqual(count3, 0);

            evt.AddListener(() =>
            {
                count1++;
                evt.RemoveAllListeners();
                count3++;

                evt.AddListener(() =>
                {
                    count3++;
                });
            });

            Assert.AreEqual(evt.ListenerCount, 3);
            Assert.AreEqual(count1, 1);
            Assert.AreEqual(count2, 1);
            Assert.AreEqual(count3, 0);

            evt.Invoke();

            Assert.AreEqual(evt.ListenerCount, 1);

            Assert.AreEqual(count1, 3);
            Assert.AreEqual(count2, 2);
            Assert.AreEqual(count3, 2);

            evt.Invoke();
            Assert.AreEqual(evt.ListenerCount, 1);
            Assert.AreEqual(count1, 3);
            Assert.AreEqual(count2, 2);

            Assert.AreEqual(count3, 3);
        }

        [Test]
        public void TestRemoveOnlyAndAdd()
        {
            var evt = new EasyDelegate();
            var count = 0;

            var h1 = evt.AddListener(() =>
            {
                count++;
            });

            evt.Invoke();
            Assert.AreEqual(count, 1);
            Assert.AreEqual(evt.ListenerCount, 1);

            evt.Remove(h1);
            Assert.AreEqual(evt.ListenerCount, 0);
            evt.Invoke();
            Assert.AreEqual(evt.ListenerCount, 0);
            Assert.AreEqual(count, 1);
            evt.AddListener(() =>
            {
                count++;
            });
            Assert.AreEqual(evt.ListenerCount, 1);
            evt.Invoke();
            Assert.AreEqual(count, 2);
        }

        [Test]
        public void TestRemoveOnlyByFunc()
        {
            this.memberCount = 0;
            var evt = new EasyDelegate();

            evt.AddListener(this.CountAdd);

            evt.Invoke();
            Assert.AreEqual(this.memberCount, 1);
            Assert.AreEqual(evt.ListenerCount, 1);

            evt.RemoveListener(this.CountAdd);
            Assert.AreEqual(evt.ListenerCount, 0);
            evt.Invoke();
            Assert.AreEqual(evt.ListenerCount, 0);
            Assert.AreEqual(this.memberCount, 1);
        }

        [Test]
        public void TestRemoveOnly()
        {
            var evt = new EasyDelegate();
            var count = 0;

            var h1 = evt.AddListener(() =>
            {
                count++;
            });

            evt.Invoke();
            Assert.AreEqual(count, 1);
            Assert.AreEqual(evt.ListenerCount, 1);

            evt.Remove(h1);
            Assert.AreEqual(evt.ListenerCount, 0);
            evt.Invoke();
            Assert.AreEqual(evt.ListenerCount, 0);
            Assert.AreEqual(count, 1);
        }

        [Test]
        public void TestInvoke()
        {
            var evt = new EasyDelegate();
            var count = 0;

            var h1 = evt.AddListener(() =>
            {
                count++;
            });

            evt.Invoke();
            evt.Invoke();

            Assert.AreEqual(count, 2);
        }

        [Test]
        public void TestInvokeTwice()
        {
            var evt = new EasyDelegate();

            var count = 0;
            evt.AddListener(() =>
            {
                count++;
            });

            evt.Invoke();
            evt.Invoke();

            Assert.AreEqual(count, 2);
        }

        [Test]
        public void TestUnlisten()
        {
            var evt = new EasyDelegate();
            var count = 0;

            var h1 = evt.AddListener(() =>
            {
                count++;
            });

            evt.Invoke();

            Assert.AreEqual(count, 1);

            h1.Unlisten();

            evt.Invoke();

            Assert.AreEqual(count, 1);
        }

        [Test]
        public void TestAddRemove2()
        {
            var count = 0;
            this.memberCount = 0;

            var evt = new EasyDelegate();

            var evt2 = new EasyDelegate();

            EventHandle handle1 = default;

            EventHandle handle2 = default;

            handle1 = evt.AddListener(() =>
            {
                count++;
                evt.Remove(handle1);
            });

            handle2 = evt2.AddListener(() => throw new NotImplementedException());

            evt.Remove(handle2);

            evt.AddListener(() => { });

            evt.AddListener(this.CountAdd);
            evt.AddListener(this.CountAdd);

            evt.Invoke();

            Assert.AreEqual(count, 1);
            Assert.AreEqual(this.memberCount, 2);

            evt.Invoke();

            Assert.AreEqual(count, 1);
            Assert.AreEqual(this.memberCount, 4);

            evt.Invoke();

            Assert.AreEqual(count, 1);
            Assert.AreEqual(this.memberCount, 6);

            var rmvCount = evt.RemoveListener(this.CountAdd);
            Assert.AreEqual(rmvCount, 2);

            Assert.AreEqual(count, 1);
            Assert.AreEqual(this.memberCount, 6);

            evt.Invoke();
            evt.Invoke();

            Assert.AreEqual(count, 1);
            Assert.AreEqual(this.memberCount, 6);
        }

        [Test]
        public void TestEmptyInvoke()
        {
            var evt = new EasyDelegate();
            evt.Invoke();
        }

        [Test]
        public void TestInvoke2()
        {
            var counter1 = 0;
            var counter2 = 0;

            var evt = new EasyDelegate();
            evt.AddListener(() =>
            {
                counter1++;
            });
            evt.Invoke();

            evt.AddListener(() =>
            {
                counter2++;
            });
            evt.Invoke();

            Assert.AreEqual(2, counter1);
            Assert.AreEqual(1, counter2);
        }

        [Test]
        public void TestAddRemove()
        {
            var counter1 = 0;
            var counter2 = 0;

            var evt = new EasyDelegate();
            var handle1 = evt.AddListener(() =>
            {
                counter1++;
            });
            evt.Invoke();

            var handle2 = evt.AddListener(() =>
            {
                counter2++;
            });
            evt.Remove(handle1);
            evt.Invoke();

            evt.Remove(handle2);
            evt.Invoke();

            Assert.AreEqual(1, counter1);
            Assert.AreEqual(1, counter2);
        }

        [Test]
        public void TestNestAdd()
        {
            var counter1 = 0;
            var counter2 = 0;

            var evt = new EasyDelegate();
            evt.AddListener(() =>
            {
                counter1++;
                evt.AddListener(() =>
                {
                    counter2++;
                });
            });

            evt.Invoke();
            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(1, counter1);
            Assert.AreEqual(1, counter2);
        }

        [Test]
        public void TestListenOnce()
        {
            var counter = 0;

            var evt = new EasyDelegate();

            EventHandle handle = null;
            handle = evt.AddListener(() =>
            {
                counter++;
                evt.Remove(handle);
            });

            evt.Invoke();
            evt.Invoke();

            Assert.AreEqual(1, counter);

            evt.ListenOnce(() =>
            {
                counter++;
            });

            evt.Invoke();
            evt.Invoke();

            Assert.AreEqual(2, counter);
        }

        [Test]
        public void TestRemoveHeadAndAdd()
        {
            var count1 = 0;
            var count2 = 0;
            var count3 = 0;
            var evt = new EasyDelegate();
            EventHandle h1 = default;
            h1 = evt.AddListener(() =>
            {
                count1++;
            });
            evt.Invoke();
            var h2 = evt.AddListener(() =>
            {
                count2++;
                evt.Remove(h1);
            });

            Assert.AreEqual(evt.ListenerCount, 2);

            evt.AddListener(() =>
            {
                count3++;
            });

            Assert.AreEqual(evt.ListenerCount, 3);

            evt.Invoke();

            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(count1, 2);
            Assert.AreEqual(count2, 1);
            Assert.AreEqual(count3, 1);

            evt.Invoke();
            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(count1, 2);
            Assert.AreEqual(count2, 2);
            Assert.AreEqual(count3, 2);
        }

        [Test]
        public void TestRemoveHeadByFunc()
        {
            this.memberCount = 0;
            var count2 = 0;
            var evt = new EasyDelegate();
            EventHandle h1 = default;
            h1 = evt.AddListener(this.CountAdd);
            evt.Invoke();
            var h2 = evt.AddListener(() =>
            {
                count2++;
                evt.RemoveListener(this.CountAdd);
            });

            Assert.AreEqual(evt.ListenerCount, 2);

            evt.Invoke();

            Assert.AreEqual(evt.ListenerCount, 1);
            Assert.AreEqual(this.memberCount, 2);
            Assert.AreEqual(count2, 1);

            evt.Invoke();
            Assert.AreEqual(evt.ListenerCount, 1);
            Assert.AreEqual(this.memberCount, 2);
            Assert.AreEqual(count2, 2);
        }

        [Test]
        public void TestRemoveHead()
        {
            var count1 = 0;
            var count2 = 0;
            var evt = new EasyDelegate();
            EventHandle h1 = default;
            h1 = evt.AddListener(() =>
            {
                count1++;
            });
            evt.Invoke();
            var h2 = evt.AddListener(() =>
            {
                count2++;
                evt.Remove(h1);
            });

            Assert.AreEqual(evt.ListenerCount, 2);

            evt.Invoke();

            Assert.AreEqual(evt.ListenerCount, 1);
            Assert.AreEqual(count1, 2);
            Assert.AreEqual(count2, 1);

            evt.Invoke();
            Assert.AreEqual(evt.ListenerCount, 1);
            Assert.AreEqual(count1, 2);
            Assert.AreEqual(count2, 2);
        }

        [Test]
        public void TestRemoveMiddleAndAdd()
        {
            var count1 = 0;
            var count2 = 0;
            var count3 = 0;
            var evt = new EasyDelegate();
            var count4 = 0;
            evt.AddListener(() =>
            {
                count4++;
            });

            EventHandle h1 = default;
            h1 = evt.AddListener(() =>
            {
                count1++;
            });
            evt.Invoke();
            var h2 = evt.AddListener(() =>
            {
                count2++;
                evt.Remove(h1);
            });

            Assert.AreEqual(evt.ListenerCount, 3);
            Assert.AreEqual(count4, 1);

            evt.AddListener(() =>
            {
                count3++;
            });

            Assert.AreEqual(evt.ListenerCount, 4);

            evt.Invoke();

            Assert.AreEqual(count4, 2);
            Assert.AreEqual(evt.ListenerCount, 3);
            Assert.AreEqual(count1, 2);
            Assert.AreEqual(count2, 1);
            Assert.AreEqual(count3, 1);

            evt.Invoke();
            Assert.AreEqual(count4, 3);
            Assert.AreEqual(evt.ListenerCount, 3);
            Assert.AreEqual(count1, 2);
            Assert.AreEqual(count2, 2);
            Assert.AreEqual(count3, 2);
        }

        [Test]
        public void TestRemoveMiddleByFunc()
        {
            this.memberCount = 0;
            var count2 = 0;
            var evt = new EasyDelegate();
            var count4 = 0;
            evt.AddListener(() =>
            {
                count4++;
            });

            EventHandle h1 = default;
            h1 = evt.AddListener(this.CountAdd);
            evt.Invoke();

            Assert.AreEqual(count4, 1);

            var h2 = evt.AddListener(() =>
            {
                count2++;
                evt.RemoveListener(this.CountAdd);
            });

            Assert.AreEqual(evt.ListenerCount, 3);

            evt.Invoke();
            Assert.AreEqual(count4, 2);
            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(this.memberCount, 2);
            Assert.AreEqual(count2, 1);

            evt.Invoke();
            Assert.AreEqual(count4, 3);
            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(this.memberCount, 2);
            Assert.AreEqual(count2, 2);
        }

        [Test]
        public void TestRemoveMiddle()
        {
            var count1 = 0;
            var count2 = 0;
            var evt = new EasyDelegate();
            var count4 = 0;
            evt.AddListener(() =>
            {
                count4++;
            });

            EventHandle h1 = default;
            h1 = evt.AddListener(() =>
            {
                count1++;
            });
            evt.Invoke();

            Assert.AreEqual(count4, 1);
            var h2 = evt.AddListener(() =>
            {
                count2++;
                evt.Remove(h1);
            });

            Assert.AreEqual(evt.ListenerCount, 3);

            evt.Invoke();

            Assert.AreEqual(count4, 2);
            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(count1, 2);
            Assert.AreEqual(count2, 1);

            evt.Invoke();
            Assert.AreEqual(count4, 3);
            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(count1, 2);
            Assert.AreEqual(count2, 2);
        }

        [Test]
        public void TestRemoveTailAndAdd()
        {
            var count1 = 0;
            var count2 = 0;
            var count3 = 0;
            var evt = new EasyDelegate();
            EventHandle h3 = default;
            evt.AddListener(() =>
            {
                count1++;
            });
            evt.Invoke();
            var h2 = evt.AddListener(() =>
            {
                count2++;
                evt.Remove(h3);
            });

            Assert.AreEqual(evt.ListenerCount, 2);

            h3 = evt.AddListener(() =>
            {
                count3++;
            });

            Assert.AreEqual(evt.ListenerCount, 3);

            evt.Invoke();

            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(count1, 2);
            Assert.AreEqual(count2, 1);
            Assert.AreEqual(count3, 0);

            evt.Invoke();
            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(count1, 3);
            Assert.AreEqual(count2, 2);
            Assert.AreEqual(count3, 0);
        }

        [Test]
        public void TestRemoveTailByFunc()
        {
            this.memberCount = 0;
            var count2 = 0;
            var evt = new EasyDelegate();
            evt.Invoke();

            var h2 = evt.AddListener(() =>
            {
                count2++;
                evt.RemoveListener(this.CountAdd);
            });
            evt.AddListener(this.CountAdd);

            Assert.AreEqual(evt.ListenerCount, 2);

            evt.Invoke();

            Assert.AreEqual(evt.ListenerCount, 1);
            Assert.AreEqual(this.memberCount, 0);
            Assert.AreEqual(count2, 1);

            evt.Invoke();
            Assert.AreEqual(evt.ListenerCount, 1);
            Assert.AreEqual(this.memberCount, 0);
            Assert.AreEqual(count2, 2);
        }

        [Test]
        public void TestRemoveTail()
        {
            var count1 = 0;
            var count2 = 0;
            var count3 = 0;
            var evt = new EasyDelegate();
            EventHandle h3 = default;
            evt.AddListener(() =>
            {
                count1++;
            });
            evt.Invoke();
            var h2 = evt.AddListener(() =>
            {
                count2++;
                evt.Remove(h3);
            });

            h3 = evt.AddListener(() =>
            {
                count3++;
            });

            Assert.AreEqual(evt.ListenerCount, 3);

            evt.Invoke();

            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(count1, 2);
            Assert.AreEqual(count2, 1);
            Assert.AreEqual(count3, 0);

            evt.Invoke();
            Assert.AreEqual(evt.ListenerCount, 2);
            Assert.AreEqual(count1, 3);
            Assert.AreEqual(count2, 2);
            Assert.AreEqual(count3, 0);
        }

        [Test]
        public void TestWithExceptions()
        {
            var count0 = 0;

            var evt = new EasyDelegate();
            evt.AddListener(() => throw new InvalidOperationException());
            evt.AddListener(() => count0++);
            evt.AddListener(() => throw new InvalidOperationException());

            Assert.Throws<InvalidOperationException>(() => evt.Invoke());
            Assert.AreEqual(count0, 0);

            Assert.Throws<AggregateException>(() => evt.InvokeAll());
            Assert.AreEqual(count0, 1);
        }

        [Test]
        public void TestListenUntil()
        {
            var counter = 0;

            var evt = new EasyDelegate();

            EventHandle handle = evt.ListenUntil(() =>
            {
                counter++;
                return counter >= 2;
            });

            evt.Invoke();
            evt.Invoke();
            evt.Invoke();

            Assert.AreEqual(2, counter);
        }

        [Test]
        public void TestListenUntil1()
        {
            var counter = 0;
            var number = 0;

            var evt1 = new EasyDelegate<int>();

            evt1.ListenUntil(e =>
            {
                counter++;
                number = e;
                return e == 2;
            });

            evt1.Invoke(1);
            evt1.Invoke(2);
            evt1.Invoke(3);

            Assert.AreEqual(2, number);
            Assert.AreEqual(2, counter);
        }
    }
}
