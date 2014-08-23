using System;
using Nancy.TinyIoc;
using Raven.Client;
using Raven.Client.Document;
using okbrain.Domain.Services;

namespace okbrain
{
    using Nancy;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var store = new DocumentStore
            {
                ConnectionStringName = "RavenDB"
            };

            store.Initialize();

            container.Register(store, "DocStore");
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            

            var docStore = container.Resolve<DocumentStore>("DocStore");
            var documentSession = docStore.OpenSession();

            container.Register<IDocumentSession>(documentSession);
            container.Register<IPostSlugDuplicateDetector, PostSlugDuplicateDetector>();
        }
    }
}