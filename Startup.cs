using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Microsoft.OpenApi.Models;

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

            // gerador do arquivo de especifica��o da API do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                // a classe OpenApiInfo requer o namespace Microsoft.OpenApi.Models
                {
                    Title = "Cadastro Produtos",
                    Version = "v1",
                    Description = "Cadastro de produtos e suas respectivas categorias"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                // esse trecho indica que o Swagger deve ler os coment�rios do c�digo a partir de um arquivo XML
                // a documenta��o � gerada pela IDE e deve ser habilitada no no arquivo .csproj


            });


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

            app.UseSwagger();
            // ativa o middleware do Swagger para expor os endpoints da API como um JSON

            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "API Produtos");
                // Nesta parte � especificado o arquivo JSON criado com os endpoints da API
                opt.RoutePrefix = string.Empty;
                // define a rota para acessar a UI do swagger
            });
            // SwaggerUI para mostrar a documenta��o interativa
            
        }
    }
}
