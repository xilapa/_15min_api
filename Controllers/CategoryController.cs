using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _15min_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15min_api.Controllers
{
    [ApiController]
    [Route("categories")]
    // como não foi definida nenhuma rota no Startup é bom utilizar o Route pois a API se irá se basear nas rotas do Controller
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        // se não colocar nada, sempre será GET
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        // retorna um task de forma assíncrona, que terá um ActionResult com uma lista de categorias
        // ActionResult já da respostas compativeis com o protocolo REST
        // o "From Services" indica que o DataContext da memória sera utilizado
        // DataContext pois dados serão manipulados
        {
            var categories = await context.Categories.ToListAsync();
            // é utilizado await por estar dentro de um método assíncrono
            // é buscada as categorias do BD através do DataContext que faz a utilização do Model

            return new OkObjectResult(categories);

        }

        [HttpPost]
        [Route("")]
        // utilizando a conveção REST, os verbos, GET,POST,UPDATE e DELETE tem a mesma rota
            
        public async Task<ActionResult<Category>> Post([FromServices] DataContext context, [FromBody] Category model)
        // desta vez é passado uma catergoria pelo ActionResult
        // do serviço é recebido o DataContext para ser injetado
        // e do corpo da requisição é recebida a categoria
        {
            if (ModelState.IsValid)
            // valida se a categoria foi passada corretamente no corpo da requisição
            // ela deve atender as restrições passadas no modelo
            {
                context.Categories.Add(model);
                // o DataContext é a representação do BD em memória, nada foi persistido
                await context.SaveChangesAsync();
                // o SaveChangesAsync é utilizado para gravar as informações no banco
                return new OkObjectResult(model);
            }
            else
            {
                return BadRequest(ModelState);
                // o método BadRequest já espera o ModelState
            }
        }


    }
}
