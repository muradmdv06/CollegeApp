using CollegeApp.Data.Repo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeApp.Data.Repository
{
    public interface IStudentRepository : ICollegeRepository<Student>
    {
        Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus);


    }
}
