using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LojaGames.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DataPedido { get; set; } = DateTime.Now;

        // Chave Estrangeira para Cliente
        [Required]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // Relacionamento Muitos para Muitos (Via ItemPedido)
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}