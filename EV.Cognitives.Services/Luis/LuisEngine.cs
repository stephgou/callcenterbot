using Microsoft.Cognitive.LUIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EV.Cognitives.Services.Luis
{
   public  class LuisEngine
    {
        private LuisClient _luisClient;
        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static LuisEngine _instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static LuisEngine Instance => _instance ?? (_instance = new LuisEngine());
        public bool Initialize(string appId,string apiKey)
        {
         
            _luisClient = new LuisClient(appId, apiKey, false);
            return true;
        }

        public async Task<LuisIntentModel> PredictAsync(string sentence)
        {
            LuisIntentModel topIntent = null;
            try
            {
                LuisResult result = await _luisClient.Predict(sentence);
                List<LuisEntity> entities = new List<LuisEntity>();
              
                foreach(var e in result.Entities)
                {
                    if (e.Value.Count >0)
                    {
                        var entity = e.Value[0];
                        var Name = entity.Name;
                        var score = entity.Score;
                        var z = entity.Value;
                        entities.Add(new LuisEntity { entity =z , IssueType = Name, Score = score });
                    }

                }
                topIntent = new LuisIntentModel {entities=entities, Query = result.OriginalQuery, Score = result.TopScoringIntent.Score, Name = result.TopScoringIntent.Name };
                
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
            return topIntent;
        }
        public async Task<List<LuisIntentModel>> PredictAsync(List<string> sentences, int index=0)
        {
            List<LuisIntentModel> luisTop = new List<LuisIntentModel>();
            int count = sentences.Count ;
            for (int i = index; i < count; i++)
            {
                var sentence = sentences[i];
                luisTop.Add(await PredictAsync(sentence));
            }
            
            return luisTop;
        }
    }
}
