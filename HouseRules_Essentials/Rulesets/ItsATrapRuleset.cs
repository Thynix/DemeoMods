namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class ItsATrapRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "It's A Trap!";
            const string description = "All the tools you need to build traps for your enemies, but not much else.";

            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, IsReplenishable = true },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, IsReplenishable = true },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, IsReplenishable = true },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = true },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, IsReplenishable = true },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroBard, bardCards },
                { BoardPieceId.HeroGuardian, guardianCards },
                { BoardPieceId.HeroHunter, hunterCards },
                { BoardPieceId.HeroRogue, assassinCards },
                { BoardPieceId.HeroSorcerer, sorcererCards },
            });

            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.HeroBard, new List<AbilityKey> { AbilityKey.PoisonBomb, AbilityKey.MagicBarrier, AbilityKey.OilLamp, AbilityKey.GasLamp, AbilityKey.IceLamp, AbilityKey.TheBehemoth, AbilityKey.RepeatingBallista } },
                { BoardPieceId.HeroGuardian, new List<AbilityKey> { AbilityKey.PoisonBomb, AbilityKey.MagicBarrier, AbilityKey.Bone, AbilityKey.Barricade, AbilityKey.OilLamp, AbilityKey.GasLamp, AbilityKey.IceLamp } },
                { BoardPieceId.HeroHunter, new List<AbilityKey> { AbilityKey.PoisonBomb, AbilityKey.MagicBarrier, AbilityKey.Fireball, AbilityKey.PoisonedTip, AbilityKey.OilLamp, AbilityKey.GasLamp, AbilityKey.IceLamp } },
                { BoardPieceId.HeroRogue, new List<AbilityKey> { AbilityKey.PoisonBomb, AbilityKey.Fireball, AbilityKey.Blink, AbilityKey.OilLamp, AbilityKey.GasLamp, AbilityKey.IceLamp, AbilityKey.RepeatingBallista } },
                { BoardPieceId.HeroSorcerer, new List<AbilityKey> { AbilityKey.PoisonBomb, AbilityKey.Fireball, AbilityKey.SummonElemental, AbilityKey.OilLamp, AbilityKey.GasLamp, AbilityKey.IceLamp, AbilityKey.TheBehemoth } },
            });

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 30 },
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneHealingFountains", 2 },
                { "FloorOneLootChests", 11 },
                { "FloorTwoHealingFountains", 4 },
                { "FloorTwoLootChests", 14 },
                { "FloorThreeHealingFountains", 4 },
                { "FloorThreeLootChests", 12 },
            });

            var aoePotions = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.StrengthPotion, 1 },
                { AbilityKey.SwiftnessPotion, 1 },
            });
            return Ruleset.NewInstance(
                name,
                description,
                startingCardsRule,
                allowedCardsRule,
                piecesAdjustedRule,
                levelPropertiesRule,
                aoePotions);
        }
    }
}
