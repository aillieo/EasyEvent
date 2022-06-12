using System;
using AillieoUtils;
using NUnit.Framework;
using UnityEngine.Events;

[Category("PerformanceTest")]
public class Performance
{
    [Test, Order(100)]
    public static void TestBatch()
    {
        for (int i = 0; i < 100; ++i)
        {
            Performance performance = new Performance();

            performance.RegisterEasyEvent();
            performance.RunEasyEvent();
            performance.UnregisterEasyEvent();

            performance.RegisterUnityEvent();
            performance.RunUnityEvent();
            performance.UnregisterUnityEvent();

            performance.RegisterCSEvent();
            performance.RunCSEvent();
            performance.UnregisterCSEvent();
        }
    }

    private AillieoUtils.Event event1 = new Event();
    private UnityEngine.Events.UnityEvent event2 = new UnityEngine.Events.UnityEvent();
    private event Action event3;

    private int loopCount = 100;

    private int counter = 0;
    private Action callback;
    private UnityAction unityCallback;
    private int result;

    public Performance()
    {
        callback = () => counter++;
        unityCallback = () => counter++;
    }

    [Test, Order(1)]
    public void RegisterEasyEvent()
    {
        for (int i = 0; i < loopCount; i++)
        {
            event1.AddListener(callback);
        }
    }

    [Test, Order(2)]
    public void RunEasyEvent()
    {
        counter = 0;
        for (int i = 0; i < loopCount; i ++)
        {
            event1.Invoke();
        }

        result = counter;
    }

    [Test, Order(3)]
    public void UnregisterEasyEvent()
    {
        event1.RemoveAllListeners();
    }

    [Test, Order(1)]
    public void RegisterUnityEvent()
    {
        for (int i = 0; i < loopCount; i++)
        {
            event2.AddListener(unityCallback);
        }
    }

    [Test, Order(2)]
    public void RunUnityEvent()
    {
        counter = 0;
        for (int i = 0; i < loopCount; i++)
        {
            event2.Invoke();
        }

        result = counter;
    }

    [Test, Order(3)]
    public void UnregisterUnityEvent()
    {
        event2.RemoveAllListeners();
    }

    [Test, Order(1)]
    public void RegisterCSEvent()
    {
        for (int i = 0; i < loopCount; i++)
        {
            event3 += callback;
        }
    }

    [Test, Order(2)]
    public void RunCSEvent()
    {
        counter = 0;
        for (int i = 0; i < loopCount; i++)
        {
            event3.Invoke();
        }

        result = counter;
    }

    [Test, Order(3)]
    public void UnregisterCSEvent()
    {
        var list = event3.GetInvocationList();
        foreach (var e in list)
        {
            event3 -= (Action)e;
        }
    }
}
