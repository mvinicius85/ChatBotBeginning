using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandsOnQnaMaker.Model
{
    public class ListaRacao
    {
        public List<Racao> lista { get; set; }
        public int count { get; set; }
    }
    public class Racao
    {
        [JsonProperty("Id")]
        public long Id { get; set; }
        [JsonProperty("Preco")]
        public double Preco { get; set; }
        [JsonProperty("Nome")]
        public string Nome { get; set; }
        [JsonProperty("Unidade")]
        public string Unidade { get; set; }
        [JsonProperty("Animal")]
        public TipoAnimal Animal { get; set; }
        [JsonProperty("Porte")]
        public PorteAnimal Porte { get; set; }
    }

    public class TipoAnimal
    {
        public TipoAnimal()
        {

        }

        public TipoAnimal(int _id, string _desc)
        {
            Id = _id;
            Descricao = _desc;
        }
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Descricao")]
        public string Descricao { get; set; }
    }

    public class PorteAnimal
    {
        public PorteAnimal()
        {

        }

        public PorteAnimal(int _id, string _desc)
        {
            Id = _id;
            Descricao = _desc;
        }
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Descricao")]
        public String Descricao { get; set; }
    }
}