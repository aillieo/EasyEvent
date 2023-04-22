## EasyEvent

EasyEvent is a lightweight and performant event system for Unity, providing an alternative to the traditional C# event and UnityEvent systems. It is designed with performance in mind and provides a more user-friendly API for developers.

## Features

### User-Friendly

EasyEvent provides a more user-friendly API for developers, making it easier to work with events in Unity. Developers can register a lambda to one EasyDelegate/EasyEvent to get an EventHandle, which can be used to remove the listener later. Unlike UnityEvent, when using an EasyEvent, the listener and invoker are separate, similar to the behavior of a C# event. For this reason, the EasyEvent and EasyDelegate are implemented separately. The built-in EventCenter can be used as an out-of-the-box messaging system when starting a new project.

### Performance

Compared to C# event and UnityEvent, EasyEvent is a lightweight solution that provides better performance in most cases. A simple test was conducted using three types of events. The test involved registering, executing, and unregistering 100 times for every event, and the results showed the following statistics data:

| Action | Event type | Total GC Alloc | Total time (ms) |
| --- | --- | --- | --- |
| Register | C# Event | 5.2M | 14.68 |
| Register | UnityEvent | 450.0KB | 11.59 |
| Register | EasyEvent    | 468.8KB | 10.46 |
| Execute | C# Event | 0 B | 80.28 |
| Execute | UnityEvent | 81.3 KB | 500.92 |
| Execute | EasyEvent    | 0 B | 82.38 |
| Unregister | C# Event | 5.2 MB | 40.06 |
| Unregister | UnityEvent | 0 | 0.11 |
| Unregister | EasyEvent    | 0 | 0.06 |

### MIT License

EasyEvent is licensed under the MIT License, which denotes that there are no restrictions for commercial projects.

## Installation

EasyEvent can be installed via the Unity Package Manager. Alternatively, you can clone the repository and import the package manually.

###  Via the Unity Package Manager

<details><summary>Steps for installing via the Unity Package Manager</summary>

- Open the Unity Package Manager from the Window menu.

- Click the + button in the top left corner and select "Add package from git URL".

- Enter the following URL: `https://github.com/aillieo/EasyEvent.git#upm`

- Click the "Add" button to add the package to your project.</details>

### Manual Installation

<details><summary>Steps for manual Installation</summary>

- Clone the repository to your local machine.

- Open your Unity project and navigate to the "Packages" folder.

- Drag the "EasyEvent" folder from the cloned repository into the "Packages" folder.

- Unity will import the package automatically.</details>

## Quick Start

### Use an EasyDelegate

AddListener/Remove/RemoveListener

When a callback function is added as a listener, the EasyEvent instance returns a `Handle`, which is intended to facilitate the removal of anonymous methods and lambda expressions. 

```C#
// add listener
EasyDelegate someDel = new EasyDelegate();
someDel.AddListener(() => Debug.Log("someDel invoke"));

// invoke
someDel.Invoke();
// output:
// someDel invoke
```

To remove a listener, simply call `Remove` and pass the handle. You can also use `RemoveListener`, but it is not efficient.

```C#
EasyDelegate someDel = new EasyDelegate();
var handle = someDel.AddListener(SomeMethod);

// to remove you can:
someDel.Remove(handle);
// or:
// someDel.RemoveListener(SomeMethod);
```

### Declare an event

When using EasyEvent, declare it as a property and define an EasyDelegate as the backing field.

```C#
public class Foo
{
    private readonly EasyDelegate<BattleResult> battleEndDel = new EasyDelegate<BattleResult>();

    public EasyEvent<BattleResult> OnBattleEnd => battleEndDel;
}
```

Register in other classes:

```C#
public class Bar
{
    private void Initialize()
    {
        var handle = instanceOfFoo.OnBattleEnd.AddListener(res => Debug.Log("battle end"));
    }
}
```

Invoke in the declaring class:

```C#
public class Foo
{
    private void WhenBattleEnds()
    {
        BattleResult battleResult = new BattleResult();
        this.battleEndDel.Invoke(battleResult);    
    }
}
```

### ListenOnce/ListenUntil

In some cases, you only need to listen to an event once and then remove it, and `ListenOnce` is ready for you.

```C#
EasyEvent evt = ...;
evt.AddListener(() => Debug.Log("only once"));

evt.Invoke();
// output:
// only once

evt.Invoke();
// no output
```

Similarly, use `ListenUntil` when you need to keep listening for events until a certain condition is met.

```C#
EasyEvent<int> evt = ...;

evt.ListenUntil(arg =>
{
    if (IsTimeToProcess(arg))
    {
        Process();
        return true;
    }

    return false;
});
```

### InvokeAll

The invocation continues when an exception occurs, and the exceptions are aggregated and thrown when the event call ends.

```C#
Event evt = new Event();
evt.AddListener(() => throw new Exception());
evt.AddListener(() => Debug.Log("evt invoke"));
evt.SafeInvoke();

// output:
// evt invoke
// Exception: Exception of type 'System.Exception' was thrown.
```

## API Reference

For detailed usage and documentation, please refer to the API reference.

## Limitations

EasyEvent is not thread-safe, so extra care is required when using it with multiple threads.