namespace JogoDasPalavras.WinFormsApp
{
    public partial class Form1 : Form
    {

        private JogosPalavras jogoPalavras;

        public Form1()
        {
            InitializeComponent();
            ConfigurarBotoes();
            StartarJogo();
        }
        private void ConfigurarBotoes()
        {
            btnEnter.Click += Ok_Click;
            lblNovoJogo.Click += NovoJogo;

            foreach (Button botao in pnlTeclas.Controls)
            {
                if (botao.Text != "OK")
                {
                    botao.Click += SelecionarLetraPorBotao;
                }
            }
        }
        private void lblNovoJogo_Click(object sender, EventArgs e)
        {
            StartarJogo();
        }
        private void Ok_Click(object sender, EventArgs e)
        {
            ConfirmarValidacaoPalavra();
        }
        private void StartarJogo()
        {
            jogoPalavras = new JogosPalavras();
            lblNovoJogo.Visible = true;

            pnlTeclas.Enabled = true;

            RecomecarPaineis();

            InicializarRodada();
        }
        private void SelecionarLetraPorBotao(object? sender, EventArgs e)
        {
            Button botaoClicado = (Button)sender;
            PassarLetraBotaoParaTextBox(Convert.ToChar(botaoClicado.Text[0]));
        }
        private void ReceberPalavraInteira()
        {
            jogoPalavras.palavraDigitada = "";

            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtLetra) == jogoPalavras.rodada)
                {
                    jogoPalavras.palavraDigitada += txtLetra.Text;
                }
            }
        }
        private void PassarLetraBotaoParaTextBox(char letraTeclado)
        {
            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (txtLetra is TextBox && pnlMostrarLetra.GetRow(txtLetra) == jogoPalavras.rodada && txtLetra.Text == "")
                {
                    txtLetra.Text = letraTeclado.ToString();
                    break;
                }
            }
        }
        private void ConfirmarValidacaoPalavra()
        {
            ReceberPalavraInteira();

            if (jogoPalavras.VerificaPalavraCompleta())

                TerminarRodada();
        }
        private void TerminarRodada()
        {
            VerificarLetrasNaoExistentesNaPalavra();

            VerificarLetraExistenteNaPalavra();

            VerificarPosicaoDaLetraNaPalavra();

            if (jogoPalavras.VerificaSeJogadorGanhou())
                VencerPartida();

            else if (jogoPalavras.VerificaSeJogadorPerdeu())
                PerderPartida();

            jogoPalavras.rodada++;

            InicializarRodada();
        }
        private void VerificarLetrasNaoExistentesNaPalavra()
        {

            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtLetra) != jogoPalavras.rodada)
                    continue;

                txtLetra.BackColor = Color.Red;

                foreach (Control btnTeclado in pnlTeclas.Controls)
                {
                    if (btnTeclado.Text == "OK")
                        continue;

                    if (jogoPalavras.CompararLetrasIguais(Convert.ToChar(btnTeclado.Text[0]), Convert.ToChar(txtLetra.Text[0])))
                        btnTeclado.BackColor = Color.Red;
                }
            }
        }
        private void VerificarLetraExistenteNaPalavra()
        {
            foreach (char letraPalavraSecreta in jogoPalavras.palavraSecreta)
            {
                foreach (Control txtLetra in pnlMostrarLetra.Controls)
                {
                    if (pnlMostrarLetra.GetRow(txtLetra) != jogoPalavras.rodada)
                        continue;

                    if (jogoPalavras.CompararLetrasIguais(Convert.ToChar(txtLetra.Text), letraPalavraSecreta))
                    {
                        txtLetra.BackColor = Color.Yellow;

                        foreach (Control btnTeclado in pnlTeclas.Controls)
                        {
                            if (btnTeclado.Text == txtLetra.Text && btnTeclado.BackColor != Color.Green)
                            {
                                btnTeclado.BackColor = Color.Yellow;
                            }
                        }
                    }
                }
            }
        }
        private void VerificarPosicaoDaLetraNaPalavra()
        {
            List<Control> txtListaLetras = new List<Control>();

            foreach (Control txtBox in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtBox) == jogoPalavras.rodada)
                    txtListaLetras.Add(txtBox);
            }

            for (int letra = 0; letra < jogoPalavras.palavraSecreta.Length && letra < txtListaLetras.Count; letra++)
            {
                char letraNoTxt = Convert.ToChar(txtListaLetras[letra].Text);
                char letraSecreta = jogoPalavras.palavraSecreta[letra];

                if (jogoPalavras.CompararLetrasIguais(letraNoTxt, letraSecreta))
                {
                    txtListaLetras[letra].BackColor = Color.Green;
                    txtListaLetras[letra].Text = letraSecreta.ToString();

                    foreach (Control btnTeclado in pnlTeclas.Controls)
                    {
                        if (btnTeclado.Text == txtListaLetras[letra].Text)
                            btnTeclado.BackColor = Color.Green;
                    }
                }
            }
        }
        private void InicializarRodada()
        {
            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtLetra) == jogoPalavras.rodada)
                {
                    txtLetra.BackColor = Color.AliceBlue;
                }
            }
        }
        private void VencerPartida()
        {
            MessageBox.Show("Voce venceu!");

            lblNovoJogo.Visible = true;
            pnlTeclas.Enabled = false;
        }
        private void PerderPartida()
        {
            MessageBox.Show("Você perdeu!");

            lblNovoJogo.Visible = true;
            pnlTeclas.Enabled = false;
        }
        private void RecomecarPaineis()
        {
            foreach (Control txtTabelaLetra in pnlMostrarLetra.Controls)
            {
                txtTabelaLetra.Text = "";
                txtTabelaLetra.BackColor = Color.Black;
            }

            foreach (Control btnTeclado in pnlTeclas.Controls)
            {
                btnTeclado.BackColor = Color.Transparent;
            }
        }
        private void NovoJogo(object sender, EventArgs e)
        {
            StartarJogo();
        }
    }
}