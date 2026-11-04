using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LojaGames.Data;
using LojaGames.Models;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new AppDbContext())
        {
            // Garante que o banco de dados e as tabelas sejam criados
            context.Database.EnsureCreated();
        }

        bool sair = false;
        while (!sair)
        {
            Console.Clear();
            Console.WriteLine("=== SISTEMA DE LOJA DE VIDEOGAMES (CRUD + EF Core) ===");
            Console.WriteLine("1. Cadastrar Categoria e Jogo (CREATE)");
            Console.WriteLine("2. Listar Jogos e Categorias (READ)");
            Console.WriteLine("3. Atualizar Preço de um Jogo (UPDATE)");
            Console.WriteLine("4. Deletar um Jogo (DELETE)");
            Console.WriteLine("5. Sair");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine();
            Console.Clear();

            switch (opcao)
            {
                case "1":
                    CriarRegistro();
                    break;
                case "2":
                    ListarRegistros();
                    break;
                case "3":
                    AtualizarRegistro();
                    break;
                case "4":
                    DeletarRegistro();
                    break;
                case "5":
                    sair = true;
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }

            if (!sair)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private static void CriarRegistro()
    {
        using (var context = new AppDbContext())
        {
            Console.WriteLine("--- CADASTRAR NOVO JOGO ---");
            Console.Write("Nome da Categoria (ex: Ação, RPG): ");
            string nomeCat = Console.ReadLine();

            // Verifica se a categoria já existe, senão cria uma nova
            var categoria = context.Categorias.FirstOrDefault(c => c.Nome.ToLower() == nomeCat.ToLower());
            if (categoria == null)
            {
                categoria = new Categoria { Nome = nomeCat };
                context.Categorias.Add(categoria);
                context.SaveChanges();
            }

            Console.Write("Título do Jogo: ");
            string titulo = Console.ReadLine();

            Console.Write("Preço do Jogo: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal preco))
            {
                var jogo = new Jogo
                {
                    Titulo = titulo,
                    Preco = preco,
                    CategoriaId = categoria.Id
                };

                context.Jogos.Add(jogo);
                context.SaveChanges();
                Console.WriteLine("\n[Sucesso] Jogo cadastrado e armazenado no SQLite via EF Core!");
            }
            else
            {
                Console.WriteLine("\n[Erro] Preço inválido.");
            }
        }
    }

    private static void ListarRegistros()
    {
        using (var context = new AppDbContext())
        {
            Console.WriteLine("--- LISTA DE JOGOS CADASTRADOS ---");
            var jogos = context.Jogos.Include(j => j.Categoria).ToList();

            if (!jogos.Any())
            {
                Console.WriteLine("Nenhum jogo cadastrado.");
                return;
            }

            foreach (var j in jogos)
            {
                Console.WriteLine($"ID: {j.Id} | Título: {j.Titulo} | Preço: R$ {j.Preco:F2} | Categoria: {j.Categoria.Nome}");
            }
        }
    }

    private static void AtualizarRegistro()
    {
        using (var context = new AppDbContext())
        {
            ListarRegistros();
            Console.Write("\nDigite o ID do jogo que deseja atualizar o preço: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var jogo = context.Jogos.Find(id);
                if (jogo != null)
                {
                    Console.Write($"Novo preço para '{jogo.Titulo}': ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal novoPreco))
                    {
                        jogo.Preco = novoPreco;
                        context.SaveChanges();
                        Console.WriteLine("\n[Sucesso] Preço atualizado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("\n[Erro] Valor inválido.");
                    }
                }
                else
                {
                    Console.WriteLine("\n[Erro] Jogo não encontrado.");
                }
            }
        }
    }

    private static void DeletarRegistro()
    {
        using (var context = new AppDbContext())
        {
            ListarRegistros();
            Console.Write("\nDigite o ID do jogo que deseja remover: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var jogo = context.Jogos.Find(id);
                if (jogo != null)
                {
                    context.Jogos.Remove(jogo);
                    context.SaveChanges();
                    Console.WriteLine("\n[Sucesso] Jogo removido do banco de dados!");
                }
                else
                {
                    Console.WriteLine("\n[Erro] Jogo não encontrado.");
                }
            }
        }
    }
}