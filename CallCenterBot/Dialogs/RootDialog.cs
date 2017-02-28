using CallCenterBot.Models;
using MessageRouting;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Microsoft.Bot.Builder.Dialogs.PromptDialog;

namespace CallCenterBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
#pragma warning disable 1998
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
#pragma warning restore 1998

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            IMessageActivity messageActivity = await result;
            string message = messageActivity.Text;

            if (!string.IsNullOrEmpty(message))
            {
                if (message.ToLower().Contains("help"))
                {
                    await MessageRouterManager.Instance.InitiateEngagementAsync((messageActivity as Activity));
                    context.Done(this);
                }
                else if (message.ToLower().Contains("hal"))
                {
                    await context.PostAsync("Bonjour je m'appelle HAL et je suis à votre service");
                    var listDemandes = new List<DemandeModel>();
                    listDemandes.Add(new DemandeModel { Title = "Problème logiciel", Demande = DEMANDTYPE.SOFTWARE_DEMAND });
                    listDemandes.Add(new DemandeModel { Title = "Problème de facturation", Demande = DEMANDTYPE.BILLING_ISSUE });
                    listDemandes.Add(new DemandeModel { Title = "Expertise comptable", Demande = DEMANDTYPE.ACCOUNTABILITY_EXPERT });

                    var promptOptions = new PromptOptions<DemandeModel>("Votre demande concerne :",
                                                              "Cette option n'est pas valide", "trop d'essais",
                                                              listDemandes,
                                                              3,
                                                              new PromptStyler(PromptStyle.Auto));

                    var child = new PromptChoice<DemandeModel>(promptOptions);
                    context.Call<DemandeModel>(child, OnOperationSelected);
                }
                else
                {
                    messageActivity = context.MakeMessage();
                    messageActivity.Text = $"" + message;
                    await context.PostAsync(messageActivity);
                    context.Done(this);
                }
            }
        }
        public async Task OnOperationSelected(IDialogContext context, IAwaitable<DemandeModel> result)
        {
            var message = await result;
            try
            {   
                switch(message.Demande)
                    {
                    case DEMANDTYPE.ACCOUNTABILITY_EXPERT:
                        break;
                    case DEMANDTYPE.BILLING_ISSUE:
                        await context.PostAsync("Pourriez-vous nous donner votre numéro de facture SVP ?");
                        context.Call(new LuisRootDialog(), ResumeAfterDemande);
                        break;
                    case DEMANDTYPE.SOFTWARE_DEMAND:
                        break;
                    }
                //var child = new PromptConfirm(Resources.BOT_PROMPT_CONFIRM_CHOICE,
                //                              Resources.BOT_PROMPT_CONFIRM_WRONG_CHOICE,
                //                              Constant.MaxRetry, PromptStyle.Auto);
                //context.Call(child, OnOperationConfirmed);

            }
            catch (TooManyAttemptsException)
            {
                context.Done("Je suis désolé mais je n'ai pas compris. Je vous invite à utiliser la commande 'help' si vous souhaitez être mis en relation avec l'un de nos agents...");
            }
        }
        private async Task ResumeAfterDemande(IDialogContext context, IAwaitable<object> result)
        {

            var message = await result;
            await context.PostAsync(message.ToString());

            await this.StartAsync(context);
        }
    }
}