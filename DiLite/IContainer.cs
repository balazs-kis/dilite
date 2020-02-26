using System;
using System.Collections.Generic;

namespace DiLite
{
    /// <summary>
    /// Dependency injection container.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Resolves the given type (<i>alias</i>) from the container.
        /// </summary>
        /// <typeparam name="T">The type (<i>alias</i>) to resolve</typeparam>
        /// <returns>An instance of the given type (<i>alias</i>)</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Resolves the given type (<i>alias</i>) from the container.
        /// </summary>
        /// <param name="t">The (<i>alias</i>) type to resolve</param>
        /// <returns>A boxed instance of the given type (<i>alias</i>) </returns>
        object Resolve(Type t);

        /// <summary>
        /// Resolves all registrations for the given type (<i>alias</i>) from the container.
        /// </summary>
        /// <typeparam name="T">The type (<i>alias</i>) to resolve</typeparam>
        /// <returns>The instances of the given type (<i>alias</i>)</returns>
        IEnumerable<T> ResolveAll<T>() where T : class;

        /// <summary>
        /// Resolves all registrations for the given type (<i>alias</i>) from the container.
        /// </summary>
        /// <param name="t">The type (<i>alias</i>) to resolve</param>
        /// <returns>The instances of the given type (<i>alias</i>)</returns>
        IEnumerable<object> ResolveAll(Type t);
    }
}