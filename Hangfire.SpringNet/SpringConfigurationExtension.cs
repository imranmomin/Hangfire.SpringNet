using System;

using Spring.Context;
using Spring.Context.Support;

namespace Hangfire
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration<SpringJobActivator> UseSpringActivator(this IGlobalConfiguration configuration, IApplicationContext context)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (context == null) throw new ArgumentNullException(nameof(context));

            return configuration.UseActivator(new SpringJobActivator(context));
        }

        public static IGlobalConfiguration<SpringJobActivator> UseSpringActivator(this IGlobalConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            IApplicationContext context = ContextRegistry.GetContext();
            return configuration.UseSpringActivator(context);
        }
    }
}