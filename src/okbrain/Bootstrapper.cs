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

        protected override void ApplicationStartup(TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            //container.RegisterMultiple(typeof(IRepo<>),
            //                           AppDomain.CurrentDomain.GetAssemblies()
            //                                    .ToList()
            //                                    .SelectMany(s => s.GetTypes())
            //                                    .Where(x => x.IsAssignableFrom(x) && x.IsClass)
            //                                    .ToList());
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

        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);



            var docStore = container.Resolve<DocumentStore>("DocStore");
            var documentSession = docStore.OpenSession();

            container.Register<IDocumentSession>(documentSession);
            container.Register<IPostSlugDuplicateDetector, PostSlugDuplicateDetector>();

            //var type = typeof(IDto);
            //var types = AppDomain.CurrentDomain.GetAssemblies().ToList()
            //    .SelectMany(s => s.GetTypes())
            //    .Where(x => type.IsAssignableFrom(x) && x.IsClass);
            //container.RegisterMultiple<IDto>(types.ToArray());
            //container.Register(typeof(IRepo<>), typeof(RavenRepo<>)).AsMultiInstance(); 

        }

        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            //nancyConventions.StaticContentsConventions.Add(Nancy.Conventions.StaticContentConventionBuilder.AddDirectory("/", "public"));

        }
    }
}