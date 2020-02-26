using System;
using System.Collections.Generic;

namespace DiLite.Registrations
{
    /// <summary>
    /// Represents a dependency injection registration for a given type.
    /// </summary>
    public interface IRegistration
    {
        /// <summary>
        /// Contains the <i>aliases</i> for the type.
        /// </summary>
        IReadOnlyList<Type> Aliases { get; }

        /// <summary>
        /// If true, only a single instance will be created of the registered type.
        /// </summary>
        bool IsSingleInstance { get; }

        /// <summary>
        /// Creates an instance of the registered type. Called by the <see cref="IContainer"/> during resolution.
        /// </summary>
        /// <param name="container">The container to resolve the dependencies with</param>
        /// <returns>A boxed instance of the registered type</returns>
        object Activate(IContainer container);
    }
}