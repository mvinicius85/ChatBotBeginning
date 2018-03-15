using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bot.CognitiveServices;
using HandsOnQnaMaker.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
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
        public async Task None(IDialogContext context, LuisResult result)
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
            //await context.PostAsync("Envie uma imagem"):
            //context.Wait((c, a) => Proc)
            var animal = result.Entities?.Select(e => e.Entity);
            int paramanimal = 0;
            switch (animal.FirstOrDefault().ToString())
            {
                case "cachorro":
                case "dog":
                case "cão":
                    paramanimal = 1;
                    break;
                case "gato":
                    paramanimal = 2;
                    break;

            }
            var endpoint = $"http://handsonchatbotwebapi.azurewebsites.net/api/produto/{paramanimal}";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    await context.PostAsync("Me desculpe, infelizmente não temos o produto que você esta procurando.");
                }
                else
                {
                    await context.PostAsync("Aguarde um momento por favor.");
                    var json = await response.Content.ReadAsStringAsync();
                    var resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Model.Racao>>(json);
                    await context.PostAsync("Encontrei os seguintes itens para voce:");
                    foreach (var item in resultado)
                    {
                        context.PostAsync("Produto: " + item.Nome + "  \r\n" +
                                          "Valor: " + item.Preco + " +" +
                                          "Qtde: " + item.Unidade + "");
                    }
                    //var cotacoes = resultado.Cotacoes?.Select(c => $"{c.Nome" : {c.Valor}");
                    //await context.PostAsync($"{string.Join(", ",cotacores.toArray())});
                    //await context.PostAsync("Seguem os dados do produto solicitado: Nome" + resultado.Nome + ", Valor: " + resultado.Preco);
                }
            }
            //await context.PostAsync("Ainda não estou pronto para receber pedidos.");
        }
        [LuisIntent("Ajuda")]
        public async Task Ajuda(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Eu posso te ajudar a escolher qual a melhor ração para seu cãozinho. " +
                "Me envie uma foto dele para que eu possa tentar ajudar.");
            context.Wait((c, a) => this.ProcessarImagemAsync(c, a));
        }
        [LuisIntent("FazerPedido")]
        public async Task FazerPedido(IDialogContext context, LuisResult result)
        {
            //await Conversation.SendAsync(Activity.CreateMessageActivity(), () => Chain.From(() => FormDialog.FromForm(() => Formulario.Pedido.BuildForm(), FormOptions.PromptFieldsWithValues)));
            await context.PostAsync("Sinto muito mas ainda não estamos preparados para atender o " +
               "seu pedido. Por favor nos desculpe.");

        }

        private async Task ProcessarImagemAsync(IDialogContext contexto, IAwaitable<IMessageActivity> argument)
        {
            var activity = await argument;

            var uri = activity.Attachments?.Any() == true ?
                new Uri(activity.Attachments[0].ContentUrl) :
                new Uri(activity.Text);

            try
            {
                Prediction result = new Prediction();
                result = await new VisaoComputacional().ClassificacaoCustomizadaAsync(uri);
                int raca = Util.RetornaPorteCachorro(result.Tag);
                var endpoint = $"http://handsonchatbotwebapi.azurewebsites.net/api/produto/1,{raca}";
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(endpoint);
                    if (!response.IsSuccessStatusCode)
                    {
                        await contexto.PostAsync("Me desculpe, infelizmente não temos o produto que você esta procurando.");
                    }
                    else
                    {
                        await contexto.PostAsync("Aguarde um momento por favor.");
                        var json = await response.Content.ReadAsStringAsync();
                        var resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Model.Racao>>(json);
                        await contexto.PostAsync($"Bom, analisando a imagem acredito que seu cachorro seja da raca {result.Tag}. " +
                            $"Sendo assim, as rações que temos para ele são as seguintes:");
                        foreach (var item in resultado)
                        {
                            contexto.PostAsync("Produto: " + item.Nome + "  \r\n" +
                                              "Valor: " + item.Preco + " +" +
                                              "Qtde: " + item.Unidade + "");
                        }
                        //var cotacoes = resultado.Cotacoes?.Select(c => $"{c.Nome" : {c.Valor}");
                        //await context.PostAsync($"{string.Join(", ",cotacores.toArray())});
                        //await context.PostAsync("Seguem os dados do produto solicitado: Nome" + resultado.Nome + ", Valor: " + resultado.Preco);
                    }
                }
                //await contexto.PostAsync(reply);
            }
            catch (Exception)
            {
                await contexto.PostAsync("Ops! Deu algo errado na hora de analisar sua imagem!");
            }

            contexto.Wait(MessageReceived);
        }
    }
}
