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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EV.Cognitives.Services.TextAnalytics;
using System.Threading.Tasks;
using System.Collections.Generic;
using EV.Cognitives.Services.Luis;
using System.Diagnostics;

namespace FRDX.Cognitives.Services.Tests
{
    [TestClass]
    public class UnitTestTextAnalytics
    {
        [TestInitialize]
        public void InitializeEngine()
        {
           TextEngine.Instance.Initialize("3c557dd4d4c54aad9b11260d30fb3ba4");      
           //Luis FR      
           LuisEngine.Instance.Initialize("42cc7c41-1b4a-4eaf-952d-a9875518d77d", "3cea665036ba4a5a92f0026694e70c7c");
        }
        [TestMethod]
        public async Task TextAnalytics_Detect_Language_fr()
        {            
            await TextAnalytics_Detect_Language("fr", new List<string> { "Salut" });            
        }
        [TestMethod]
        public async Task TextAnalytics_Detect_Sentiment_fr()
        {
            await TextAnalytics_Detect_Sentiment("fr", 
                 new List<string> { "J'ai un problème avec un retrait",
                                     "Je n'arrive pas à retirer",
                                    "Je suis content des services en ligne",
                                    "Je ne sais pas quoi vous dire",
                                    "Je sais quoi vous dire",
                                    "Pourquoi vous me dite cela",
                                    "Pourquoi pas",
                                    "J'en ai marre, le retrait d'argent ne fonctionne pas",
                                    "Envoyez moi tous les élèments nécessaires pour fermer mon compte",
                                    "S'il vous plait, Envoyez moi tous les élèments nécessaires pour fermer mon compte",
                                    "S'il vous plait, Envoyez moi tous les élèments nécessaires pour ouvrir un compte",
                                    "S'il vous plait, Envoyez moi tous les élèments nécessaires pour fermer mon compte",
                                    "S'il vous plait, Envoyez moi tous les élèments nécessaires pour cloturer mon compte",
                                    "SVP, Envoyez moi tous les élèments nécessaires pour fermer mon compte",
                                    "Envoyez moi tous les élèments nécessaires pour fermer mon compte, merci",
                                    "C'est le bordel ",
                                    "C'est toujours la même chose avec vous",
                                    "Pourriez-vous m'aider sur le sujet",
                                    "Bordel de service",
                                    "Je voudrais parler à mon conseiller",
                                    "Je vais finir par changer",
                                    "Je vais partir",
                                    "Je vais changer de banque",
                                    "Je vais cloturer mon compte",
                                    "Donnez moi les informations pour fermer mon compte",
                                    "Vous seriez gentil de m'aider avec mon problème de carte bleue",
                                    "Vous êtes bien aimable",
                                    "je n'arrive pas à avoir mon conseiller",
                                    "je n'arrive pas à retirer de l'argent à mon dab habituel",
                 "je souhaiterai annuler ma demande de résilation"  },0);


        }

        public async Task TextAnalytics_Detect_Sentiment_en()
        {
            await TextAnalytics_Detect_Language("en", new List<string> { "I have got a problem" });
        }

        [TestMethod]
        public async Task TextAnalytics_Detect_Language_en()
        {
            await TextAnalytics_Detect_Language("en", new List<string> { "I have got a problem" });
        }


        [TestMethod]
        public async Task Luis_Predict_Issue_fr()
        {
            //await Luis_Predict(
            //   new List<string> {
            //                        "je n'arrive pas à tirer de l'argent avec ma carte",
            //                        "je n'arrive pas à tirer de l'argent avec ma carte ni avec un chèque",
            //                        "je n'arrive pas à payer avec ma carte"

            //   });
            await Luis_Predict(
               new List<string> {   "j'ai un problème",
                                    "j'ai un problème avec ma carte",
                                    "j'ai un problème de retrait",
                                    "j'ai un problème de retrait domestique",
                                    "Je n'arrive pas à retirer d'argent",
                                    "ma carte est bloquée je suis à l'étranger",
                                    "ma carte est bloquée",
                                    "Je n'arrive pas à retirer d'argent avec ma carte",
                                    "mais je ne suis pas d'accord",
                                    "je n'arrive pas à avoir mon conseiller",
                                    "Je souhaite retirer de l'argent",
                                    "je n'arrive pas à retirer de l'argent à mon dab habituel",
                                    "je n'arrive pas à tirer de l'argent depuis l'étranger",
                                    "je n'arrive pas à retirer de l'argent à losangeles",
                                    "je n'arrive pas à retirer de l'argent à paris",
                                    "je n'arrive pas à retirer de l'argent à sanfransisco",
                                    "je n'arrive pas à retirer de l'argent à santafe",
                                    "je n'arrive pas à retirer de l'argent à bellevue",
                                    "je n'arrive pas à retirer de l'argent à bordeaux",
                                    "je n'arrive pas à retirer de l'argent à bethune"


               }, 0);
        }

        [TestMethod]
        public async Task Luis_Predict_Salutation_fr()
        {
            
            await Luis_Predict(
                new List<string> {
                                    "Bonjour",
                                    "Salutation",
                                    "Salut",

                });

           
        }
        
        public async Task Luis_Predict(List<string> sentences,int index=0)
        {
            var intents = await LuisEngine.Instance.PredictAsync(sentences,index);

            Console.Out.WriteLine("Query :         Intent :                         Score");
            Console.Out.WriteLine("-----------------------------------------------------------");
            foreach (var intent in intents)
            {
                if (intent != null)
                {
                    Console.Out.WriteLine("-----------------------------------------------------------");
                    Console.Out.WriteLine($"{intent.Query} :         {intent.Name} :                      {intent.Score}");
                    Console.Out.WriteLine("-----------------------------------------------------------");
                    foreach (var entity in intent.entities)
                    {
                        Console.Out.WriteLine($"{entity.entity} :         {entity.IssueType} :                      {entity.Score}");
                    }
                }
            }

        }

        public async Task TextAnalytics_Detect_Sentiment(string language, List<string> sentences,int index=0)
        {           
            var response = await TextEngine.Instance.DetectSentimentAsync(sentences, language,index);
            int count = response.documents.Count;
            
            if (count == 0)
            {
                Assert.Fail("No Language available");
            }
            
            foreach(var doc in response.documents)
            {                
                Console.Out.WriteLine($"{doc.text} :                         {doc.score}");
                
            }

        }

        public async Task TextAnalytics_Detect_Language(string language, List<string> sentences)
        {
            int countLanguageExpected = 1;
            var response = await TextEngine.Instance.DetectLanguageAsync(sentences,1);
            int count = response.documents.Count;
            if (count  == 0 )
            {
                Assert.Fail("No Language available");
            }

            count = response.documents[0].detectedLanguages.Count; 
            
            Assert.AreEqual(countLanguageExpected, count);

            var isoLanguageName = response.documents[0].detectedLanguages[0].iso6391Name;
            Assert.AreEqual(language, isoLanguageName);
           }


    }
}
