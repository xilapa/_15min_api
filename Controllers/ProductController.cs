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
    [Produces("application/json")]
    [Route("products")]
    public class ProductController : ControllerBase
    {   
        /// <summary>
        /// Retorna a lista dos produtos
        /// </summary>
        /// <returns>Lista de produtos</returns>
        /// <response code="200">Lista de produtos</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Product>>> GetAll([FromServices] DataContext context)
        {
            var products = await context.Products.Include(x => x.Category).ToListAsync();
            // include: O carregamento adiantado é o processo pelo qual uma consulta para um tipo de entidade também carrega entidades relacionadas como parte da consulta.
            // https://docs.microsoft.com/pt-br/ef/ef6/querying/related-data
            return new OkObjectResult(products);
        }

        /// <summary>
        /// Retorna um produto pelo ID
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <returns>Produto</returns>
        /// <response code="200">Produto</response>
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Retorna os produtos de uma dada categoria
        /// </summary>
        /// <param name="id">ID da categoria</param>
        /// <returns>Produtos de uma categoria</returns>
        /// <response code="200">Produtos de uma categoria</response>
        [HttpGet]
        [Route("categories/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Adiciona um produto
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// POST /products
        /// 
        ///     {
        ///         "Name": "esponja",
        ///         "Quantity" : 10,
        ///         "Price" : 2.5,
        ///         "CategoryId" : 1
        ///     }
        /// </remarks>
        /// <param name="product">Produto</param>
        /// <returns>Produto adicionado</returns>
        /// <response code="200">Retorna o produto adicionado</response>
        /// <response code="400">O corpo da mensagem contém erros</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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


        /// <summary>
        /// Atualiza um produto
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// PUT /products
        /// 
        ///     {
        ///         "Id": 1,
        ///         "Name": "esponja de aço",
        ///         "Quantity" : 22,
        ///         "Price" : 7.5,
        ///         "CategoryId" : 2
        ///     }
        /// </remarks>
        /// <param name="product">Produto</param>
        /// <param name="id">ID do produto</param>
        /// <response code="204">O produto é atualizado e nada é retornado</response>
        /// <response code="400">O ID passado é nulo, ou o ID do produto na URI é diferente do ID no corpo da mensagem, ou o corpo da mensagem está com erros</response>
        /// <response code="404">A categoria não existe</response>
        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deleta uma categoria
        /// </summary>
        /// <returns>Produto removido</returns>
        /// <param name="id">ID do produto</param>
        /// <response code="200">Retorna o produto deletado</response>
        /// <response code="404">O id passado é nulo ou o produto não existe</response>
        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
