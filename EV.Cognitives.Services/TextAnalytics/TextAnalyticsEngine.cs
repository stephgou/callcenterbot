// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************

using FRDX.Cognitives.Services.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EV.Cognitives.Services.TextAnalytics
{
    public class TextEngine
    {
        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static TextEngine _instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static TextEngine Instance => _instance ?? (_instance = new TextEngine());

        /// <summary>
        /// Today only westus available
        /// </summary>
        private const string ServiceBaseUri = "https://westus.api.cognitive.microsoft.com";

        private Transport _transport;

        public bool Initialize(string apiKey)
        {
            _transport = new Transport(ServiceBaseUri, apiKey);
            return true;
        }

        public async Task<ResultAnalyzeDataModel> DetectLanguageAsync(List<string> text, short countLanguage)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var detectLanguage = new DetectedLanguageDataModel();
            detectLanguage.documents = new List<Document>();
            foreach(var t in text)
            {
                detectLanguage.documents.Add(new Document { id = Guid.NewGuid().ToString(), text = t });
            }
            
            string uri = $"text/analytics/v2.0/languages?{countLanguage}";           
            return await _transport.PostAsync<ResultAnalyzeDataModel>(uri, detectLanguage, CancellationToken.None);            

        }
        public async Task<DetectedLanguageDataModel> DetectSentimentAsync(List<string> text, string language,int index = 0)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var detectLanguage = new DetectedLanguageDataModel();
            detectLanguage.documents = new List<Document>();
            for(int i=index;i<text.Count;i++)            
            {
                var t = text[i];
                detectLanguage.documents.Add(new Document { id = Guid.NewGuid().ToString(), text = t, language = language});
            }


            string uri = $"text/analytics/v2.0/sentiment";
            var sentiments = await _transport.PostAsync<DetectedLanguageDataModel>(uri, detectLanguage, CancellationToken.None);
            //Just for debug
#if DEBUG
            int count = sentiments.documents.Count;
            int offset = 0;
            if (sentiments.documents.Count >0)
            {
                for(int i=index; i< text.Count;  i++)
                {
                    sentiments.documents[offset++].text = text[i];
                }
            }
#endif
            return sentiments;
        }
    }
}
