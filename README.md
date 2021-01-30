## Easy Event

Another choice to build your event systems in C#.

EasyEvent provides the following interfaces to help you handle callbacks more easily:

1. AddListener/Remove/RemoveListener

Unlike C# events, when a callback function is added as a listener, the EasyEvent instance returns a Handle, which is intended to facilitate the removal of anonymous methods and lambda expressions.

```C#
// add listener
Event evt = new Event();
evt.AddListener(() => Debug.Log("evt invoke"));

// invoke
evt.Invoke();
// will output:
// evt invoke
```

To remove a listener, just call Remove and pass the handle. You can also use RemoveListener, but it is not efficient.

```C#
Event evt = new Event();
var handle = evt.AddListener(SomeMethod);

// to remove you can:
evt.Remove(handle);
// or:
// evt.RemoveListener(SomeMethod);
```

2. ListenOnce

In some cases, you only need to listen to an Event once and then remove it, and ListenOnce is ready for you.

```C#
Event evt = new Event();
evt.AddListener(() => Debug.Log("only once"));

evt.Invoke();
// will output:
// only once

evt.Invoke();
// will output nothing
```

3. SafeInvoke

The invocation continues when exception occurs and the exceptions are aggregated and thrown when the event call ends;

```C#
Event evt = new Event();
evt.AddListener(() => throw new Exception());
evt.AddListener(() => Debug.Log("evt invoke"));
evt.SafeInvoke();

// will output:
// evt invoke
// Exception: Exception of type 'System.Exception' was thrown.
```

## Usage

Clone this repository and copy it to your project folder, or add `https://github.com/aillieo/EasyEvent.git#upm` as a dependency in the package management window.

