using System;
using System.ComponentModel.DataAnnotations;

namespace _15min_api.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int CategoryId { get; set; }
        // referencia a categoria no banco de dados
        public Category Category { get; set; }
        // propriedade de navegação, utilizada para poder acessar informações da categoria dentro do produto
    }
}
