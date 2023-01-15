using System;
using AillieoUtils;
using NUnit.Framework;

[Category("EventTest")]
public class TestCases
{
    private int memberCount = 0;

    private void CountAdd()
    {
        this.memberCount++;
    }

    [Test]
    public void TestRemoveAll()
    {
        int count1 = 0;
        int count2 = 0;
        int count3 = 0;

        Event evt = new Event();
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
        Event evt = new Event();
        int count = 0;

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
        Event evt = new Event();

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
        Event evt = new Event();
        int count = 0;

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
        Event evt = new Event();
        int count = 0;

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
        Event evt = new Event();

        int count = 0;
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
        Event evt = new Event();
        int count = 0;

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
        int count = 0;
        this.memberCount = 0;

        Event evt = new Event();

        Handle handle1 = default;

        Handle handle2 = default;

        handle1 = evt.AddListener(() =>
        {
            count++;
            evt.Remove(handle1);
        });

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

        int rmvCount = evt.RemoveListener(this.CountAdd);
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
        Event evt = new Event();
        evt.Invoke();
    }

    [Test]
    public void TestInvoke2()
    {
        int counter1 = 0;
        int counter2 = 0;

        Event evt = new Event();
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
        int counter1 = 0;
        int counter2 = 0;

        Event evt = new Event();
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
        int counter1 = 0;
        int counter2 = 0;

        Event evt = new Event();
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
        int counter = 0;

        Event evt = new Event();

        Handle handle = null;
        handle = evt.AddListener(() =>
        {
            counter++;
            evt.Remove(handle);
        });

        evt.Invoke();
        evt.Invoke();

        Assert.AreEqual(1, counter);
    }

    [Test]
    public void TestRemoveHeadAndAdd()
    {
        int count1 = 0;
        int count2 = 0;
        int count3 = 0;
        Event evt = new Event();
        Handle h1 = default;
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
        int count2 = 0;
        Event evt = new Event();
        Handle h1 = default;
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
        int count1 = 0;
        int count2 = 0;
        Event evt = new Event();
        Handle h1 = default;
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
        int count1 = 0;
        int count2 = 0;
        int count3 = 0;
        Event evt = new Event();
        int count4 = 0;
        evt.AddListener(() =>
        {
            count4++;
        });

        Handle h1 = default;
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
        int count2 = 0;
        Event evt = new Event();
        int count4 = 0;
        evt.AddListener(() =>
        {
            count4++;
        });

        Handle h1 = default;
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
        int count1 = 0;
        int count2 = 0;
        Event evt = new Event();
        int count4 = 0;
        evt.AddListener(() =>
        {
            count4++;
        });

        Handle h1 = default;
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
        int count1 = 0;
        int count2 = 0;
        int count3 = 0;
        Event evt = new Event();
        Handle h3 = default;
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
        int count2 = 0;
        Event evt = new Event();
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
        int count1 = 0;
        int count2 = 0;
        int count3 = 0;
        Event evt = new Event();
        Handle h3 = default;
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
        int count0 = 0;

        Event evt = new Event();
        evt.AddListener(() => throw new InvalidOperationException());
        evt.AddListener(() => count0++);
        evt.AddListener(() => throw new InvalidOperationException());

        Assert.Throws<InvalidOperationException>(() => evt.Invoke());
        Assert.AreEqual(count0, 0);

        Assert.Throws<AggregateException>(() => evt.InvokeAll());
        Assert.AreEqual(count0, 1);
    }
}
