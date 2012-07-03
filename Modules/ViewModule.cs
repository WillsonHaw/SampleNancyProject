using Nancy;
using SampleNancyProject.Model;
using SampleNancyProject.Persistence;

namespace SampleNancyProject.Modules
{
    public class ViewModule : NancyModule
    {
        private readonly IRavenRepository<UserModel> _repository;

        public ViewModule(IRavenRepository<UserModel> repository)
        {
            _repository = repository;
            Get["/"] = x => View["index.html"];
            Get["/users"] = x => View["users.cshtml", new { Users = _repository.LoadAll() }];
        }
    }
}