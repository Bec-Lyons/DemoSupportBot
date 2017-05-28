using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using AzureSearchBot.Services;
using Microsoft.Bot.Connector;
using System.Diagnostics;
using AzureSearchBot.Model;
using System.Collections.Generic;
using AzureSearchBot.Util;

namespace AzureSearchBot.Dialogs
{

    [Serializable]
    public class CategorySearchDialog : IDialog<object>
    {
        private readonly AzureSearchService searchService = new AzureSearchService();
        public async Task StartAsync(IDialogContext context)
        {
            try
            {
                FacetResult facetResult = await searchService.FetchFacets();
                if (facetResult.searchfacets.ArticleCategory.Length != 0)
                {
                    List<string> eras = new List<string>();
                    foreach (ArticleCategory era in facetResult.searchfacets.ArticleCategory)
                    {
                        eras.Add($"{era.value} ({era.count})");
                    }
                    PromptDialog.Choice(context, AfterMenuSelection, eras, "Which topic do you want to search?");
                }
                else
                {
                    await context.PostAsync("I'm having a bit of problems searching categories. Try me later.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error when faceting by category: {e}");
            }
        }

        private async Task AfterMenuSelection(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;
            string selectedEra = optionSelected.Split(' ')[0];

            try
            {
                SearchResult searchResult = await searchService.SearchByCategory(selectedEra);
                if(searchResult.value.Length != 0)
                {
                    CardUtil.showHeroCard((IMessageActivity)context.Activity, searchResult);
                }
                else
                {
                    await context.PostAsync($"I couldn't find any articles in that category :0");
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Error when filtering articles by category: {e}");
            }
            context.Done<object>(null);
        }
    }
}
    