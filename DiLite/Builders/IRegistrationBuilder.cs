using DiLite.Registrations;
using System;

namespace DiLite.Builders
{
    /// <summary>
    /// Used to construct a dependency injection registration.
    /// </summary>
    public interface IRegistrationBuilder
    {
        /// <summary>
        /// Adds an <i>alias</i> to the registration.
        /// </summary>
        /// <typeparam name="T">The <i>alias</i> type</typeparam>
        /// <returns>The registration builder itself to chain other methods</returns>
        IRegistrationBuilder As<T>();

        /// <summary>
        /// Adds an <i>alias</i> to the registration.
        /// </summary>
        /// <param name="t">he <i>alias</i> type</param>
        /// <returns>The registration builder itself to chain other methods</returns>
        IRegistrationBuilder As(Type t);

        /// <summary>
        /// Adds the registered type as its own <i>alias</i>.
        /// </summary>
        /// <returns>The registration builder itself to chain other methods</returns>
        IRegistrationBuilder AsSelf();

        /// <summary>
        /// Marks the registration as a singleton.
        /// Only one instance will be created and returned for every resolution request.
        /// </summary>
        /// <returns>The registration builder itself to chain other methods</returns>
        IRegistrationBuilder AsSingleInstance();
    }
}