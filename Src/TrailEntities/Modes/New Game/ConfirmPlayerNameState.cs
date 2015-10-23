﻿using System;
using TrailCommon;

namespace TrailEntities
{
    /// <summary>
    ///     Confirms if a particular index in the user data player name list is what the player desires. Since we offer them
    ///     the ability to input this data we also offer them a chance to confirm it, if they say no it will bounce the state
    ///     back to the name entry for that index.
    /// </summary>
    public sealed class ConfirmPlayerNameState : ModeState<NewGameInfo>
    {
        private readonly int _playerNameIndex;

        /// <summary>
        ///     This constructor will be used by the other one
        /// </summary>
        public ConfirmPlayerNameState(int playerNameIndex, IMode gameMode, NewGameInfo userData)
            : base(gameMode, userData)
        {
            _playerNameIndex = playerNameIndex;
        }

        /// <summary>
        ///     Returns a text only representation of the current game mode state. Could be a statement, information, question
        ///     waiting input, etc.
        /// </summary>
        public override string GetStateTUI()
        {
            return $"You entered {UserData.PlayerNames[_playerNameIndex]} for " +
                   $"player slot {_playerNameIndex + 1}.\n Does this look correct? Y/N";
        }

        /// <summary>
        ///     Fired when the game mode current state is not null and input buffer does not match any known command.
        /// </summary>
        /// <param name="input">Contents of the input buffer which didn't match any known command in parent game mode.</param>
        public override void OnInputBufferReturned(string input)
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (input.ToUpperInvariant() == "Y")
            {
                if (_playerNameIndex < 3)
                {
                    // Depending on what index we are confirming for we might move onto party names confirmation, or just the next name in the list.
                    SetNameSelectionStateByIndex(false);
                }
                else if (_playerNameIndex >= 3)
                {
                    // Instead of throwing an exception when out of bounds we just goto confirm the party since you must have filled them all out.
                    Mode.CurrentState = new ConfirmGroupNamesState(Mode, UserData);
                }
            }
            else if (input.ToUpperInvariant() == "N")
            {
                SetNameSelectionStateByIndex(true);
            }
        }

        /// <summary>
        ///     Allows the user to change what they entered then restart entry for that name.
        /// </summary>
        /// <param name="retrying">Determines if this selection state change is a retry or progression to next state.</param>
        private void SetNameSelectionStateByIndex(bool retrying)
        {
            // Add one to the selection state if we are advancing the state forward and not retrying.
            var correctedPlayerNameIndex = _playerNameIndex;
            if (!retrying)
                correctedPlayerNameIndex++;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (correctedPlayerNameIndex)
            {
                case 0:
                    Mode.CurrentState = new InputPlayerNameState(correctedPlayerNameIndex, "Party leader name?", Mode,
                        UserData);
                    break;
                case 1:
                    Mode.CurrentState = new InputPlayerNameState(correctedPlayerNameIndex, "Party member two name?",
                        Mode, UserData);
                    break;
                case 2:
                    Mode.CurrentState = new InputPlayerNameState(correctedPlayerNameIndex, "Party member three name ?",
                        Mode, UserData);
                    break;
                case 3:
                    Mode.CurrentState = new InputPlayerNameState(correctedPlayerNameIndex, "Party member four name?",
                        Mode, UserData);
                    break;
                default:
                    if (!retrying)
                    {
                        Mode.CurrentState = new ConfirmGroupNamesState(Mode, UserData);
                    }
                    else
                    {
                        throw new ArgumentException(
                            "Attempted to set game mode state party by index but was set to retry and higher than total player count!");
                    }
                    break;
            }
        }
    }
}