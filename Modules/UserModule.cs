using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using SampleNancyProject.Model;
using SampleNancyProject.Persistence;

namespace SampleNancyProject.Modules
{
    public class UserModule : NancyModule
    {
        private readonly IRavenRepository<UserModel> _repository;

        public UserModule(IRavenRepository<UserModel> repository) : base("api")
        {
            _repository = repository;
            Get["/user/{id}"] = x => Response.AsJson(_repository.Load((string)x.id));
            Get["/users"] = x => Response.AsJson(_repository.LoadAll());
            Post["/user"] = x => CreateUser();
            Put["/user/{id}"] = x => UpdateUser(x.id);
            Delete["/user/{id}"] = x => DeleteUser(x.id);

            Nancy.Json.JsonSettings.MaxJsonLength = 1000000;
        }

        private Response DeleteUser(string id)
        {
            var user = _repository.Load(id);

            if (user == null)
                return new Response { StatusCode = HttpStatusCode.NotFound };
            
            _repository.Delete(id);

            return new Response { StatusCode = HttpStatusCode.OK };
        }

        private Response CreateUser()
        {
            //This Nancy extension can automatically take form POST data, and bind it to
            //  a model. This will map the "name" attribute of a form element to a property
            //  of the model. The alternative is to just create the user manually. You can
            //  look at the UpdateUser() function as an example of doing it manually.
            var newUser = this.Bind<UserModel>();

            //Set the avatar
            newUser.Avatar = GetAvatar(Request.Files);

            //Save the user to database
            _repository.Store(newUser);

            //Redirect to list page after creating
            return Response.AsRedirect("/users");
        }

        private string GetAvatar(IEnumerable<HttpFile> files)
        {
            if (files == null || !files.Any()) return null;

            //We are only posting one file, so we will just get the first item
            var file = files.First();

            //Read the data
            byte[] buffer = new byte[file.Value.Length];
            file.Value.Read(buffer, 0, (int)file.Value.Length);

            return Convert.ToBase64String(buffer);
        }

        private Response UpdateUser(string id)
        {
            var user = _repository.Load(id); //Load the user for update

            //Manually setting each of the fields. The easier way to do this is by using
            //  the .Bind<> extension method provided by Nancy. See CreateUser() for an
            //  example.
            user.UserName = Request.Form["UserName"];
            user.Password = Request.Form["Password"];
            user.FirstName = Request.Form["FirstName"];
            user.LastName = Request.Form["LastName"];
            user.Address = Request.Form["Address"];

            //Set the avatar, only if it's uploaded. Or else it'll clear out any existing avatar
            if (Request.Files.Any())
                user.Avatar = GetAvatar(Request.Files);

            //We're doing a store, but because Id has been set, it'll do an update instead
            _repository.Store(user);

            //Return status OK
            return Response.AsJson(user);
        }
    }
}