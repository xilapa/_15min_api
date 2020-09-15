using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _15min_api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace _15min_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<DataContext>();
            // informa para a aplicação que estamos utilizando o DataContext
            // não precisa passar o argumento pois o DataContext já foi configurado em sua própria classe

            services.AddScoped<DataContext, DataContext>();
            // deixa o DataContext disponível através do AddScoped, que é a injeção de dependência do ASP.NET Core
            // toda vez que algum lugar na aplicação pedir um DataContext, será criada uma versão do DataContext em memória na primeira vez
            // e nas próximas vezes ele irá utilizar o DataContext que já esta na memória e não irá criar uma nova, ou seja,
            // não será aberta uma nova conexão no banco e quando a requisição terminar o DataContext será removido da memória

            //https://balta.io/blog/aspnet-core-dependency-injection


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            // forçar uso de HTTPS

            app.UseRouting();
            // utilizar esquema de rotas no Controller

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
