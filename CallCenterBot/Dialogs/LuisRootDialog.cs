using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CallCenterBot.Common;

namespace CallCenterBot.Dialogs
{
    [Serializable]
    [LuisModel("", "")]
    public class LuisRootDialog : LuisDialog<object>
    {
        public const string FACTURENUMBER_TYPE_ENTITY = "FactureNumber";

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {

            await context.PostAsync("Désolé, mais je n'ai pas compris");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("GetFactureNumber")]
        public async Task GetFactureNumber(IDialogContext context, LuisResult result)
        {
            string factureNumber = "";
            EntityRecommendation factureNumberEntityRecommmandation;

            try
            {
                if (result.TopScoringIntent.IsInRange())
                {
                    if (result.Entities.Count != 0)
                    {
                        //var entitiesRecommendation = from e in result.Entities
                        //    where (e.Type == FACTURENUMBER_TYPE_ENTITY)
                        //    select e;
                        //var entityRecommendation = entitiesRecommendation.FirstOrDefault();
                        //factureNumber = entityRecommendation.Entity;
                        if (!result.TryFindEntity(FACTURENUMBER_TYPE_ENTITY, out factureNumberEntityRecommmandation))
                        {
                            await context.PostAsync("Je suis désolé mais je n'ai pas compris quel était votre numéro de facture. Je vous invite à utiliser la commande 'help' si vous souhaitez être mis en relation avec l'un de nos agents...");
                            return;
                        }
                        else
                        {
                            factureNumber = factureNumberEntityRecommmandation.Entity;
                        }
                    }
                }
                //if (factureNumber == "")
                //    await context.PostAsync("Ce numéro de facture est invalide. Je vous invite à utiliser la commande 'help' si vous souhaitez être mis en relation avec l'un de nos agents...");
                //else
                //    await context.PostAsync("Votre numéro de facture est : " + factureNumber + ". Nous allons maintenant pouvoir traiter votre demande.");
                if (factureNumber == "")
                    context.Done("Ce numéro de facture est invalide. Je vous invite à utiliser la commande 'help' si vous souhaitez être mis en relation avec l'un de nos agents...");
                else
                    context.Done("Votre numéro de facture est : " + factureNumber + ". Nous allons maintenant pouvoir traiter votre demande.");
                //context.Wait(this.MessageReceived);
            }
            catch (Exception e)
            {
                await context.PostAsync(string.Format("Une erreur est survenue: {0}", e.Message));
                context.Wait(MessageReceived);
            }
            finally
            {
            }
        }
    }
}