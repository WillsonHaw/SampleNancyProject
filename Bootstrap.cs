using Nancy;
using SampleNancyProject.Model;
using SampleNancyProject.Persistence;
using Raven.Client;
using Raven.Client.Document;

namespace SampleNancyProject
{
    public class Bootstrap : DefaultNancyBootstrapper
    {
        private IDocumentStore CreateDocumentStore()
        {
            var documentStore = new DocumentStore { ConnectionStringName = "RAVENDB" };
            documentStore.Initialize();

            return documentStore;
        }
        
        protected override void ConfigureApplicationContainer(TinyIoC.TinyIoCContainer container)
        {
            var documentStore = CreateDocumentStore();
            container.Register(documentStore);
            container.Register<IRavenRepository<UserModel>>(new RavenRepository<UserModel>(documentStore));
        }

        //Example code to override the default nancy directories for content. The default location
        //  for all your css, images, etc is inside the Content folder.
        /*protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("css", @"Content\css")
            );

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("img", @"Content\img")
            );

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("scripts", @"Content\scripts")
            );
        }*/
    }
}