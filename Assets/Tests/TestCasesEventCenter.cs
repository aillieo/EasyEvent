using System;
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
        int counter = 0;
        string eventDef = "event01";
        Handle<object> handle = EventCenter.Default.AddListener(eventDef, () => counter++);
        EventCenter.Default.Invoke(eventDef);
        Assert.AreEqual(counter, 1);
        EventCenter.Default.Invoke(eventDef);
        Assert.AreEqual(counter, 2);
    }

    [Test]
    public void TestAdd1()
    {
        int counter = 0;
        string eventDef = "event02";
        Handle<object> handle = EventCenter.Default.AddListener(eventDef, n => counter += (int)n);
        EventCenter.Default.Invoke(eventDef, 1);
        Assert.AreEqual(counter, 1);
        EventCenter.Default.Invoke(eventDef, 2);
        Assert.AreEqual(counter, 3);
    }

    [Test]
    public void TestAddAndRemove0()
    {
        int counter = 0;
        string eventDef = "event03";
        Handle<object> handle = EventCenter.Default.AddListener(eventDef, () => counter++);
        EventCenter.Default.Invoke(eventDef);
        Assert.AreEqual(counter, 1);
        EventCenter.Default.Remove(eventDef, handle);
        EventCenter.Default.Invoke(eventDef);
        Assert.AreEqual(counter, 1);
    }

    [Test]
    public void TestAddAndRemove1()
    {
        int counter = 0;
        string eventDef = "event04";
        Handle<object> handle = EventCenter.Default.AddListener(eventDef, n => counter++);
        EventCenter.Default.Invoke(eventDef, 1);
        Assert.AreEqual(counter, 1);
        EventCenter.Default.Remove(eventDef, handle);
        EventCenter.Default.Invoke(eventDef, 1);
        Assert.AreEqual(counter, 1);
    }

    [Test]
    public void TestAddAndRemove2()
    {
        int counter = 0;
        string eventDef = "event05";
        Handle<object> handle = EventCenter.Default.AddListener(eventDef, () => counter++);
        EventCenter.Default.Invoke(eventDef);
        Assert.AreEqual(counter, 1);
        handle.Unlisten();
        EventCenter.Default.Invoke(eventDef);
        Assert.AreEqual(counter, 1);
    }

    [Test]
    public void TestAddAndRemove3()
    {
        int counter = 0;
        string eventDef = "event06";
        Handle<object> handle = EventCenter.Default.AddListener(eventDef, n => counter++);
        EventCenter.Default.Invoke(eventDef, 1);
        Assert.AreEqual(counter, 1);
        handle.Unlisten();
        EventCenter.Default.Invoke(eventDef, 1);
        Assert.AreEqual(counter, 1);
    }

    [Test]
    public void TestAddAndRemove4()
    {
        memberCount = 0;
        string eventDef = "event07";
        Handle<object> handle = EventCenter.Default.AddListener(eventDef, CountAdd);
        EventCenter.Default.Invoke(eventDef);
        Assert.AreEqual(memberCount, 1);
        EventCenter.Default.RemoveListener(eventDef, CountAdd);
        EventCenter.Default.Invoke(eventDef);
        Assert.AreEqual(memberCount, 1);
    }

    [Test]
    public void TestRemoveAll0()
    {
        int counter = 0;
        string eventDef1 = "event08";
        string eventDef2 = "event09";
        Handle<object> handle1 = EventCenter.Default.AddListener(eventDef1, () => counter++);
        Handle<object> handle2 = EventCenter.Default.AddListener(eventDef2, () => counter += 2);
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
        int counter = 0;
        string eventDef1 = "event10";
        string eventDef2 = "event11";
        Handle<object> handle1 = EventCenter.Default.AddListener(eventDef1, n => counter++);
        Handle<object> handle2 = EventCenter.Default.AddListener(eventDef2, s => counter += 2);
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
