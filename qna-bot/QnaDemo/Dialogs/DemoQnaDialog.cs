using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using QnAMakerDialog;

namespace QnaDemo.Dialogs
{
    [Serializable]
    [QnAMakerService("646475201fe74e5aa1aed248c253d751", "3492b54e-5aaa-49b3-8fcd-2878782bd20d")]
    public class DemoQnaDialog : QnAMakerDialog<object>
    {
        public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        {
            await context.PostAsync($"Sorry, I couldn't find an answer for '{originalQueryText}'.");
            context.Wait(MessageReceived);
        }
    }
}