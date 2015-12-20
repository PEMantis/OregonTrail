﻿using System;
using System.Text;
using TrailSimulation.Core;

namespace TrailSimulation.Game
{
    /// <summary>
    ///     Form that is used to show data about a tombstone. It will look for a tombstone at the current vehicle odometer, if
    ///     none if found one will be generated by the user data.
    /// </summary>
    [ParentWindow(GameWindow.Tombstone)]
    public sealed class TombstoneView : InputForm<TombstoneInfo>
    {
        /// <summary>
        ///     This constructor will be used by the other one
        /// </summary>
        public TombstoneView(IWindow window) : base(window)
        {
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game Windows and would like to have a string returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Check if the tombstone manager returned anything, if not then check for user data it's player death then.
            var _tombstone = new StringBuilder();

            // Finding a tombstone at the current vehicle odometer means we use that reference.
            if (GameSimulationApp.Instance.Vehicle.PassengerLivingCount > 0)
            {
                // Grab the current Tombstone based on players progress on the trail so far.
                Tombstone foundTombstone;
                GameSimulationApp.Instance.Graveyard.FindTombstone(
                    GameSimulationApp.Instance.Vehicle.Odometer,
                    out foundTombstone);

                _tombstone.AppendLine($"{Environment.NewLine}{foundTombstone}");
            }
            else
            {
                _tombstone.AppendLine($"{Environment.NewLine}{UserData.Tombstone}");

                // Adds the underlying reason for the games failure if it was not obvious to the player by now.
                _tombstone.AppendLine("All the members of");
                _tombstone.AppendLine("your party have");
                _tombstone.AppendLine($"died.{Environment.NewLine}");
            }

            // Write out the tombstone text and epitaph message to the game window.
            return _tombstone.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Determine if we are showing the player a tombstone because they died.
            if (GameSimulationApp.Instance.Vehicle.PassengerLivingCount <= 0)
            {
                // Completely resets the game to default state it was in when it first started.
                UserData.ClearTombstone();
                GameSimulationApp.Instance.Restart();
                return;
            }

            // Return to travel mode menu if we are just looking at some other dead guy grave.
            ParentWindow.RemoveWindowNextTick();
        }
    }
}