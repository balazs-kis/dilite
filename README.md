![DiLite](/logo.png)

DiLite /dɪˈlʌɪt/ is a minimalist, lightweight DI framework. Registrations and resolutions will look very familiar to those accustomed to Autofac.

![Build status: unknown](https://travis-ci.org/balazs-kis/dilite.svg?branch=master)

## Usage

Create a `ContainerBuilder`, make the registrations, then build your `Container`:
```csharp
var containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<MyClass>().As<IMyClass>();
var container = containerBuilder.Build();
```
Then simply resolve what you need from the `Container`:
```csharp
var myInstance = container.Resolve<IMyClass>();
```


## Features

### 1. Single instance
Each resulution call to the container will create a new instance of the registered type, unless it is specified as a singleton during registration. To register a type as a singleton, just use the `AsSingleInstance` method:
```csharp
containerBuilder.RegisterType<MyClass>().As<IMyClass>().AsSingleInstance();
```

### 2. As self
If no *alias* is specified during the registration of a type, it will be registered as itself.
```csharp
containerBuilder.RegisterType<MyClass>();
```
It can be resolved as:
```csharp
var myInstance = container.Resolve<MyClass>();
```

If another *alias* is specified, but the registration as itself is also needed, the `AsSelf` method must be used:
```csharp
containerBuilder.RegisterType<MyClass>().As<IMyClass>().AsSelf();
```

### 3. Multiple registrations for the same *alias*
Multiple registrations can be made for the same *alias*:
```csharp
containerBuilder.RegisterType<MyClassImplementation1>().As<IMyClass>();
containerBuilder.RegisterType<MyClassImplementation2>().As<IMyClass>();
```
In this case, the `Resolve` method will return an instance of `MyClassImplementation2` because it was the last one registered for that *alias*. By using the `ResolveAll` method, all registrations for the *alias* will be returned:
```csharp
IEnumerable<IMyClass> myInstances = container.ResolveAll<IMyClass>();
```

### 4. Factory method registrations
Factory methods can also be registered as `Func<IContainer, T>`:
```csharp
containerBuilder.RegisterFactoryMethod(container =>
{
    var dep1 = container.Resolve<IDependency1>();
    var dep2 = container.Resolve<IDependency2>();
    return new MyClass(dep1, dep2);
}).As<IMyClass>();
```
These registrations can be resolved as their simple type counterparts:
```csharp
var myInstance = container.Resolve<IMyClass>();
```
