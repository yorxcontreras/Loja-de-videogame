using System.ComponentModel.DataAnnotations;

namespace LojaGames.Models
{
    public class ItemPedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        [Required]
        public int JogoId { get; set; }
        public Jogo Jogo { get; set; }

        [Required]
        public int Quantidade { get; set; }
    }
}