using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var msg = await result as IMessageActivity;
            if (msg.Text.Equals(@"search", StringComparison.OrdinalIgnoreCase))
            {
                PromptDialog.Text(context, QueryEntered, @"Who do you want to serach for?");
            }
            else if (msg.Text.StartsWith(@"search ", StringComparison.OrdinalIgnoreCase))
            {
                var query = msg.Text.Substring(7);
                await context.Forward<string, string>(new SearchDialog(), SearchComplete, query,
                    default(CancellationToken));
            }
        }

        private async Task QueryEntered(IDialogContext context, IAwaitable<string> result)
        {
            await context.Forward<string, string>(new SearchDialog(), SearchComplete, await result,
                default(CancellationToken));
        }

        private async Task SearchComplete(IDialogContext context, IAwaitable<string> result)
        {
            var msg = await result;

            await context.PostAsync(msg);

            context.Wait(MessageReceivedAsync);
        }
    }
}