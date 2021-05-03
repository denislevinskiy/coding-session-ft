using CodingSessionFT.Api.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodingSessionFT.Api
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<IEmployeesRepository, EmployeesRepository>();
      services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseHttpsRedirection();
      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}
