using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using AzureSearchBot.Services;
using AzureSearchBot.Model;
using System.Diagnostics;
using AzureSearchBot.Util;
using System.Collections.Generic;
using formflow.FormFlow;

namespace AzureSearchBot.Dialogs
{

    [Serializable]
    public class TopicSearchDialog : IDialog<object>
    {
        private readonly AzureSearchService searchService = new AzureSearchService();
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("What information are you looking for? Type in a topic.");
            context.Wait(MessageRecievedAsync);
        }

        public virtual async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            Boolean found = false;
            try
            {
                SearchResult searchResult = await searchService.SearchByName(message.Text);

                int checksum = 0;
                foreach (Value musician in searchResult.value)
                {
                    if (checkValid(musician))
                    {
                        checksum++;
                    }
                }
                if (searchResult.value.Length != 0 && checksum!=0)
                {
                    CardUtil.showHeroCard(message, searchResult);
                    found = true;
                }
                else
                {
                    await context.PostAsync($"No information by the topic *{message.Text}* found");
                    found = false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error when searching for topic: {e.Message}");
            }
            if (found)
            {
                context.Done<object>(null);
            }
            else
            {
                PromptDialog.Choice(context, this.AfterMenuSelection, new List<string>() { "Search by Category", "Cancel" }, "How would you like to proceed?");
            }
        }

        //After users select option, Bot call other dialogs
        private async Task AfterMenuSelection(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;
            switch (optionSelected)
            {
                case "Cancel":
                    await context.PostAsync("Cancelled.");
                    context.Done<object>(null);
                    break;
                case "Search by Category":
                    context.Call(new CategorySearchDialog(), ResumeAfterOptionDialog);
                    break;
            }
        }

        private Boolean checkValid(Value article)
        {
            return article.searchscore > 0.1;
        }

        //This function is called after each dialog process is done
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }


    }
}