using Newtonsoft.Json;

namespace BuscaCEP
{
    public class ViaCepResponse
    {

        [JsonProperty("logradouro")]
        public string Rua { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("localidade")]
        public string Cidade { get; set; }

        [JsonProperty("uf")]
        public string Estado { get; set; }

        [JsonProperty("ibge")]
        public string Ibge { get; set; }
    }
}