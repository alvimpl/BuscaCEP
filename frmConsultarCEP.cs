using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuscaCEP
{
    public partial class frmConsultarCEP : Form
    {
        public frmConsultarCEP()
        {
            InitializeComponent();
            txtCEP.Focus();
        }
        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            ConsultarCEPAsync();
        }
        private void btnConsultar_Enter(object sender, EventArgs e)
        {
            ConsultarCEPAsync();
        }

        //private void btnConsultar_Click(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(txtCEP.Text))
        //    {
        //        using (var ws = new WSCorreios.AtendeClienteClient())  //AtendeClienteClient())
        //        {
        //            try
        //            {
        //                string usuario = "01366498666";
        //                string senha = "PLzYef9RyzD2s8HuinocYq0rLTBPtK7TNJ11ccAB";
        //                var PesquisaCEP = ws.consultaCEP(txtCEP.Text.Trim(), usuario, senha);
        //                //string cep, string usuario, string senha token PLzYef9RyzD2s8HuinocYq0rLTBPtK7TNJ11ccAB



        //                txtEstado.Text = PesquisaCEP.uf;
        //                txtCidade.Text = PesquisaCEP.cidade;
        //                txtBairro.Text = PesquisaCEP.bairro;
        //                txtLogradouro.Text = PesquisaCEP.end;
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }

        //        }
        //    } else
        //    {
        //        MessageBox.Show("Informe um CEP válido!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        public async Task ConsultarCEPAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Pega o CEP que o usuário digitou
                    string cep = txtCEP.Text;

                    // Monta o endereço da "loja" do ViaCEP
                    string url = $"https://viacep.com.br/ws/{cep.Replace(" ", "").Replace("-", "").Replace(".", "").Replace(",", "")}/json/";

                    // A palavra "await" pausa o método aqui, mas NÃO TRAVA o seu programa.
                    // O programa fica livre para fazer outras coisas enquanto espera a resposta da internet.
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Verifica se a viagem foi um sucesso
                    if (response.IsSuccessStatusCode)
                    {
                        // Lê a "caixa" que o ViaCEP nos devolveu (o JSON)
                        string json = await response.Content.ReadAsStringAsync();

                        ViaCepResponse endereco = JsonConvert.DeserializeObject<ViaCepResponse>(json);

                        // Verifica se o ViaCEP retornou um CEP válido (ele pode retornar
                        // sucesso mas com um JSON indicando erro se o CEP não existir)
                        if (endereco.Rua != null)
                        {
                            txtLogradouro.Text = endereco.Rua;
                            txtBairro.Text = endereco.Bairro;
                            txtCidade.Text = endereco.Cidade;
                            txtEstado.Text = endereco.Estado;
                        }
                        else
                        {
                            MessageBox.Show("CEP não encontrado. Verifique se digitou corretamente.");
                        }
                    }
                    else
                    {
                        // Avisa o usuário se algo deu errado na comunicação
                        MessageBox.Show("Não foi possível encontrar o CEP digitado: " + cep);
                    }
                }
                catch (Exception ex)
                {
                    // Captura qualquer outro erro (como falta de internet)
                    MessageBox.Show("Ocorreu um erro: " + ex.Message);
                }
            }
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtCEP.Clear();
            txtEstado.Clear();
            txtCidade.Clear();
            txtBairro.Clear();
            txtLogradouro.Clear();
        }
    }
}
