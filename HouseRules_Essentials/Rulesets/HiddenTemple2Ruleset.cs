﻿namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class HiddenTemple2Ruleset
    {
        internal static Ruleset Create()
        {
            const string name = "Hidden Temple: II";
            const string description = "Explore the legends of the second hidden temple.";

            var levelSequenceOverriddenRule = new LevelSequenceOverriddenRule(new List<string>
            {
                "ElvenFloor14",//small
                "ShopFloor02",
                "ElvenFloor16",//med
                "ShopFloor02",
                "ElvenFloor17",//med-large
            });


            return Ruleset.NewInstance(
                name,
                description,
                levelSequenceOverriddenRule);
        }
    }
}
