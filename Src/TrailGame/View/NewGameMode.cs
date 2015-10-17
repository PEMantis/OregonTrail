﻿using System;
using System.Collections.Generic;
using TrailCommon;
using TrailEntities;

namespace TrailGame
{
    public class NewGameMode : GameMode, ISimulationInitializer
    {
        private bool _hasChosenNames;
        private bool _hasChosenProfession;
        private bool _hasChosenStartingItems;
        private bool _hasStartedGame;
        private List<string> _playerNames;
        private Profession _playerProfession;
        private Vehicle _playerVehicle;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:TrailEntities.NewGameMode" /> class.
        /// </summary>
        public NewGameMode(Vehicle vehicle) : base(vehicle)
        {
            _playerVehicle = vehicle;
        }

        public override SimulationMode Mode
        {
            get { return SimulationMode.NewGame; }
        }

        public void ChooseProfession()
        {
            _playerProfession = Profession.Banker;
            var professionCount = 1;

            Console.Clear();
            Console.WriteLine("What profession is " + _playerNames[0] + "?");

            foreach (var possibleProfession in Enum.GetValues(typeof (Profession)))
            {
                Console.WriteLine(professionCount + "). " + possibleProfession);
                professionCount++;
            }

            switch (Console.ReadKey(true).KeyChar.ToString())
            {
                case "1":
                    _playerProfession = Profession.Banker;
                    break;
                case "2":
                    _playerProfession = Profession.Carpenter;
                    break;
                case "3":
                    _playerProfession = Profession.Farmer;
                    break;
                default:
                    ChooseProfession();
                    break;
            }

            Console.WriteLine("You selected " + _playerProfession);
            Console.WriteLine("Does this look correct? Y/N");
            var professionCorrectResponse = Console.ReadLine();
            if (!string.IsNullOrEmpty(professionCorrectResponse) &&
                !string.IsNullOrWhiteSpace(professionCorrectResponse))
            {
                professionCorrectResponse = professionCorrectResponse.Trim().ToLowerInvariant();
                if (professionCorrectResponse.Equals("n"))
                {
                    _playerProfession = Profession.Banker;
                    ChooseProfession();
                }
            }
        }

        public void BuyInitialItems()
        {
            //SimulationApp.Instance.SetMode(new Store("Matt's General Store", 9000, ));
        }

        public void ChooseNames()
        {
            _playerNames = new List<string>();
            Console.Clear();
            Console.WriteLine("Party leader name?");
            var playerName = GetPlayerName();
            _playerNames.Add(playerName);
            Console.WriteLine("Added: " + playerName);

            Console.WriteLine("Party member two name?");
            playerName = GetPlayerName();
            _playerNames.Add(playerName);
            Console.WriteLine("Added: " + playerName);

            Console.WriteLine("Party member three name?");
            playerName = GetPlayerName();
            _playerNames.Add(playerName);
            Console.WriteLine("Added: " + playerName);

            Console.WriteLine("Party member four name?");
            playerName = GetPlayerName();
            _playerNames.Add(playerName);
            Console.WriteLine("Added: " + playerName);

            Console.WriteLine("Your Party Members:");
            var crewNumber = 1;
            foreach (var name in _playerNames)
            {
                var isLeader = _playerNames.IndexOf(name) == 0;
                if (isLeader)
                {
                    Console.WriteLine(crewNumber + ")." + name + " (leader)");
                }
                else
                {
                    Console.WriteLine(crewNumber + ")." + name);
                }
                crewNumber++;
            }

            Console.WriteLine("Does this look correct? Y/N");
            var namesCorrectResponse = Console.ReadLine();
            if (!string.IsNullOrEmpty(namesCorrectResponse) &&
                !string.IsNullOrWhiteSpace(namesCorrectResponse))
            {
                namesCorrectResponse = namesCorrectResponse.Trim().ToLowerInvariant();
                if (namesCorrectResponse.Equals("n"))
                {
                    _playerNames.Clear();
                    ChooseNames();
                }
            }
        }

        public void StartGame()
        {
            Console.WriteLine("Adding " + _playerNames.Count + " people to vehicle...");
            foreach (var name in _playerNames)
            {
                // First name in list in leader.
                var isLeader = _playerNames.IndexOf(name) == 0;
                _playerVehicle.AddPerson(new Person(_playerProfession, name, isLeader));
            }
        }

        protected override void OnTick()
        {
            base.OnTick();

            // Every new game has you picking names, profession, and starting items.
            if (!_hasChosenNames && !_hasChosenProfession && !_hasChosenStartingItems && !_hasStartedGame)
            {
                _hasChosenNames = true;
                ChooseNames();
            }
            else if (_hasChosenNames && !_hasChosenProfession && !_hasChosenStartingItems && !_hasStartedGame)
            {
                _hasChosenProfession = true;
                ChooseProfession();
            }
            else if (_hasChosenNames && _hasChosenProfession && !_hasChosenStartingItems && !_hasStartedGame)
            {
                _hasChosenStartingItems = true;
                BuyInitialItems();
            }
            else if (_hasChosenNames && _hasChosenProfession && _hasChosenStartingItems && !_hasStartedGame)
            {
                _hasStartedGame = true;
                StartGame();
            }
        }

        private static string GetPlayerName()
        {
            var readLine = Console.ReadLine();
            if (readLine != null)
            {
                readLine = readLine.Trim();
                if (!string.IsNullOrEmpty(readLine) &&
                    !string.IsNullOrWhiteSpace(readLine))
                {
                    return readLine;
                }
            }

            // Just return a random name if there is invalid input.
            string[] names = {"Bob", "Joe", "Sally", "Tim", "Steve"};
            return names[SimulationApp.Instance.Random.Next(names.Length)];
        }
    }
}