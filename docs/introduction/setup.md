# Setup

You can use SystemsRx with an infrastructure layer or just by itself, but it is recommended to use the infrastructure if possible.

> SystemsRx is more of a bare bones library which other libraries such as EcsRx builds on top of so while you can use this by itself you may want to look at the libraries that build on top of this layer.
 
## Pre built infrastructure or not?

Out the box SystemsRx comes with a load of infrastructure classes which simplify setup, if you are happy to use that as a basis for setup then your job becomes a bit simpler.

There is also a whole section in these docs around the infrastructure and how to use the application and other classes within there in common scenarios.

### YES I WANT ALL THE INFRASTRUCTURE PLX PLX

A wise choice so to start with its advised that you take:

- `SystemsRx`
- `SystemsRx.Infrastructure`

This will provide the basic classes for you to extend, however one fundamental piece of the puzzle is the DI abstraction layer. It doesn't really care which DI framework as it provides an interface for you to implement and then consume that in your own `EcsRxApplication` implementation.

So here are the main bits you need:

- [Download a premade provider](https://github.com/EcsRx/SystemsRx/tree/main/src/SystemsRx.Infrastructure.Ninject) or implement `IDependencyContainer` for your DI framework of choice.
- Extend `SystemsRxApplication` implementation, providing it the DI container provider you wish to use 

There are pre-made DI implementations for **Ninject** and **Zenject** so if you can use one of those on your platform GREAT! if not then just pick a DI framework of choice and implement your own handler for it (using the ninject one as an example to base it off).

> So if you dont know what DI (Dependency Injection) is I recommend you go [read this](https://grofit.gitbooks.io/development-for-winners/content/development/dependency-patterns/dependency-injection.html) and [this](https://grofit.gitbooks.io/development-for-winners/content/development/dependency-patterns/inversion-of-control.html) which will give you a quick overview on what IoC (Inversion of Control) and DI is and how you use it.

It is worth noting here that this is EXACTLY how the examples work in this project so its worth cracking them open to see how its all done, but the same principals can be applied to your own applications.

### NOOOPE! I dont care for your design patterns sir, just let me get going

Ok captain, if you just want to get things going with minimum effort then I would just get the core lib and manually instantiate everything that is needed. 

This is like the most bare bones setup I would advise:

```csharp
public abstract class SystemsRxApplication
{
	public ISystemExecutor SystemExecutor { get; }
	public IEventSystem EventSystem { get; }
	public IUpdateScheduler UpdateScheduler { get; }

	protected EcsRxApplication()
	{
		// For sending events around
		EventSystem = new EventSystem(new MessageBroker());
	    
	    // Update scheduler for deciding when an *update* should happen
	    UpdateScheduler = new DefaultUpdateScheduler(); // 60 fps default
	
		// All system handlers for the system types you want to support
		var manualSystemHandler = new ManualSystemHandler(UpdateScheduler);
		var basicSystemHandler = new BasicSystemHandler();
		var reactToEventSystemHandler = new ReactToEventSystemHandler(EventSystem);

		var conventionalSystems = new List<IConventionalSystemHandler>
		{
			manualSystemHandler,
            basicSystemHandler,
            reactToEventSystemHandler
		};
		
		// The main executor which manages how systems are given information
		SystemExecutor = new SystemExecutor(conventionalSystems);
	}

	public abstract void StartApplication();
}
```

Then all you need to do is go:

```csharp
public class HelloWorldExampleApplication : EcsRxApplication
{
	public override void StartApplication()
	{
		SystemExecutor.AddSystem(new SomeSystemHere());
	}
}
```

HUZZAH! you are now up and running and you can make your own conventions or design patterns around this.