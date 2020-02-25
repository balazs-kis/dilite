using System;

namespace DiLite.Builders
{
    public interface IContainerBuilder
    {
        IRegistrationBuilder RegisterType<T>() where T : class;
        IRegistrationBuilder RegisterType(Type t);

        IRegistrationBuilder RegisterFactoryMethod<T>(Func<IContainer, T> factoryMethod) where T : class;

        IContainer Build();
    }
}