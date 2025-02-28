namespace CollegeApp.Data
{
    public class User
    {
        public int Id { get; set; }
        public int Username { get; set; }
        public string Password { get; set; }
        public int PasswordSalt { get; set; }
        public int Usertype { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set;}

        public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }

    }
}
