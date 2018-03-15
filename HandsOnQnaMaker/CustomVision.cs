
using HandsOnQnaMaker.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Bot.CognitiveServices
{
    public class VisaoComputacional
    {
        private readonly string _customApiKey = ConfigurationManager.AppSettings["CustomVisionKey"];
        private readonly string _customVisionUri = ConfigurationManager.AppSettings["CustomVisionUri"];

        public async Task<Prediction> ClassificacaoCustomizadaAsync(Uri query)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-key", _customApiKey);

            HttpResponseMessage response = null;

            var byteData = Encoding.UTF8.GetBytes("{ 'url': '" + query + "' }");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(_customVisionUri, content).ConfigureAwait(false);
            }

            var responseString = await response.Content.ReadAsStringAsync();

            var tags = JsonConvert.DeserializeObject<CustomVisionModel>(responseString);

            var tag = tags.Predictions.OrderByDescending(c => c.Probability).FirstOrDefault();

            return tag;
        }

    }

}