﻿namespace RecommendMe.Data.Entities
{
    public class User 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}