namespace CollegeApp.Models
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; }= new List<Student> // Fixed List<Student> initialization
            {
                new Student
                {
                    Id = 1,
                    StudentName = "Murad",
                    Email = "Murad@gmail.com",
                    Address = "Baku, Azerbaijan"
                },
                new Student
                {
                    Id = 2, // Fixed duplicate Id (changed from 1 to 2)
                    StudentName = "Baylar",
                    Email = "Baylar@gmail.com",
                    Address = "Beyleqan,Azerbaijan" // Fixed spelling of "Bangalore"
                }
            };

    }
}
