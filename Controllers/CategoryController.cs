using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using _15min_api.Models;
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
        // retorna todas as categorias
        public async Task<ActionResult<List<Category>>> GetAll([FromServices] DataContext context)
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

        [HttpGet]
        [Route("{id:int}")]
        // retorna uma categoria em específico
        // foi feita a restrição para o id ser passada como um int
        public async Task<ActionResult<Category>> GetById([FromServices] DataContext context, int? id)
        {
            var category = await context.Categories.FindAsync(id);
            // Find async busca a entidade com o chave primária passada
            if (id == null || category == null)
            {
                return new NotFoundResult();
                // retorna a resposta 404 not found
            }
            else
            {
                return new OkObjectResult(category);
            }
        }



        [HttpPost]
        [Route("")]
        // utilizando a conveção REST, os verbos, GET,POST,PUT e DELETE tem a mesma rota

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


        [HttpPut]
        [Route("{id:int}")]
        // A diferença entre PUT e POST é que PUT é idempotente: chamá-lo uma ou várias vezes sucessivamente terá o mesmo efeito (não é um efeito colateral),
        //enquanto usar POST repetidamente pode ter efeitos adicionais, como passar uma ordem várias vezes.
        public async Task<ActionResult> Put([FromServices] DataContext context, int? id, [FromBody] Category category)
        {
            if (id == null )
            {
                return new NotFoundResult();
            }
            else if (id != category.Id)
            // checa se o id é o mesmo do corpo da mensagem
            {
                return BadRequest();
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                context.Entry(category).State = EntityState.Modified;
                try
                {
                    await context.SaveChangesAsync();
                    // tenta salvar
                }
                catch (DbUpdateConcurrencyException)
                // verifica se o banco de dados não foi alterado desde que a entidade foi carregada
                {
                    if(!context.Categories.Any(x => x.Id == id))
                    // verifica se a categoria com o id informado ainda existe e retorna um booleano
                    {
                        return new NotFoundResult();
                    }
                    else
                    {
                        throw;
                    }
                    
                        
                }

                return new NoContentResult();
                // caso não haja erros, nada é retornado
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete([FromServices] DataContext context, int? id)
        {
            if (id == null )
            {
                return new NotFoundResult();
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else 
            {
                var category = await context.Categories.FindAsync(id);
                if (category == null)
                // a verificação se a categoria existe pode ser feita assim como no Put com utilização do Any
                // porém será nescessário buscar e passar o objeto category para ser deletado
                {
                    return new NotFoundResult();
                }
                else
                {
                    context.Categories.Remove(category);
                    await context.SaveChangesAsync();
                    return new OkObjectResult(category);
                }

            }
        }


    }
}
