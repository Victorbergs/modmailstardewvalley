using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using System.Collections.Generic;

namespace Firstmod
{
    public class ModEntry : Mod
    {
        private IDictionary<string, string> customMail;

        public override void Entry(IModHelper helper)
        {
            // Load custom mail data
            customMail = helper.GameContent.Load<Dictionary<string, string>>("assets/mail.json");

            // Hook into the DayStarted event
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            // Hook into the SaveLoaded event to inject mail
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            // Check the current day and add mail if conditions are met
            if (Game1.Date.DayOfMonth == 1 && Game1.Date.Season == Season.Spring)
            {
                Game1.addMailForTomorrow("spring_1");
            }
            else if (Game1.Date.DayOfMonth == 1 && Game1.Date.Season == Season.Fall)
            {
                Game1.addMailForTomorrow("fall_1");
            }
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            // Inject custom mail into the game's mail data
            foreach (var mail in customMail)
            {
                if (!Game1.content.Load<Dictionary<string, string>>("Data\\Mail").ContainsKey(mail.Key))
                {
                    Game1.content.Load<Dictionary<string, string>>("Data\\Mail")[mail.Key] = mail.Value;
                }
            }
        }
    }
}   