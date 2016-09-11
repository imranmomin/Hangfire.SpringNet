using System;

using Hangfire;
using Spring.Context;
using Spring.Context.Support;

namespace ProjectiveGraph.Core.Hangfire
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration<SpringJobActivator> UseSpringActivator(this IGlobalConfiguration configuration, IApplicationContext context)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (context == null) throw new ArgumentNullException("context");

            return configuration.UseActivator(new SpringJobActivator(context));
        }

        public static IGlobalConfiguration<SpringJobActivator> UseSpringActivator(this IGlobalConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            IApplicationContext context = ContextRegistry.GetContext();
            if (context == null) throw new ArgumentNullException("context");

            return configuration.UseActivator(new SpringJobActivator(context));
        }
    }
}
