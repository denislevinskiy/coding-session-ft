using System.Threading.Tasks;
using CodingSessionFT.Api.Repositories;
using CodingSessionFT.Core.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CodingSessionFT.Api.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesRepository _employeesRepository;

        public EmployeesController(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeModel>> GetAsync(
            int id, 
            [FromQuery] int error)
        {
            if (error != 0)
            {
                await Task.Delay(500);
                return await Task.FromResult(Problem(statusCode: 503));
            }

            await Task.Delay(500);

            return Ok(await _employeesRepository.GetAsync(id));
        }
    }
}