﻿namespace Common
{
    using Bowser.Core;
    using MelonLoader;

    internal static class CommonModule
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("Common");

        internal static BowserButtonHandler HangoutsButtonHandler { get; set; }

        internal static bool IsInitialized { get; private set; }

        /// <summary>
        /// Initialize the module. This should be called during the dependant module's OnApplicationStart().
        /// </summary>
        public static void Initialize()
        {
            var harmony = new HarmonyLib.Harmony("com.orendain.demeomods.common");
            ModPatcher.Patch(harmony);
            IsInitialized = true;
        }
    }
}
