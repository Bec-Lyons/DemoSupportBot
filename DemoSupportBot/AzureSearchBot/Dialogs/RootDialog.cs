using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using Microsoft.Bot.Builder.FormFlow;
using formflow.FormFlow;

namespace AzureSearchBot.Dialogs
{
    /// <summary>
    /// The root dialog processes the message and generates a response
    /// </summary>
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string PactSimpleSearchOption = "Search by topic";
        private const string PactCategorySearchOption = "Search by category";
        private const string FormOption = "Log support ticket";

        public async Task StartAsync(IDialogContext context)
        {
            /* Wait until the first message is received from the conversation and call MessageReceviedAsync 
             *  to process that message. */
            await context.PostAsync("Hi. I'm a demo support bot.");
            context.Wait(this.MessageRecievedAsync);
        }

        public virtual async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            /* When MessageReceivedAsync is called, it's passed an IAwaitable<IMessageActivity>. To get the message,
             *  await the result. */
            //Show options whatever users chat
            PromptDialog.Choice(context, this.AfterMenuSelection, new List<string>() { PactSimpleSearchOption, PactCategorySearchOption, FormOption }, "How can I help you?");
        }

        //After users select option, Bot call other dialogs
        private async Task AfterMenuSelection(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;
            switch (optionSelected)
            {
                case PactSimpleSearchOption:
                    context.Call(new TopicSearchDialog(), ResumeAfterOptionDialog);
                    break;
                case PactCategorySearchOption:
                    context.Call(new CategorySearchDialog(), ResumeAfterOptionDialog);
                    break;
                case FormOption:
                    var myform = new FormDialog<Enquiry>(new Enquiry(), Enquiry.BuildEnquiryForm, FormOptions.PromptInStart, null);
                    context.Call<Enquiry>(myform, ResumeAfterFormOption);
                    break;

            }
        }


        //This function is called after each dialog process is done
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            //This means  MessageRecievedAsync function of this dialog (PromptButtonsDialog) will receive users' messeges
            await context.PostAsync("Type anything to start again");
            context.Wait(MessageRecievedAsync);
        }

        private async Task ResumeAfterFormOption(IDialogContext context, IAwaitable<Enquiry> result)
            {
                Enquiry order = null;
                try
                {
                    order = await result;
                }
                catch (OperationCanceledException)
                {
                    await context.PostAsync("You canceled the form!");
                    return;
                }

                if (order != null)
                {
                    string orderstring = "Your Form is comeplete. Information captured: " + order.Name + order.JobTitle;
  
                    await context.PostAsync(orderstring);

                }
                else
                {
                    await context.PostAsync("Form returned empty response!");
                }

                await context.PostAsync("Type anything to start again");
                context.Wait(MessageRecievedAsync);
            }
    }
}