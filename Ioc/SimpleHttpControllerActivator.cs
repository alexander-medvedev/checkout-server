using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Ioc
{
    /// <summary>
    /// Controller activator based on explicitely registered controllers
    /// In reality this should use some IoC container to resolve dependencies
    /// </summary>
    public class SimpleHttpControllerActivator : IHttpControllerActivator
    {
        private readonly Dictionary<Type, Func<IHttpController>> resolvers = new Dictionary<Type, Func<IHttpController>>();

        /// <summary>
        /// Register a resolver for a controller
        /// </summary>
        /// <typeparam name="T">Controller type</typeparam>
        /// <param name="controllerResolver">Controller creation function</param>
        public void Register<T>(Func<T> controllerResolver) where T : IHttpController =>
            this.resolvers.Add(typeof(T), () => controllerResolver());

        /// <summary>
        /// Create a controller for the given request
        /// </summary>
        /// <param name="request">Requesst</param>
        /// <param name="controllerDescriptor">Controller descriptor</param>
        /// <param name="controllerType">Controller type</param>
        /// <returns>Controller instance if it was registered, null overwise</returns>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType) =>
            this.resolvers.TryGetValue(controllerType, out Func<IHttpController> resolver) ? resolver() : null;
    }
}
