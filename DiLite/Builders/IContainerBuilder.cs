using System;

namespace DiLite.Builders
{
    /// <summary>
    /// Used to construct a dependency injection container.
    /// </summary>
    public interface IContainerBuilder
    {
        /// <summary>
        /// Register a type into the container.
        /// </summary>
        /// <typeparam name="T">The type to register</typeparam>
        /// <returns>A <see cref="IRegistrationBuilder"/> instance to configure the registration</returns>
        IRegistrationBuilder RegisterType<T>() where T : class;

        /// <summary>
        /// Register a type into the container.
        /// </summary>
        /// <param name="t">The type to register</param>
        /// <returns>A <see cref="IRegistrationBuilder"/> instance to configure the registration</returns>
        IRegistrationBuilder RegisterType(Type t);

        /// <summary>
        /// Register a factory method for a type into the container.
        /// </summary>
        /// <typeparam name="T">The return type of the factory method, the type to register</typeparam>
        /// <param name="factoryMethod">The factory method which will return an instance of the specified type</param>
        /// <returns>A <see cref="IRegistrationBuilder"/> instance to configure the registration</returns>
        IRegistrationBuilder RegisterFactoryMethod<T>(Func<IContainer, T> factoryMethod) where T : class;

        /// <summary>
        /// Builds the <see cref="IContainer"/> instance. Call this method when all the registrations are done.
        /// </summary>
        /// <returns>The <see cref="IContainer"/> instance</returns>
        IContainer Build();
    }
}