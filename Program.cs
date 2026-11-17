using System;
using System.Windows.Forms;
using LojaGames.Data;

namespace LojaGamesDesktop
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Garante que o banco de dados SQLite e as tabelas sejam criados automaticamente ao iniciar
            using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();
            }

            // Inicializa os estilos visuais do Windows Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Inicia a aplicação abrindo a nossa janela moderna (Form1)
            Application.Run(new Form1());
        }
    }
}