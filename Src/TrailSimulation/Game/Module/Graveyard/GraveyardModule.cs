﻿using System.Collections.Generic;
using TrailSimulation.Core;

namespace TrailSimulation.Game
{
    /// <summary>
    ///     Keeps track of all the tombstones in a nice collection and also supports saving them to disk and loading them again
    ///     using JSON. Finally it also has all the needed methods to check for a Tombstone at a particular spot on the
    ///     trail.
    /// </summary>
    public sealed class GraveyardModule : Module
    {
        /// <summary>
        ///     Tombstone for the player (or another dead player) that will be either viewed or shown to user so they can
        ///     confirm their graves details.
        /// </summary>
        private Tombstone _tempTombstone;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:TrailSimulation.Game.TombstoneManager" /> class.
        /// </summary>
        public GraveyardModule()
        {
            Tombstones = new Dictionary<int, Tombstone>();
            _tempTombstone = null;
        }

        /// <summary>
        ///     Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        ///     The element with the specified key.
        /// </returns>
        /// <param name="key">The key of the element to get or set. </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null. </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The property is set and the
        ///     <see cref="T:System.Collections.IDictionary" /> object is read-only.-or- The property is set,
        ///     <paramref name="key" /> does not exist in the collection, and the <see cref="T:System.Collections.IDictionary" />
        ///     has a fixed size.
        /// </exception>
        public Tombstone this[int key]
        {
            get { return Tombstones[key]; }
            set { Tombstones[key] = value; }
        }

        /// <summary>
        ///     References all of the currently loaded tombstones.
        /// </summary>
        private Dictionary<int, Tombstone> Tombstones { get; }

        /// <summary>
        ///     Creates a new internally workable tombstone that can be accessed by the running window and form logic. Intended to
        ///     be used to create a tombstone for a player that has failed to reach the end of the trail.
        /// </summary>
        public Tombstone TempTombstone
        {
            get
            {
                // Create a tombstone if one does not exist, otherwise reference existing one.
                return _tempTombstone ?? (_tempTombstone = new Tombstone());
            }
        }

        /// <summary>
        ///     Creates a shallow copy of the tombstone item and adds it to the list of tombstones. Does not check if it already
        ///     exists. Only safety is that multiple tombstones cannot be placed at the same mile marker.
        /// </summary>
        public void Add(Tombstone tombstoneItem)
        {
            // Clone the tombstone.
            var tombstoneClone = tombstoneItem.Clone() as Tombstone;

            // Skip if the cloning fails.
            if (tombstoneClone == null)
                return;

            // Check if we already have a tombstone at this mile marker.
            if (Tombstones.ContainsKey(tombstoneClone.MileMarker))
                return;

            // Actually adds the tombstone to the internal list of them using mile marker as a key.
            Tombstones.Add(tombstoneItem.MileMarker, tombstoneClone);
        }

        /// <summary>
        ///     Clears any existing tombstone data that might be loaded in the module.
        /// </summary>
        public void Reset()
        {
            Tombstones.Clear();
        }

        /// <summary>
        ///     Delegating method for the internal tombstones dictionary, used to make grabbing values less verbose.
        /// </summary>
        /// <param name="odometer">Number of miles the vehicle has traveled (to check for grave-site).</param>
        /// <param name="foundTombstone">Tombstone item returned from this mile marker if one exists, NULL if no tombstone found.</param>
        public void FindTombstone(int odometer, out Tombstone foundTombstone)
        {
            Tombstones.TryGetValue(odometer, out foundTombstone);
        }

        /// <summary>
        ///     Destroys any currently existing temporary tombstone.
        /// </summary>
        public void ClearTempTombstone()
        {
            _tempTombstone = null;
        }
    }
}