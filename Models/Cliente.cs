using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LojaGames.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [MaxLength(150)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [MaxLength(150)]
        public string Email { get; set; }

        // Relacionamento Um para Muitos (Um Cliente possui Vários Pedidos)
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}