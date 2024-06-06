using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Firstmod
{
    internal sealed class ModEntry : Mod
    {
        private IDictionary<string, string> customMail;

        public override void Entry(IModHelper helper)
        {
            // Log when Entry is called
            Monitor.Log("Entry method called", LogLevel.Debug);

            try
            {
                // Load custom mail data
                Monitor.Log("Custom mail data loaded", LogLevel.Debug);
                customMail = helper.GameContent.Load<Dictionary<string, string>>("assets/mail.json");
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed to load custom mail data: {ex.Message}", LogLevel.Error);
                return;
            }

            // Hook into the DayStarted event
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            Monitor.Log("DayStarted event hooked", LogLevel.Debug);

            // Hook into the SaveLoaded event to inject mail
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            Monitor.Log("SaveLoaded event hooked", LogLevel.Debug);
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            Monitor.Log("OnDayStarted event triggered", LogLevel.Debug);

            // Check the current day and add mail if conditions are met
            if (Game1.Date.DayOfMonth == 1 && Game1.Date.Season == Season.Spring)
            {
                Monitor.Log("Added mail for spring_1", LogLevel.Debug);
                Game1.addMailForTomorrow("spring_1");
            }
            else if (Game1.Date.DayOfMonth == 1 && Game1.Date.Season == Season.Fall)
            {
                Game1.addMailForTomorrow("fall_1");
                Monitor.Log("Added mail for fall_1", LogLevel.Debug);
            }
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            Monitor.Log("OnSaveLoaded event triggered", LogLevel.Debug);

            // Inject custom mail into the game's mail data
            foreach (var mail in customMail)
            {
                if (!Game1.content.Load<Dictionary<string, string>>("Data\\Mail").ContainsKey(mail.Key))
                {
                    Game1.content.Load<Dictionary<string, string>>("Data\\Mail")[mail.Key] = mail.Value;
                    Monitor.Log($"Injected custom mail: {mail.Key}", LogLevel.Debug);
                }
            }
        }
    }
}