## Easy Event

Another choice to build your event systems in C#.

EasyEvent provides the following interfaces to help you handle callbacks more easily:

1. AddListener/Remove/RemoveListener

Unlike C# events, when a callback function is added as a listener, the EasyEvent instance returns a Handle, which is intended to facilitate the removal of anonymous methods and lambda expressions.

```C#

```

To remove a listener, just call Remove and pass the handle. You can also use RemoveListener, but it is not efficient.

```C#

```

2. ListenOnce

In some cases, you only need to listen to an Event once and then remove it, and ListenOnce is ready for you.

```C#

```

3. SafeInvoke

The invocation continues when exception occurs and the exceptions are aggregated and thrown when the event call ends;

```C#

```
