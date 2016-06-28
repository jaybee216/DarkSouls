using System;
using Autofac;
using DataAccess;

namespace DarkSoulsII.Infrastructure
{
    public class IoC
    {
        private static IContainer _container = RegisterDependencies();
        private static ContainerBuilder _builder;
        private static Object _lock = new object();

        private static IContainer RegisterDependencies()
        {
            _builder = new ContainerBuilder();
            _builder.RegisterType<Cache>().As<ICache>();
            _builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            return _builder.Build();
        }

        private static void EnsureContainerIsBuilt()
        {
            // This If statement avoids locking if _container is not null
            if (_container == null)
            {
                // Synchronized block to avoid calling _builder.Build() more than once in a multi-threaded environment
                lock (_lock)
                {
                    // If _container is still null (i.e. has not been initiallized on another thread)
                    if (_container == null)
                    {
                        _container = _builder.Build();
                    }
                }
            }
        }

        public static IContainer Container
        {
            get
            {
                EnsureContainerIsBuilt();
                return _container;
            }
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static void RegisterType<T1, T2>()
        {
            _builder.RegisterType<T2>().As<T1>();
        }

        /// <summary>
        /// For Unit Tests
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        public static void RegisterSingleInstance<T1, T2>()
        {
            _builder.RegisterType<T2>().As<T1>().SingleInstance();
        }

        /// <summary>
        /// For Unit Tests
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="value"></param>
        public static void RegisterInstance<T1>(object value)
        {
            _builder.RegisterInstance(value).As<T1>();
        }

        public static void Clear()
        {
            _builder = new ContainerBuilder();
            _container = null;
        }
    }
}