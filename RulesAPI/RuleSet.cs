﻿namespace RulesAPI
{
    using System;
    using System.Collections.Generic;

    public abstract class RuleSet
    {
        /// <summary>
        /// Gets the description of the ruleset.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the rules of the ruleset.
        /// </summary>
        protected internal abstract HashSet<Rule> Rules { get; }

        internal void Activate()
        {
            RulesAPIMod.Logger.Msg($"Activating ruleset: {GetType()} (with {Rules.Count} rules)");

            foreach (var rule in Rules)
            {
                try
                {
                    rule.Activate();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Rollback activation.
                    RulesAPIMod.Logger.Warning($"Failed to activate rule [{rule.GetType()}]: {e}");
                }
            }
        }

        internal void Deactivate()
        {
            RulesAPIMod.Logger.Msg($"Deactivating ruleset: {GetType()} (with {Rules.Count} rules)");
            foreach (var rule in Rules)
            {
                try
                {
                    rule.Deactivate();
                }
                catch (Exception e)
                {
                    RulesAPIMod.Logger.Warning($"Failed to deactivate rule [{rule.GetType()}]: {e}");
                }
            }
        }
    }
}