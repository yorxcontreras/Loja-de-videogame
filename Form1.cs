using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using LojaGames.Data;
using LojaGames.Models;

namespace LojaGamesDesktop
{
    public partial class Form1 : Form
    {
        private DataGridView dgvJogos;
        private TextBox txtTitulo;
        private TextBox txtPreco;
        private TextBox txtCategoria;
        private Button btnSalvar;
        private Button btnAtualizar;
        private Button btnDeletar;
        private Button btnLimpar;
        private Label lblTituloApp;
        private Panel panelLateral;
        private int? jogoIdSelecionado = null;

        public Form1()
        {
            ConfigurarJanela();
            ConstruirLayoutEstiloStreamlit();
            CarregarDadosNaTabela();
        }

        private void ConfigurarJanela()
        {
            this.Text = "GameStore Dashboard - Gerenciamento de Jogos";
            this.Size = new Size(950, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250); // Fundo cinza claro super limpo
        }

        private void ConstruirLayoutEstiloStreamlit()
        {
            // Painel Lateral esquerdo (Estilo menu / sidebar em tons de azul escuro)
            panelLateral = new Panel()
            {
                Dock = DockStyle.Left,
                Width = 320,
                BackColor = Color.FromArgb(15, 23, 42) // Azul Meia-Noite moderno
            };
            this.Controls.Add(panelLateral);

            // Título do Painel
            Label lblMenu = new Label()
            {
                Text = "Painel de Controle",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            panelLateral.Controls.Add(lblMenu);

            // Campo: Título do Jogo
            Label lbl1 = CriarLabel("Título do Jogo:", 70);
            txtTitulo = CriarTextBox(95);
            panelLateral.Controls.Add(lbl1);
            panelLateral.Controls.Add(txtTitulo);

            // Campo: Preço
            Label lbl2 = CriarLabel("Preço (R$):", 150);
            txtPreco = CriarTextBox(175);
            panelLateral.Controls.Add(lbl2);
            panelLateral.Controls.Add(txtPreco);

            // Campo: Categoria
            Label lbl3 = CriarLabel("Categoria (ex: Ação, RPG):", 230);
            txtCategoria = CriarTextBox(255);
            panelLateral.Controls.Add(lbl3);
            panelLateral.Controls.Add(txtCategoria);

            // Botão Salvar / Cadastrar (Azul vibrante moderno)
            btnSalvar = CriarBotao("Cadastrar Jogo", 310, Color.FromArgb(37, 99, 235));
            btnSalvar.Click += BtnSalvar_Click;
            panelLateral.Controls.Add(btnSalvar);

            // Botão Atualizar
            btnAtualizar = CriarBotao("Atualizar Selecionado", 360, Color.FromArgb(13, 148, 136));
            btnAtualizar.Click += BtnAtualizar_Click;
            panelLateral.Controls.Add(btnAtualizar);

            // Botão Deletar
            btnDeletar = CriarBotao("Excluir Jogo", 410, Color.FromArgb(225, 29, 72));
            btnDeletar.Click += BtnDeletar_Click;
            panelLateral.Controls.Add(btnDeletar);

            // Botão Limpar Campos
            btnLimpar = CriarBotao("Limpar Campos", 460, Color.FromArgb(71, 85, 105));
            btnLimpar.Click += (s, e) => LimparCampos();
            panelLateral.Controls.Add(btnLimpar);

            // Título Principal à Direita
            lblTituloApp = new Label()
            {
                Text = "Lista de Jogos Cadastrados (Banco SQLite)",
                ForeColor = Color.FromArgb(30, 41, 59),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(350, 25),
                AutoSize = true
            };
            this.Controls.Add(lblTituloApp);

            // Tabela (DataGridView) à direita
            dgvJogos = new DataGridView()
            {
                Location = new Point(350, 70),
                Size = new Size(560, 465),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true
            };
            dgvJogos.CellClick += DgvJogos_CellClick;
            this.Controls.Add(dgvJogos);
        }

        private Label CriarLabel(string texto, int y)
        {
            return new Label()
            {
                Text = texto,
                ForeColor = Color.FromArgb(203, 213, 225),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(20, y),
                AutoSize = true
            };
        }

        private TextBox CriarTextBox(int y)
        {
            return new TextBox()
            {
                Location = new Point(20, y),
                Width = 270,
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private Button CriarBotao(string texto, int y, Color corFundo)
        {
            Button btn = new Button()
            {
                Text = texto,
                Location = new Point(20, y),
                Width = 270,
                Height = 38,
                BackColor = corFundo,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        // --- LÓGICA DO CRUD COM EF CORE ---

        private void CarregarDadosNaTabela()
        {
            using (var context = new AppDbContext())
            {
                var lista = context.Jogos
                    .Include(j => j.Categoria)
                    .Select(j => new {
                        j.Id,
                        j.Titulo,
                        PrecoFormatado = "R$ " + j.Preco.ToString("F2"),
                        Categoria = j.Categoria.Nome
                    })
                    .ToList();

                dgvJogos.DataSource = lista;
            }
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text) || string.IsNullOrWhiteSpace(txtPreco.Text) || string.IsNullOrWhiteSpace(txtCategoria.Text))
            {
                MessageBox.Show("Preencha todos os campos!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPreco.Text, out decimal preco))
            {
                MessageBox.Show("Preço inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var context = new AppDbContext())
            {
                string nomeCat = txtCategoria.Text.Trim();
                var categoria = context.Categorias.FirstOrDefault(c => c.Nome.ToLower() == nomeCat.ToLower());
                
                if (categoria == null)
                {
                    categoria = new Categoria { Nome = nomeCat };
                    context.Categorias.Add(categoria);
                    context.SaveChanges();
                }

                var novoJogo = new Jogo
                {
                    Titulo = txtTitulo.Text.Trim(),
                    Preco = preco,
                    CategoriaId = categoria.Id
                };

                context.Jogos.Add(novoJogo);
                context.SaveChanges();
            }

            MessageBox.Show("Jogo cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CarregarDadosNaTabela();
            LimparCampos();
        }

        private void DgvJogos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow linha = dgvJogos.Rows[e.RowIndex];
                jogoIdSelecionado = Convert.ToInt32(linha.Cells["Id"].Value);
                txtTitulo.Text = linha.Cells["Titulo"].Value.ToString();
                txtPreco.Text = linha.Cells["PrecoFormatado"].Value.ToString().Replace("R$ ", "");
                txtCategoria.Text = linha.Cells["Categoria"].Value.ToString();
            }
        }

        private void BtnAtualizar_Click(object sender, EventArgs e)
        {
            if (jogoIdSelecionado == null)
            {
                MessageBox.Show("Selecione um jogo na tabela para atualizar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                var jogo = context.Jogos.Find(jogoIdSelecionado);
                if (jogo != null)
                {
                    jogo.Titulo = txtTitulo.Text.Trim();
                    jogo.Preco = decimal.Parse(txtPreco.Text);
                    
                    context.SaveChanges();
                    MessageBox.Show("Registro atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            CarregarDadosNaTabela();
            LimparCampos();
        }

        private void BtnDeletar_Click(object sender, EventArgs e)
        {
            if (jogoIdSelecionado == null)
            {
                MessageBox.Show("Selecione um jogo na tabela para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Deseja realmente excluir este jogo?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (var context = new AppDbContext())
                {
                    var jogo = context.Jogos.Find(jogoIdSelecionado);
                    if (jogo != null)
                    {
                        context.Jogos.Remove(jogo);
                        context.SaveChanges();
                        MessageBox.Show("Jogo removido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                CarregarDadosNaTabela();
                LimparCampos();
            }
        }

        private void LimparCampos()
        {
            txtTitulo.Clear();
            txtPreco.Clear();
            txtCategoria.Clear();
            jogoIdSelecionado = null;
        }
    }
}