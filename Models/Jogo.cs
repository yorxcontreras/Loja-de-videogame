using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaGames.Models
{
    public class Jogo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título do jogo é obrigatório.")]
        [MaxLength(150)]
        public string Titulo { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        // Chave Estrangeira para Categoria
        [Required]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        // Relacionamento Muitos para Muitos (Via ItemPedido)
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}