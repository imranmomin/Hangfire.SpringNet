using System;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SpringJobActivator"/>
        /// class with a given Spring.Net Context.
        /// </summary>
        /// <param name="context">Context that will be used to create instance
        /// of classes during job activation process.</param>
        public SpringJobActivator(IApplicationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            this.context = context;
        }

        /// <inheritdoc />
        public override object ActivateJob(Type jobType)
        {
            IDictionary<string, object> objects = context.GetObjectsOfType(jobType);
            if (objects.Count > 0) return objects.Select(v => v.Value).FirstOrDefault();
            return null;
        }
    }
}
