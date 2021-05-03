using System.Threading.Tasks;
using CodingSessionFT.Core.DTO;

namespace CodingSessionFT.Api.Repositories
{
  public interface IEmployeesRepository
  {
    Task<EmployeeModel> GetAsync(int id);
  }
}
