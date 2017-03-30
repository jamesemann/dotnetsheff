using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace FormFlowBot.Forms
{
    [Serializable]
    public class ParkingEnquiryForm
    {
        public string MachineIdOrPostcode { get; set; }
        public bool IsFault { get; set; }
        public string Problem { get; set; }

        public static IForm<ParkingEnquiryForm> BuildForm()
        {
            var formbuilder = new FormBuilder<ParkingEnquiryForm>()
                .Field(nameof(IsFault))
                .Field(nameof(MachineIdOrPostcode))
                .Confirm(
                    async state =>
                    {
                        return
                            new PromptAttribute(
                                $"![img](https://maps.google.com/maps/api/staticmap?center={state.MachineIdOrPostcode}&zoom=15&size=400x400&maptype=roadmap&markers=color:ORANGE|label:A|{state.MachineIdOrPostcode}&sensor=false) \r\n\r\n\r\nIs the parking meter here?");
                    })
                .Field(nameof(Problem), state => { return state.IsFault; })
                .OnCompletion(async (context, state) =>
                {
                    if (state.IsFault)
                        await context.PostAsync(
                            "Thank you for reporting the fault, we will have someone look at it immediately and contact you back on this channel");

                    else
                        await context.PostAsync(
                            $"[Parking information for {state.MachineIdOrPostcode}](http://fastandfluid.com/publicdownloads/AngularJSIn60MinutesIsh_DanWahlin_May2013.pdf");
                })
                .Build();

            return formbuilder;
        }
    }
}