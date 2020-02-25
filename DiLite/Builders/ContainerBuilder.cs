using System;
using System.Collections.Generic;
using System.Linq;

namespace DiLite.Builders
{
    public class ContainerBuilder : BuilderBase<IContainer>, IContainerBuilder
    {
        private readonly List<IRegistrationBuilder> _registrationBuilders;


        public ContainerBuilder()
        {
            _registrationBuilders = new List<IRegistrationBuilder>();
        }


        public IRegistrationBuilder RegisterType<T>() where T : class
        {
            return RegisterType(typeof(T));
        }

        public IRegistrationBuilder RegisterType(Type t)
        {
            CheckState();

            var regBuilder = new RegistrationBuilder(t);
            _registrationBuilders.Add(regBuilder);

            return regBuilder;
        }

        public IRegistrationBuilder RegisterFactoryMethod<T>(Func<IContainer, T> factoryMethod) where T : class
        {
            CheckState();

            var regBuilder = new RegistrationBuilder(factoryMethod, typeof(T));
            _registrationBuilders.Add(regBuilder);

            return regBuilder;
        }

        protected override IContainer BuildInternal() =>
            new Container(_registrationBuilders.Select(r => r.Build()));

    }
}