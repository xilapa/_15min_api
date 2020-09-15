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
            // informa para a aplica��o que estamos utilizando o DataContext
            // n�o precisa passar o argumento pois o DataContext j� foi configurado em sua pr�pria classe

            services.AddScoped<DataContext, DataContext>();
            // deixa o DataContext dispon�vel atrav�s do AddScoped, que � a inje��o de depend�ncia do ASP.NET Core
            // toda vez que algum lugar na aplica��o pedir um DataContext, ser� criada uma vers�o do DataContext em mem�ria na primeira vez
            // e nas pr�ximas vezes ele ir� utilizar o DataContext que j� esta na mem�ria e n�o ir� criar uma nova, ou seja,
            // n�o ser� aberta uma nova conex�o no banco e quando a requisi��o terminar o DataContext ser� removido da mem�ria

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
            // for�ar uso de HTTPS

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
