using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _15min_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _15min_api.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> GetAll([FromServices] DataContext context)
        {
            var products = await context.Products.Include(x => x.Category).ToListAsync();
            // include: O carregamento adiantado é o processo pelo qual uma consulta para um tipo de entidade também carrega entidades relacionadas como parte da consulta.
            // https://docs.microsoft.com/pt-br/ef/ef6/querying/related-data
            return new OkObjectResult(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int? id)
        {
            var product = await context.Products.Include(x => x.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            // include para incluir a entidade categoria relacionada ao produto
            // x.Id == id para buscar o produto
            // toda vez que pegamos um objeto do banco o EF cria uma versão desse objeto e a cada alteração ele cria uma nova versão, essas versões são chamadas proxies sõa utilizadas para ele se orientar e fazer o crud no banco
            // se o objetivo for apenas leitura dos dados para a tela, o AsNoTracking() possibilita ganhos de performance
            if (id == null || product == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(product);
            }

        }

        [HttpGet]
        [Route("categories/{id:int}")]
        // listar produtos por categoria
        public async Task<ActionResult<List<Product>>> GetAllByCategory([FromServices] DataContext context, int? id)
        {
            var products = await context.Products.Include(x => x.Category).AsNoTracking().Where(x => x.CategoryId == id).ToListAsync();
            // to list sempre no fim
            if (id == null | products == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(products);
            }

        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
                return new OkObjectResult(product);
            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }


        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> Put([FromServices] DataContext context, [FromBody] Product product, int? id)
        {
            if (id == null)
            {
                return new NotFoundResult();
            }
            else if (id != product.Id)
            {
                return BadRequest();
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!context.Products.Any(x => x.Id == id))
                    {
                        return new NotFoundResult();
                    }
                    else
                    {
                        throw;
                    }
                }
                return new NoContentResult();
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete([FromServices] DataContext context, int? id)
        {
            if (id == null)
            {
                return new NotFoundResult();
            }
            else
            {
                var product = await context.Products.FindAsync(id);
                if (product == null)
                {
                    return new NotFoundResult();
                }
                else
                {
                    context.Products.Remove(product);
                    await context.SaveChangesAsync();
                    return new OkObjectResult(product);
                }
            }
        }
    }
}
