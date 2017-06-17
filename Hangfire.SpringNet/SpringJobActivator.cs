using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Generic;

using Spring.Context;

namespace Hangfire
{
    /// <summary>
    /// HangFire Job Activator based on Spring.Net IoC Container.
    /// </summary>
    public sealed class SpringJobActivator : JobActivator
    {
        private readonly IApplicationContext context;
        private readonly List<string> disposableTypes = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SpringJobActivator"/>
        /// class with a given Spring.Net Context.
        /// </summary>
        /// <param name="context">Context that will be used to create instance
        /// of classes during job activation process.</param>
        public SpringJobActivator(IApplicationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.context = context;
        }

        /// <inheritdoc />
        public override object ActivateJob(Type jobType)
        {
            IDictionary<string, object> objects = context.GetObjectsOfType(jobType);
            KeyValuePair<string, object> instance = objects.First();

            if (!context.IsSingleton(instance.Key) && instance.Value is IDisposable)
            {
                string fullName = instance.Value.GetType().FullName;
                if (!disposableTypes.Exists(x => x.Equals(fullName)))
                {
                    disposableTypes.Add(fullName);
                }
            }

            return instance.Value;
        }

        /// <inheritdoc />
        public override JobActivatorScope BeginScope(JobActivatorContext context) => new SpringJobActivatorScope(this);

        private void Dispose(IDisposable instance)
        {
            if (instance != null)
            {
                string fullName = instance.GetType().FullName;
                if (disposableTypes.Exists(x => x.Equals(fullName)))
                {
                    instance.Dispose();
                }
            }
        }

        class SpringJobActivatorScope : JobActivatorScope
        {
            private readonly SpringJobActivator activator;
            private IDisposable disposableInstance;

            public SpringJobActivatorScope(SpringJobActivator activator)
            {
                if (activator == null) throw new ArgumentNullException(nameof(activator));
                this.activator = activator;
            }

            public override object Resolve(Type type)
            {
                object instance = activator.ActivateJob(type);
                disposableInstance = instance as IDisposable;
                return instance;
            }

            public override void DisposeScope() => activator.Dispose(disposableInstance);
        }
    }
}
