﻿namespace Rules
{
    using System.Collections.Generic;
    using MelonLoader;

    internal class RulesMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("Rules");

        public override void OnApplicationStart()
        {
            RegisterNewRuleTypes();
            RegisterNewRulesets();
        }

        private static void RegisterNewRuleTypes()
        {
            var registrar = RulesAPI.Registrar.Instance();
            registrar.Register(typeof(Rule.SampleRule));
            registrar.Register(typeof(Rule.ActionPointsAdjustedRule));
            registrar.Register(typeof(Rule.BallistaAttackDamageAdjustedRule));
            registrar.Register(typeof(Rule.RatNestsSpawnGoldRule));
            registrar.Register(typeof(Rule.ZapDamageAdjustedRule));
            registrar.Register(typeof(Rule.ZapStartingInventoryAdjustedRule));
        }

        private static void RegisterNewRulesets()
        {
            var sampleRules = new HashSet<RulesAPI.Rule> { new Rule.SampleRule() };
            var sampleRuleset = RulesAPI.Ruleset.NewInstance("SampleRuleset", "Just a sample ruleset.", sampleRules);

            var registrar = RulesAPI.Registrar.Instance();
            registrar.Register(sampleRuleset);
        }
    }
}
