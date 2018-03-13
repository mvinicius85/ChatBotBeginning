using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace HandsOnQnaMaker.Dialogs
{
    [Serializable]
    [LuisModel("8e9252a2-7172-4798-9959-c8f57ae00853", "3c0344efc0104fcfb2a2540511fe4746")]
    public class PedidoDialog : LuisDialog<object>
    {
        [LuisIntent("None")]
        public async Task None(IDialogContext context,  LuisResult result)
        {
            await context.PostAsync($"Desculpe, não sei o que significa {result.Query}");
               
        }
    
        [LuisIntent("consciencia")]
        public async Task consciencia(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Eu sou o bot do petshop. Posso tirar duvidas sobre produtos, e receber seus pedidos.");
        }

        [LuisIntent("saudar")]
        public async Task saudar(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Ola, tudo bem com voce? Como posso lhe ajudar?");
        }
        [LuisIntent("Pedido")]
        public async Task Pedido(IDialogContext context, LuisResult result)
        {
            var prod = result.Entities?.Select( e => e.Entity);
            var endpoint = $"http://handsonchatbotwebapi.azurewebsites.net/api/produto/{prod.FirstOrDefault()}";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    await context.PostAsync("Não foi possível identificar o produto.");
                }
                else
                {
                    await context.PostAsync("Aguarde um momento por favor.");
                    var json = await response.Content.ReadAsStringAsync();
                    var resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<Model.Produto>(json);
                    await context.PostAsync("Seguem os dados do produto solicitado: Nome" + resultado.Nome + ", Valor: " + resultado.Preco);
                }
            }
            //await context.PostAsync("Ainda não estou pronto para receber pedidos.");
        }
        
    }
}
