using System.Collections.Generic;
using SampleNancyProject.Model;
using Raven.Client.Linq;

namespace SampleNancyProject.Persistence
{
    public interface IRavenRepository<T> where T : IModel
    {
        IRavenQueryable<T> Query();
        T Load(string id);
        List<T> LoadAll();
        void Store(T resource);
        void Delete(T resource);
        void Delete(string id);
    }
}