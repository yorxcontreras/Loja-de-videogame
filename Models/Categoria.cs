using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LojaGames.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [MaxLength(100)]
        public string Nome { get; set; }

        // Relacionamento Um para Muitos (Uma Categoria possui Vários Jogos)
        public ICollection<Jogo> Jogos { get; set; } = new List<Jogo>();
    }
}