using Microsoft.EntityFrameworkCore;

namespace _15min_api.Models
{
    public class DataContext : DbContext
    // tem que instalar o pacote do entity framework para o DbContext ser herdado
    // o DbContext é a representação do banco de dados em memória
    // neste caso foi usado o Entity Framework Core In Memory 
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        // chama o construtor da classe pai e passa esse objeto options para o mesmo
        // base é utilizado para referenciar o paramentro DbContextOptions da superClasse DbContext
        {
            // caso fosse utilizado outro BD, a connection string ficaria aqui
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        // este método é chamado pelo construtor da classe para configurar o banco de dados
        {
            options.UseInMemoryDatabase("Database");
            // informa que esta sendo utilizado o banco de dados em memória e define o nome do mesmo
        }



        //define as coleções, tabelas do banco que serão utilizadas
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
