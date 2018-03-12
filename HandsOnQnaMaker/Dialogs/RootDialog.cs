using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HandsOnQnaMaker.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            await context.PostAsync("Ola, tudo bem?");

            var message = activity.CreateReply();

            if (activity.Text.Equals("herocard", StringComparison.InvariantCultureIgnoreCase))
            {
                var herocard = new HeroCard();
                herocard.Title = "Bem vindo";
                herocard.Subtitle = "Obrigado por visitar nossa loja";
                herocard.Images = new List<CardImage> {
            new CardImage("http://www.guiazonaoestesp.com.br/rp/riopequeno/images/petshop.png", "logo")
            };

                message.Attachments.Add(herocard.ToAttachment());
            }

            if (activity.Text.Equals("audiocard", StringComparison.InvariantCultureIgnoreCase))
            {
                var videocard = new VideoCard();
                videocard.Title = "show";
                videocard.Subtitle = "Conheça nossa loja fisica";
                videocard.Autostart = true;
                videocard.Autoloop = false;

            }
            await context.PostAsync(message);

            await context.PostAsync("Como podemos ajudar?");
            //// calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }
    }
}