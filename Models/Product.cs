using System;
using System.ComponentModel.DataAnnotations;

namespace _15min_api.Models
{
    public class Product
    {
        /// <summary>
        /// ID do produto, chave primária
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        /// <example>Sabão</example>
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]
        public string Name { get; set; }

        /// <summary>
        /// Quantidade do produto
        /// </summary>
        /// <example>12</example>
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public int Quantity { get; set; }

        /// <summary>
        /// Preço do produto
        /// </summary>
        /// <example>8.50</example>
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Price { get; set; }

        /// <summary>
        /// ID da Categoria
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int CategoryId { get; set; }
        // referencia a categoria no banco de dados

        /// <summary>
        /// Categoria, propriedade de navegação
        /// Não precisa ser passada
        /// </summary>
        public Category Category { get; set; }
        // propriedade de navegação, utilizada para poder acessar informações da categoria dentro do produto
    }
}
