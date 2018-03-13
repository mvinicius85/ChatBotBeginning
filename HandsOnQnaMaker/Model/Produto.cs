using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandsOnQnaMaker.Model
{

    public class Produto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Preco")]
        public double Preco { get; set; }
        [JsonProperty("Nome")]
        public string Nome { get; set; }
        [JsonProperty("Quantidade")]
        public int Quantidade { get; set; }
    }

}