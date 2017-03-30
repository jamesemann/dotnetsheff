using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CardsDemoBot.Dialogs
{
    [Serializable]
    public class CardsDemoDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedStart);
        }
        public async Task MessageReceivedStart(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var activity = await argument;

            if (activity != null && activity.Text != null)
            {
                var replyMessage = context.MakeMessage();
                replyMessage.Attachments = new List<Attachment>();
                
                switch (activity.Text.ToLower())
                {
                    case "static-card":
                        {
                            ShowStaticCard(replyMessage);
                            break;
                        }
                    case "hero-card":
                        {
                            ShowHeroCard(replyMessage);
                            break;
                        }
                    case "thumbnail-card":
                        {
                            ShowThumbnailCard(replyMessage);
                            break;
                        }
                    case "receipt-card":
                        {
                            ShowReceiptCard(replyMessage);
                            break;
                        }
                    case "signin-card":
                        {
                            ShowSignInCard(replyMessage);
                            break;
                        }
                    case "airline-checkin-card":
                        {
                            ShowFacebookMessengerAirlineCheckInCard(replyMessage);
                            break;
                        }
                    case "airline-update-card":
                        {
                            ShowFacebookMessengerAirlineFlightUpdateCard(replyMessage);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                await context.PostAsync(replyMessage);

            }

            context.Wait(MessageReceivedStart);
        }

        private static void ShowSignInCard(IMessageActivity replyMessage)
        {
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = "https://<OAuthSignInURL>",
                Type = "signin",
                Title = "Connect"
            };
            cardButtons.Add(plButton);
            SigninCard plCard = new SigninCard(text: "You need to authorize me", buttons: cardButtons);
            Attachment plAttachment = plCard.ToAttachment();
            replyMessage.Attachments.Add(plAttachment);
        }

        private static void ShowReceiptCard(IMessageActivity replyMessage)
        {
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://lorempixel.com/200/200/food"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                Type = "openUrl",
                Title = "Wikipedia Page"
            };
            cardButtons.Add(plButton);
            ReceiptItem lineItem1 = new ReceiptItem()
            {
                Title = "Pork Shoulder",
                Subtitle = "8 lbs",
                Text = null,
                Image = new CardImage(url: "http://lorempixel.com/200/200/food"),
                Price = "16.25",
                Quantity = "1",
                Tap = null
            };
            ReceiptItem lineItem2 = new ReceiptItem()
            {
                Title = "Bacon",
                Subtitle = "5 lbs",
                Text = null,
                Image = new CardImage(url: "http://lorempixel.com/200/200/food"),
                Price = "34.50",
                Quantity = "2",
                Tap = null
            };
            List<ReceiptItem> receiptList = new List<ReceiptItem>();
            receiptList.Add(lineItem1);
            receiptList.Add(lineItem2);
            ReceiptCard plCard = new ReceiptCard()
            {
                Title = "I'm a receipt card, isn't this bacon expensive?",
                Buttons = cardButtons,
                Items = receiptList, 
                Total = "275.25",
                Tax = "27.52"
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyMessage.Attachments.Add(plAttachment);
        }

        private static void ShowThumbnailCard(IMessageActivity replyMessage)
        {
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://lorempixel.com/200/200/food"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                Type = "openUrl",
                Title = "Wikipedia Page"
            };
            cardButtons.Add(plButton);
            ThumbnailCard plCard = new ThumbnailCard()
            {
                Title = "I'm a thumbnail card",
                Subtitle = "Wikipedia Page",
                Images = cardImages,
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyMessage.Attachments.Add(plAttachment);
        }

        private static void ShowHeroCard(IMessageActivity replyMessage)
        {
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://lorempixel.com/200/200/food"));
            cardImages.Add(new CardImage(url: "http://lorempixel.com/200/200/food"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                Type = "openUrl",
                Title = "Wikipedia Page"
            };
            cardButtons.Add(plButton);
            HeroCard plCard = new HeroCard()
            {
                Title = "I'm a hero card",
                Subtitle = "Wikipedia Page",
                Images = cardImages,
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyMessage.Attachments.Add(plAttachment);
        }

        private static void ShowStaticCard(IMessageActivity replyMessage)
        {
            replyMessage.Attachments.Add(new Attachment()
            {
                ContentUrl = "http://lorempixel.com/200/200/food",
                ContentType = "image/png",
                Name = "food.png"
            });
        }

        private static void ShowFacebookMessengerAirlineCheckInCard(IMessageActivity replyMessage)
        {
            var attachment = new
            {
                type = "template",
                payload = new
                {
                    template_type = "airline_checkin",
                    intro_message = "Check-in is available now.",
                    locale = "en_US",
                    pnr_number = "ABCDEF",
                    checkin_url = "https://www.airline.com/check-in",
                    flight_info = new[]
                    {
                        new
                        {
                            flight_number = "AL01",
                            departure_airport =
                            new
                            {
                                airport_code = "SFO",
                                city = "San Francisco",
                                terminal = "T4",
                                gate = "G8"
                            },
                            arrival_airport = new
                            {
                                airport_code = "SEA",
                                city = "Seattle",
                                terminal = "T4",
                                gate = "G8"
                            },
                            flight_schedule = new
                            {
                                boarding_time = "2016-01-05T15:05",
                                departure_time = "2016-01-05T15:45",
                                arrival_time = "2016-01-05T17:30"
                            }
                        }
                    }
                }
            };

            replyMessage.ChannelData = JObject.FromObject(new { attachment });
        }
        private static void ShowFacebookMessengerAirlineFlightUpdateCard(IMessageActivity replyMessage)
        {
            var attachment = new
            {
                type = "template",
                payload = new
                {
                    template_type = "airline_update",
                    intro_message = "Your flight is delayed.",
                    update_type = "delay",
                    locale = "en_US",
                    pnr_number = "ABCDEF",
                    //checkin_url = "https://www.airline.com/check-in",
                    update_flight_info =
                        new
                        {
                            flight_number = "AL01",
                            departure_airport =
                            new
                            {
                                airport_code = "SFO",
                                city = "San Francisco",
                                terminal = "T4",
                                gate = "G8"
                            },
                            arrival_airport = new
                            {
                                airport_code = "SEA",
                                city = "Seattle",
                                terminal = "T4",
                                gate = "G8"
                            },
                            flight_schedule = new
                            {
                                boarding_time = "2016-01-05T15:05",
                                departure_time = "2016-01-05T15:45",
                                arrival_time = "2016-01-05T17:30"
                            }
                        }
                }
            };

            replyMessage.ChannelData = JObject.FromObject(new { attachment });
        }
    }
}