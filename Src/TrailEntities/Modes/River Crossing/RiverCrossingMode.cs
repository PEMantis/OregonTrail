﻿using System;
using TrailCommon;

namespace TrailEntities
{
    /// <summary>
    ///     Manages a boolean event where the player needs to make a choice before they can move onto the next location on the
    ///     trail. Depending on the outcome of this event the player party may lose items, people, or parts depending on how
    ///     bad it is.
    /// </summary>
    public sealed class RiverCrossingMode : GameMode<RiverCrossingCommands>, IRiverCrossingMode
    {
        private uint _depth;
        private uint _ferryCost;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:TrailEntities.GameMode" /> class.
        /// </summary>
        public RiverCrossingMode()
        {
            _depth = (uint) GameSimulationApp.Instance.Random.Next(1, 20);
            _ferryCost = (uint) GameSimulationApp.Instance.Random.Next(3, 8);
        }

        public uint Depth
        {
            get { return _depth; }
        }

        public uint FerryCost
        {
            get { return _ferryCost; }
        }

        public void CaulkVehicle()
        {
            throw new NotImplementedException();
        }

        public void Ford()
        {
            throw new NotImplementedException();
        }

        public void UseFerry()
        {
            throw new NotImplementedException();
        }

        public void UpdateVehicle()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Fired by game simulation system timers timer which runs on same thread, only fired for active (last added), or
        ///     top-most game mode.
        /// </summary>
        public override void TickMode()
        {
            throw new NotImplementedException();
        }

        public override ModeType ModeType
        {
            get { return ModeType.RiverCrossing; }
        }

        /// <summary>
        ///     Fired when this game mode is removed from the list of available and ticked modes in the simulation.
        /// </summary>
        public override void OnModeRemoved()
        {
            throw new NotImplementedException();
        }

        public void CrossRiver()
        {
            throw new NotImplementedException();
        }
    }
}