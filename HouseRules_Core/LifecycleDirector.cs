﻿namespace HouseRules
{
    using System;
    using System.Reflection;
    using System.Text;
    using Boardgame;
    using Boardgame.Networking;
    using HarmonyLib;
    using HouseRules.Types;

    internal static class LifecycleDirector
    {
        private const float WelcomeMessageDurationSeconds = 30f;

        private static GameContext _gameContext;
        private static bool _isCreatingGame;
        private static bool _isLoadingGame;

        internal static bool IsRulesetActive { get; private set; }

        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(LifecycleDirector), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: AccessTools
                    .Inner(typeof(GameStateMachine), "CreatingGameState").GetTypeInfo()
                    .GetDeclaredMethod("OnJoinedRoom"),
                prefix: new HarmonyMethod(typeof(LifecycleDirector), nameof(CreatingGameState_OnJoinedRoom_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "GoToPlayingState"),
                postfix: new HarmonyMethod(typeof(LifecycleDirector), nameof(GameStateMachine_GoToPlayingState_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "GoToShoppingState"),
                postfix: new HarmonyMethod(typeof(LifecycleDirector), nameof(GameStateMachine_GoToShoppingState_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(PostGameControllerBase), "OnPlayAgainClicked"),
                postfix: new HarmonyMethod(
                    typeof(LifecycleDirector),
                    nameof(PostGameControllerBase_OnPlayAgainClicked_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "EndGame"),
                prefix: new HarmonyMethod(typeof(LifecycleDirector), nameof(GameStateMachine_EndGame_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "DisconnectLocalPlayer"),
                prefix: new HarmonyMethod(
                    typeof(LifecycleDirector),
                    nameof(SerializableEventQueue_DisconnectLocalPlayer_Prefix)));
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            var gameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
            _gameContext = gameContext;
        }

        private static void CreatingGameState_OnJoinedRoom_Prefix()
        {
            if (_gameContext.gameStateMachine.goBackToMenuState)
            {
                return;
            }

            var createGameMode = Traverse.Create(_gameContext.gameStateMachine)
                .Field<CreateGameMode>("createGameMode").Value;
            if (createGameMode != CreateGameMode.Private)
            {
                return;
            }

            var createdGameFromSave =
                Traverse.Create(_gameContext.gameStateMachine).Field<bool>("createdGameFromSave").Value;
            if (createdGameFromSave)
            {
                _isLoadingGame = true;
            }
            else
            {
                _isCreatingGame = true;
            }

            var levelSequence = Traverse.Create(_gameContext.gameStateMachine).Field<LevelSequence>("levelSequence").Value;
            MotherbrainGlobalVars.CurrentConfig = levelSequence.gameConfig;

            ActivateRuleset();
            OnPreGameCreated();
        }

        private static void GameStateMachine_GoToPlayingState_Postfix()
        {
            if (!_isCreatingGame)
            {
                return;
            }

            _isCreatingGame = false;
            OnPostGameCreated();
            ShowWelcomeMessage();
        }

        private static void GameStateMachine_GoToShoppingState_Postfix()
        {
            if (!_isLoadingGame)
            {
                return;
            }

            _isLoadingGame = false;
            OnPostGameCreated();
            ShowWelcomeMessage();
        }

        private static void PostGameControllerBase_OnPlayAgainClicked_Postfix()
        {
            ActivateRuleset();
            _isCreatingGame = true;
            OnPreGameCreated();
        }

        private static void GameStateMachine_EndGame_Prefix()
        {
            DeactivateRuleset();
        }

        private static void SerializableEventQueue_DisconnectLocalPlayer_Prefix()
        {
            DeactivateRuleset();
        }

        private static void ActivateRuleset()
        {
            if (IsRulesetActive)
            {
                HR.Logger.Warning("Ruleset activation was attempted whilst a ruleset was already activated. This should not happen. Please report this to HouseRules developers.");
                return;
            }

            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (GameHub.GetGameMode == GameHub.GameMode.Multiplayer && !HR.SelectedRuleset.IsSafeForMultiplayer)
            {
                HR.Logger.Warning($"The selected ruleset [{HR.SelectedRuleset.Name}] is not safe for multiplayer games. Skipping activation.");
                return;
            }

            IsRulesetActive = true;

            HR.Logger.Msg($"Activating ruleset: {HR.SelectedRuleset.Name} (with {HR.SelectedRuleset.Rules.Count} rules)");
            foreach (var rule in HR.SelectedRuleset.Rules)
            {
                try
                {
                    HR.Logger.Msg($"Activating rule type: {rule.GetType()}");
                    rule.OnActivate(_gameContext);
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    HR.Logger.Warning($"Failed to activate rule [{rule.GetType()}]: {e}");
                }
            }
        }

        private static void DeactivateRuleset()
        {
            if (!IsRulesetActive)
            {
                return;
            }

            IsRulesetActive = false;

            HR.Logger.Msg($"Deactivating ruleset: {HR.SelectedRuleset.Name} (with {HR.SelectedRuleset.Rules.Count} rules)");
            foreach (var rule in HR.SelectedRuleset.Rules)
            {
                try
                {
                    HR.Logger.Msg($"Deactivating rule type: {rule.GetType()}");
                    rule.OnDeactivate(_gameContext);
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    HR.Logger.Warning($"Failed to deactivate rule [{rule.GetType()}]: {e}");
                }
            }
        }

        private static void OnPreGameCreated()
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (!IsRulesetActive)
            {
                return;
            }

            foreach (var rule in HR.SelectedRuleset.Rules)
            {
                try
                {
                    HR.Logger.Msg($"Calling OnPreGameCreated for rule type: {rule.GetType()}");
                    rule.OnPreGameCreated(_gameContext);
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    HR.Logger.Warning($"Failed to successfully call OnPreGameCreated on rule [{rule.GetType()}]: {e}");
                }
            }
        }

        private static void OnPostGameCreated()
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (!IsRulesetActive)
            {
                return;
            }

            foreach (var rule in HR.SelectedRuleset.Rules)
            {
                try
                {
                    HR.Logger.Msg($"Calling OnPostGameCreated for rule type: {rule.GetType()}");
                    rule.OnPostGameCreated(_gameContext);
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    HR.Logger.Warning($"Failed to successfully call OnPostGameCreated on rule [{rule.GetType()}]: {e}");
                }
            }
        }

        private static void ShowWelcomeMessage()
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (!IsRulesetActive)
            {
                GameUI.ShowCameraMessage(NotSafeForMultiplayerMessage(), WelcomeMessageDurationSeconds);
                return;
            }

            GameUI.ShowCameraMessage(RulesetActiveMessage(), WelcomeMessageDurationSeconds);
        }

        private static string NotSafeForMultiplayerMessage()
        {
            return new StringBuilder()
                .AppendLine("Attention:")
                .AppendLine("The HouseRules ruleset you selected is not safe for multiplayer games, and was not activated.")
                .ToString();
        }

        private static string RulesetActiveMessage()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Welcome to a game using HouseRules!");
            sb.AppendLine();
            sb.AppendLine($"{HR.SelectedRuleset.Name}:");
            sb.AppendLine(HR.SelectedRuleset.Description);
            sb.AppendLine();
            sb.AppendLine("Rules:");

            for (var i = 0; i < HR.SelectedRuleset.Rules.Count; i++)
            {
                var description = HR.SelectedRuleset.Rules[i].Description;
                sb.AppendLine($"{i + 1}. {description}");
            }

            return sb.ToString();
        }
    }
}
