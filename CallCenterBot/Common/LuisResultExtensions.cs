using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CallCenterBot.Common
{
    static public class LuisResultExtensions
    {
        public static bool IsInRange(this IntentRecommendation result, double range=0.7)
        {
            return result.Score.Value > range;
        }
    }
}