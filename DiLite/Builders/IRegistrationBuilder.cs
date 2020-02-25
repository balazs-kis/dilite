using DiLite.Registrations;
using System;

namespace DiLite.Builders
{
    public interface IRegistrationBuilder
    {
        IRegistrationBuilder As<T>();
        IRegistrationBuilder As(Type t);

        IRegistrationBuilder AsSelf();

        IRegistrationBuilder AsSingleInstance();

        IRegistration Build();
    }
}