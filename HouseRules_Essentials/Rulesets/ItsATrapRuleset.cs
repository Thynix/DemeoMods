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
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.VortexLamp, IsReplenishable = false },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.VortexLamp, IsReplenishable = false },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.VortexLamp, IsReplenishable = false },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.VortexLamp, IsReplenishable = false },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.VortexLamp, IsReplenishable = false },
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
                { BoardPieceId.HeroBard, new List<AbilityKey> { AbilityKey.PoisonBomb, AbilityKey.MagicBarrier, AbilityKey.OilLamp, AbilityKey.GasLamp, AbilityKey.IceLamp, AbilityKey.TheBehemoth, AbilityKey.VortexLamp, AbilityKey.HealingPotion } },
                { BoardPieceId.HeroGuardian, new List<AbilityKey> { AbilityKey.PoisonBomb, AbilityKey.MagicBarrier, AbilityKey.Bone, AbilityKey.HealingPotion, AbilityKey.OilLamp, AbilityKey.GasLamp, AbilityKey.IceLamp, AbilityKey.VortexLamp } },
                { BoardPieceId.HeroHunter, new List<AbilityKey> { AbilityKey.PoisonBomb, AbilityKey.MagicBarrier, AbilityKey.HealingPotion, AbilityKey.PoisonedTip, AbilityKey.OilLamp, AbilityKey.GasLamp, AbilityKey.IceLamp } },
                { BoardPieceId.HeroRogue, new List<AbilityKey> { AbilityKey.PoisonBomb, AbilityKey.HealingPotion, AbilityKey.OilLamp, AbilityKey.GasLamp, AbilityKey.IceLamp, AbilityKey.RepeatingBallista } },
                { BoardPieceId.HeroSorcerer, new List<AbilityKey> { AbilityKey.PoisonBomb, AbilityKey.HealingPotion, AbilityKey.OilLamp, AbilityKey.GasLamp, AbilityKey.IceLamp, AbilityKey.TheBehemoth, AbilityKey.Vortex } },
            });

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Torch, Property = "VisionRange", Value = 40 },
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneHealingFountains", 0 },
                { "FloorOneLootChests", 11 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoLootChests", 14 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreeLootChests", 12 },
                { "FloorOneEndZoneSpikeMaxBudget", 12 },
                { "PacingSpikeSegmentFloorOneBudget", 12 },
            });

            var aoePotions = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.StrengthPotion, 1 },
                { AbilityKey.SwiftnessPotion, 1 },
                { AbilityKey.HealingPotion, 1 },
            });

            var abilityActionCostRule = new AbilityActionCostAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.BoobyTrap, false },
            });

            var lampTypesRule = new LampTypesOverriddenRule(new LampTypesOverriddenRule.LampConfig
            {
                Floor1Lamps = new List<BoardPieceId>
                {
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.HealingBeacon,
                    BoardPieceId.HealingBeacon,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                },
                Floor2Lamps = new List<BoardPieceId>
                {
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.HealingBeacon,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                },
                Floor3Lamps = new List<BoardPieceId>
                {
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.Torch,
                    BoardPieceId.HealingBeacon,
                    BoardPieceId.Torch,
                },
            });

            var piecePieceTypeRule = new PiecePieceTypeListOverriddenRule(new Dictionary<BoardPieceId, List<PieceType>>
            {
                { BoardPieceId.Torch, new List<PieceType> { PieceType.Prop, PieceType.UpdateFogOfWar, PieceType.ShowNameplate } },
                { BoardPieceId.HealingBeacon, new List<PieceType> { PieceType.Prop, PieceType.Bot, PieceType.ShowNameplate } },
            });

            var abilityMaxRangeRule = new AbilityMaxRangeOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.BoobyTrap, 5 },
                { AbilityKey.OilLamp, 5 },
                { AbilityKey.GasLamp, 5 },
                { AbilityKey.IceLamp, 5 },
                { AbilityKey.VortexLamp, 5 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                startingCardsRule,
                allowedCardsRule,
                piecesAdjustedRule,
                levelPropertiesRule,
                aoePotions,
                abilityActionCostRule,
                lampTypesRule,
                piecePieceTypeRule,
                abilityMaxRangeRule,
                new EnemyDoorOpeningDisabledRule(true));
        }
    }
}
