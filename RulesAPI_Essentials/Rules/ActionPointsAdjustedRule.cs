﻿namespace RulesAPI.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using HarmonyLib;
    using UnityEngine;

    public sealed class ActionPointsAdjustedRule : Rule, IConfigWritable<Dictionary<string, int>>
    {
        public override string Description => "Action points are adjusted";

        private readonly Dictionary<string, int> _adjustments;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionPointsAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of an entity to the number of action points
        /// added to their base. Negative numbers are allowed.</param>
        public ActionPointsAdjustedRule(Dictionary<string, int> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<string, int> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated()
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item.Key}"));
                var property = Traverse.Create(pieceConfig).Property<int>("ActionPoint");
                property.Value += item.Value;
            }
        }

        protected override void OnDeactivate()
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            foreach (var item in _adjustments)
            {
                var pieceConfig = pieceConfigs.First(c => c.name.Equals($"PieceConfig_{item.Key}"));
                var property = Traverse.Create(pieceConfig).Property<int>("ActionPoint");
                property.Value -= item.Value;
            }
        }
    }
}