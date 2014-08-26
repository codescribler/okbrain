using System;
using System.Configuration;
using Nancy.TinyIoc;
using Raven.Client;
using Raven.Client.Document;
using okbrain.Domain.Services;
using okbrain.Services;


namespace okbrain
{
    using Nancy;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ApplicationStartup(TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            StaticConfiguration.DisableErrorTraces = false;

            var store = new DocumentStore
            {
                ConnectionStringName = "RavenDB"
            };

            store.Initialize();

            container.Register(store, "DocStore");

            string isLive = ConfigurationManager.AppSettings["live"];

            if (string.IsNullOrEmpty(isLive))
            {
                container.Register<ITaxonomy, LocalTaxonomyService>();
            }
            else
            {
                container.Register<ITaxonomy, AlchemyTaxonomy>();
            }
            

        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            var docStore = container.Resolve<DocumentStore>("DocStore");
            var documentSession = docStore.OpenSession();

            container.Register<IDocumentSession>(documentSession);
            container.Register<IPostSlugDuplicateDetector, PostSlugDuplicateDetector>();

        }

        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            //nancyConventions.StaticContentsConventions.Add(Nancy.Conventions.StaticContentConventionBuilder.AddDirectory("/", "public"));

        }
    }
}