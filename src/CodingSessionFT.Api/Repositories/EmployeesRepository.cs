using System.Threading.Tasks;
using CodingSessionFT.Core.DTO;

namespace CodingSessionFT.Api.Repositories
{
  public sealed class EmployeesRepository : IEmployeesRepository
  {
    public Task<EmployeeModel> GetAsync(int id)
    {
      return Task.FromResult(new EmployeeModel
      {
        Id = id,
        Age = 34,
        Name = "John Doe",
      });
    }
  }
}
