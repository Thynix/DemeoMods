﻿namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HouseRules.Types;

    public sealed class AbilityMaxRangeOverriddenRule : Rule, IConfigWritable<Dictionary<AbilityKey, int>>, IMultiplayerSafe
    {
        public override string Description => "Ability Max Range is adjusted";

        private readonly Dictionary<AbilityKey, int> _adjustments;
        private Dictionary<AbilityKey, int> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityMaxRangeOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of an entity to the maxRange</param>
        public AbilityMaxRangeOverriddenRule(Dictionary<AbilityKey, int> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, int>();
        }

        public Dictionary<AbilityKey, int> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceAbilities(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceAbilities(_originals);
        }

        private static Dictionary<AbilityKey, int> ReplaceAbilities(Dictionary<AbilityKey, int> replacements)
        {
            var originals = new Dictionary<AbilityKey, int>();

            foreach (var replacement in replacements)
            {
                if (!AbilityFactory.TryGetAbility(replacement.Key, out var ability))
                {
                    throw new InvalidOperationException($"AbilityKey [{replacement.Key}] does not have a corresponding ability.");
                }

                originals[replacement.Key] = ability.maxRange;
                ability.maxRange = replacement.Value;
            }

            return originals;
        }
    }
}
