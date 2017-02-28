using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CallCenterBot.Models
{
    public enum DEMANDTYPE
    {
        SOFTWARE_DEMAND = 1,
        BILLING_ISSUE = 2,
        ACCOUNTABILITY_EXPERT = 3
        
    }
    [Serializable]
    public class DemandeModel
    {
        public string Title { get; set; }
        public DEMANDTYPE Demande { get; set; }
        public override string ToString()
        {
            return this.Title;
        }
    }
}