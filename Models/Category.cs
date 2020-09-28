using System.ComponentModel.DataAnnotations;

namespace _15min_api.Models
{
    public class Category
    {
        /// <summary>
        /// Id da categoria, chave primária
        /// </summary>
        /// <example>1</example>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome da categoria
        /// </summary>
        /// <example>Limpeza</example>
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]
        public string Name { get; set; }
    }
}
