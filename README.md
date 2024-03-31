## EasyEvent

EasyEvent is a lightweight and performant event system for Unity that provides an alternative to the traditional C# event and UnityEvent systems. It offers a more user-friendly API and is designed with performance in mind.

### Features

#### User-Friendly API

EasyEvent provides a more intuitive and user-friendly API compared to UnityEvent and C# events. With EasyEvent, you can register a lambda function to an EasyDelegate/EasyEvent and obtain an EventHandle, which can be used to remove the listener later. This separation of listener and invoker behavior is similar to that of a C# event, making it easier to work with events in Unity. Additionally, the built-in EventCenter can be used as a messaging system in new projects.

#### Performance Optimization

EasyEvent is a lightweight solution that offers improved performance over C# events and UnityEvent in most cases. In a performance test that involved registering, executing, and unregistering events 100 times, the statistics showed lower memory allocation and faster execution times for EasyEvent compared to C# events and UnityEvent.

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

#### MIT License

EasyEvent is licensed under the MIT License, which means there are no restrictions for using it in commercial projects.

### Installation

EasyEvent can be installed via the Unity Package Manager. Alternatively, you can clone the repository and import the package manually.

####  Via the Unity Package Manager

<details><summary>Steps for installing via the Unity Package Manager</summary>

- Open the Unity Package Manager from the Window menu.

- Click the + button in the top left corner and select "Add package from git URL".

- Enter the following URL: `https://github.com/aillieo/EasyEvent.git#upm`

- Click the "Add" button to add the package to your project.</details>

#### Manual Installation

<details><summary>Steps for manual Installation</summary>

- Clone the repository to your local machine.

- Open your Unity project and navigate to the "Packages" folder.

- Drag the "EasyEvent" folder from the cloned repository into the "Packages" folder.

- Unity will import the package automatically.</details>

### Quick Start

#### Using an EasyDelegate

You can add listeners to an EasyDelegate and remove them using the returned handle.

```C#
EasyDelegate someDel = new EasyDelegate();
someDel.AddListener(() => Debug.Log("someDel invoked"));

someDel.Invoke();
// Output:
// someDel invoked
```

To remove a listener, call `Remove` and pass the handle.

```C#
EasyDelegate someDel = new EasyDelegate();
var handle = someDel.AddListener(SomeMethod);

// To remove the listener:
someDel.Remove(handle);
// or:
// someDel.RemoveListener(SomeMethod);
```

#### Declaring an Event

When using EasyEvent, declare it as a property and define an EasyDelegate as the backing field.

```C#
public class Foo
{
    private readonly EasyDelegate<BattleResult> battleEndDel = new EasyDelegate<BattleResult>();

    public EasyEvent<BattleResult> OnBattleEnd => battleEndDel;
}
```

Register listeners in other classes.

```C#
public class Bar
{
    private void Initialize()
    {
        var handle = instanceOfFoo.OnBattleEnd.AddListener(res => Debug.Log("Battle ended"));
    }
}
```

Invoke the event in the declaring class.

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

#### ListenOnce/ListenUntil

If you only need to listen to an event once and then remove the listener, you can use `ListenOnce`.

```C#
EasyEvent evt = ...;
evt.AddListener(() => Debug.Log("Only once"));

evt.Invoke();
// Output:
// Only once

evt.Invoke();
// No output
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

#### InvokeAll

Event invocation continues even if an exception occurs. Exceptions are aggregated and thrown when the event call ends.

```C#
Event evt = new Event();
evt.AddListener(() => throw new Exception());
evt.AddListener(() => Debug.Log("Event invoked"));
evt.InvokeAll();

// Output:
// Event invoked
// Exception: Exception of type 'System.Exception' was thrown.
```

### API Reference

For detailed usage and documentation, please refer to the API reference.

### Limitations

EasyEvent is not thread-safe, so extra care is required when using it with multiple threads.