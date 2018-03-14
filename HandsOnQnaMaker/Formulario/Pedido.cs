using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandsOnQnaMaker.Formulario
{
    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "Desculpe, não entendi \"{0}")]
    public class Pedido
    {
        public Item item { get; set; }
        public Animal animal { get; set; }
        public TipoEntrega tipoentrega { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }

        public static IForm<Pedido> BuildForm()
        {
            var form = new FormBuilder<Pedido>();
            form.Configuration.DefaultPrompt.ChoiceStyle = ChoiceStyleOptions.Buttons;
            form.Message("Pois não, vamos anotar o seu pedido: ");
            form.OnCompletion(async (context, pedido) => {
                await context.PostAsync("Seu pedido foi realizado com sucesso. Muito obrigado");
            });
            return form.Build();
        }
    }

    public enum Item
    {
        Racao = 1,
        Brinquedo, 
        Roupa,
        Outros
    }

    public enum Animal
    {
        Cachorro = 1,
        Gato,
        Passario
    }
    public enum TipoEntrega
    {
        [Terms("R","Retirar no Local", "Pegar no Local", "Em mãos")]
        [Describe("Retirar")]
        Retirar = 1,
        [Terms("T", "Transporte", "Van")]
        [Describe("Transportadora")]
        Transportadora,
        [Terms("C", "Encomenda", "Sedex")]
        [Describe("Correios")]
        Correios
    }
}