using Ninject;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharpPlanOut.Di
{
    public class InversionOfControlContainer
    {
        private static IKernel _kernel;
        private static readonly InversionOfControlContainer InversionOfControlContainerInstance = new InversionOfControlContainer();

        private InversionOfControlContainer()
        {
        }

        public IKernel Kernel
        {
            get { return _kernel ?? (_kernel = CreateKernel()); }
        }

        public static InversionOfControlContainer GetInversionOfControlContainer()
        {
            return InversionOfControlContainerInstance;
        }

        public IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            _kernel = kernel;
            return kernel;
        }

        public T Get<T>()
        {
            return Kernel.Get<T>();
        }

        public object Get(Type type)
        {
            return Kernel.Get(type);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return Kernel.GetAll<T>();
        }

        public void Rebind<T, TC>() where TC : T
        {
            Kernel.Rebind<T>().To<TC>();
        }

        public void Load(params Assembly[] assemblies)
        {
            Kernel.Load(assemblies);
        }
    }
}