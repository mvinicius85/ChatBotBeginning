using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HandsOnQnaMaker.Dialogs
{
    [Serializable]
    public class QnADialog : QnAMakerDialog
    {
        public QnADialog() : base(new QnAMakerService(new QnAMakerAttribute(ConfigurationManager.AppSettings["QnaSubscriptionKey"], ConfigurationManager.AppSettings["QnaKnowledgebaseId"], "Não, encontrei sua resposta", 0.5)))
        {

        }
    }
}