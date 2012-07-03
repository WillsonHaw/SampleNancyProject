﻿namespace SampleNancyProject.Model
{
    public class UserModel : IModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
    }
}