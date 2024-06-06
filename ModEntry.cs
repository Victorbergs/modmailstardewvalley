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
            // Hook into the DayStarted event
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            Monitor.Log("DayStarted event hooked", LogLevel.Debug);

            // Hook into the SaveLoaded event to inject mail
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            Monitor.Log("SaveLoaded event hooked", LogLevel.Debug);
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
        }
        
        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/mail"))
            {
                e.Edit(this.EditImpl);
            }
        }
        public void EditImpl(IAssetData asset)
        {
            // Log when Entry is called
            Monitor.Log("Entry method called", LogLevel.Debug);

            try
            {
            Monitor.Log("Entry method called3", LogLevel.Debug);
            customMail = asset.AsDictionary<string, string>().Data;

            // "MyModMail1" is referred to as the mail Id.  It is how you will uniquely identify and reference your mail.
            // The @ will be replaced with the player's name.  Other items do not seem to work (''i.e.,'' %pet or %farm)
            // %item object 388 50 %%   - this adds 50 pieces of wood when added to the end of a letter.
            // %item tools Axe Hoe %%   - this adds tools; may list any of Axe, Hoe, Can, Scythe, and Pickaxe
            // %item money 250 601  %%  - this sends a random amount of gold from 250 to 601 inclusive.
            // For more details, see: https://stardewvalleywiki.com/Modding:Mail_data 
            customMail["MyModMail1"] = "PLACEHOLDER";
            customMail["MyModMail2"] = "PLACEHOLDER";
            customMail["MyModMail3"] = "PLACEHOLDER";
            customMail["MyModMail4"] = "PLACEHOLDER";
            customMail["MyModMail5"] = "PLACEHOLDER";
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed to load custom mail data: {ex.Message}", LogLevel.Error);
                return;
            }
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            Monitor.Log("OnDayStarted event triggered", LogLevel.Debug);

            // Check the current day and add mail if conditions are met
            if (Game1.Date.DayOfMonth == 1 && Game1.Date.Season == Season.Spring)
            {
                Monitor.Log("Added mail for spring_1", LogLevel.Debug);
                Game1.player.mailbox.Add("MyModMail1");
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