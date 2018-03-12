using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

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
            await context.PostAsync("Ainda não estou pronto para receber pedidos.");
        }
        
    }
}
