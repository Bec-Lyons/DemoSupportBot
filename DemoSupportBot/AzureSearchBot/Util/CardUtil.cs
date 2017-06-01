using System;
using System.Collections.Generic;
using Microsoft.Bot.Connector;
using AzureSearchBot.Model;

namespace AzureSearchBot.Util
{
    public static class CardUtil
    {
        public static async void showHeroCard(IMessageActivity message, SearchResult searchResult)
        {
            //Make reply activity and set layout
            Activity reply = ((Activity)message).CreateReply();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            //Make each Card for each article
            foreach (Value article in searchResult.value)
            {
                if (article.searchscore > 0.1)
                {
                    List<CardImage> cardImages = new List<CardImage>();
                    //cardImages.Add(new CardImage(url: musician.ArticleIMG));
                    String summarytext = "";

                    if (article.ArticleBody.Length > 200)
                    {
                        summarytext = article.ArticleBody.Substring(0, 200) + "...";
                    }
                    else
                    {
                        summarytext = article.ArticleBody;
                    }

                    HeroCard card = new HeroCard()
                    {
                        Title = article.ArticleTitle,
                        Subtitle = $"Category: {article.ArticleCategory } | Search Score: {article.searchscore}",
                        Text = summarytext,
                        Images = cardImages,
                        Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "See more.", value: "" + article.ArticleURL) }

                    };
                    reply.Attachments.Add(card.ToAttachment());
                }
            }

            //make connector and reply message
            ConnectorClient connector = new ConnectorClient(new Uri(reply.ServiceUrl));
            await connector.Conversations.SendToConversationAsync(reply);
        }
    }
}