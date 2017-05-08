using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    internal class SearchDialog: IDialog<string>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait<string>(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {
            var query = await result;

            var profiles = await new GitHubClient().ExecuteSearch(query);

            var totalCount = profiles.TotalCount;

            if (totalCount == 0)
            {
                context.Done(@"Sorry, no results found");
            }
            else if (totalCount > 10)
            {
                context.Done(@"More than 10 results were found. Please narrow your search");
            }
            else
            {
                PromptDialog.Choice(context, UserChosen, profiles.Items.Select(
                    item => item.Login), @"Which profile would you like to see?");
            }
        }

        private async Task UserChosen(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(await result);
        }
    }
}