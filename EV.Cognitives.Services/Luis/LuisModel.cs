using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EV.Cognitives.Services.Luis
{
    public class LuisIntentModel
    {
        public double Score { get; set; }
        public string Name { get; set; }
        public string Query { get; set; }
        public List<LuisEntity> entities { get; set; }
    }
    public class LuisEntity
    {
        public string entity { get; set; }
        public string IssueType { get; set; }
        public double Score { get; set; }
    }
}
