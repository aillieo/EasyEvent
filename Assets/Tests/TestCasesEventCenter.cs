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
        Handle<object> handle = EventCenter.AddListener(eventDef, () => counter++);
        EventCenter.Invoke(eventDef);
        Assert.AreEqual(counter, 1);
        EventCenter.Invoke(eventDef);
        Assert.AreEqual(counter, 2);
    }

    [Test]
    public void TestAdd1()
    {
        int counter = 0;
        string eventDef = "event02";
        Handle<object> handle = EventCenter.AddListener(eventDef, n => counter += (int)n);
        EventCenter.Invoke(eventDef, 1);
        Assert.AreEqual(counter, 1);
        EventCenter.Invoke(eventDef, 2);
        Assert.AreEqual(counter, 3);
    }

    [Test]
    public void TestAddAndRemove0()
    {
        int counter = 0;
        string eventDef = "event03";
        Handle<object> handle = EventCenter.AddListener(eventDef, () => counter++);
        EventCenter.Invoke(eventDef);
        Assert.AreEqual(counter, 1);
        EventCenter.Remove(eventDef, handle);
        EventCenter.Invoke(eventDef);
        Assert.AreEqual(counter, 1);
    }

    [Test]
    public void TestAddAndRemove1()
    {
        int counter = 0;
        string eventDef = "event04";
        Handle<object> handle = EventCenter.AddListener(eventDef, n => counter++);
        EventCenter.Invoke(eventDef, 1);
        Assert.AreEqual(counter, 1);
        EventCenter.Remove(eventDef, handle);
        EventCenter.Invoke(eventDef, 1);
        Assert.AreEqual(counter, 1);
    }

    [Test]
    public void TestAddAndRemove2()
    {
        int counter = 0;
        string eventDef = "event05";
        Handle<object> handle = EventCenter.AddListener(eventDef, () => counter++);
        EventCenter.Invoke(eventDef);
        Assert.AreEqual(counter, 1);
        handle.Unlisten();
        EventCenter.Invoke(eventDef);
        Assert.AreEqual(counter, 1);
    }

    [Test]
    public void TestAddAndRemove3()
    {
        int counter = 0;
        string eventDef = "event06";
        Handle<object> handle = EventCenter.AddListener(eventDef, n => counter++);
        EventCenter.Invoke(eventDef, 1);
        Assert.AreEqual(counter, 1);
        handle.Unlisten();
        EventCenter.Invoke(eventDef, 1);
        Assert.AreEqual(counter, 1);
    }

    [Test]
    public void TestAddAndRemove4()
    {
        memberCount = 0;
        string eventDef = "event07";
        Handle<object> handle = EventCenter.AddListener(eventDef, CountAdd);
        EventCenter.Invoke(eventDef);
        Assert.AreEqual(memberCount, 1);
        EventCenter.RemoveListener(eventDef, CountAdd);
        EventCenter.Invoke(eventDef);
        Assert.AreEqual(memberCount, 1);
    }

    [Test]
    public void TestRemoveAll0()
    {
        int counter = 0;
        string eventDef1 = "event08";
        string eventDef2 = "event09";
        Handle<object> handle1 = EventCenter.AddListener(eventDef1, () => counter++);
        Handle<object> handle2 = EventCenter.AddListener(eventDef2, () => counter += 2);
        EventCenter.Invoke(eventDef1);
        Assert.AreEqual(counter, 1);
        EventCenter.Invoke(eventDef2);
        Assert.AreEqual(counter, 3);
        EventCenter.Clear();
        EventCenter.Invoke(eventDef1);
        Assert.AreEqual(counter, 3);
        EventCenter.Invoke(eventDef2);
        Assert.AreEqual(counter, 3);
    }

    [Test]
    public void TestRemoveAll1()
    {
        int counter = 0;
        string eventDef1 = "event10";
        string eventDef2 = "event11";
        Handle<object> handle1 = EventCenter.AddListener(eventDef1, n => counter++);
        Handle<object> handle2 = EventCenter.AddListener(eventDef2, s => counter += 2);
        EventCenter.Invoke(eventDef1, 1);
        Assert.AreEqual(counter, 1);
        EventCenter.Invoke(eventDef2, "1");
        Assert.AreEqual(counter, 3);
        EventCenter.Clear();
        EventCenter.Invoke(eventDef1, 1);
        Assert.AreEqual(counter, 3);
        EventCenter.Invoke(eventDef2, "1");
        Assert.AreEqual(counter, 3);
    }
}
