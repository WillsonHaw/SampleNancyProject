using System.Collections.Generic;
using System.Linq;
using SampleNancyProject.Model;
using Raven.Abstractions.Commands;
using Raven.Client;
using Raven.Client.Linq;

namespace SampleNancyProject.Persistence
{
    public class RavenRepository<T> : IRavenRepository<T> where T : IModel
    {
        private IDocumentStore _documentStore;

        public RavenRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public IRavenQueryable<T> Query()
        {
            IRavenQueryable<T> query;

            using (var session = _documentStore.OpenSession())
            {
                query = session.Query<T>();
            }

            return query;
        }

        public T Load(string id)
        {
            T result;

            using (var session = _documentStore.OpenSession())
            {
                result = session.Load<T>(id);
            }

            return result;
        }

        public List<T> LoadAll()
        {
            List<T> results;

            using (var session = _documentStore.OpenSession())
            {
                results = session.Query<T>().ToList();
            }

            return results;
        }

        public void Store(T resource)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(resource);
                session.SaveChanges();
            }
        }

        public void Delete(T resource)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Delete(resource);
                session.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Advanced.Defer(new DeleteCommandData { Key = id });
                session.SaveChanges();
            }
        }
    }
}