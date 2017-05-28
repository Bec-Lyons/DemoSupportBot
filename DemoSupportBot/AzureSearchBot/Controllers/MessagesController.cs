using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using AzureSearchBot.Dialogs;

namespace AzureSearchBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Post receives a message from a user and invokes the root dialog
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            //Activity: The Connector uses an Activity object to pass information back and forth between bot and channel (user).
            //Activities can be multiple types - to communication various types of information to a bot or channel.
            if (activity.Type == ActivityTypes.Message)
            {
                //Connector (single REST API) enables a bot to communicate across multiple channels 
                //Connector facilitates communication between bot + user by relaying messages from bot to channel (vice versa)
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                //Start the Root Dialog
                //Dialogs: model a conversation and manage conversation flow.
                //Dialog can be composed of other dialogs to maximize reuse
                //Dialogs are arranged in a stack, and the top dialog in the stack processes all incoming messages until it is closed or a different dialog is invoked.
                await Conversation.SendAsync(activity, () => new RootDialog());
            }
            else
            {
                //Handle all other activities types 
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}