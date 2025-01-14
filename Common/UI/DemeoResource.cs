﻿namespace Common.UI
{
    using System;
    using System.Linq;
    using TMPro;
    using UnityEngine;

    internal class DemeoResource
    {
        private static DemeoResource _instance;

        public Color ColorBrown { get; } = new Color(0.0392f, 0.0157f, 0, 1);

        public Color ColorBeige { get; } = new Color(0.878f, 0.752f, 0.384f, 1);

        public Component VrLobbyTableAnchor { get; private set; }

        public GameObject HangoutsTableAnchor { get; private set; }

        public TMP_FontAsset Font { get; private set; }

        public TMP_ColorGradient FontColorGradient { get; private set; }

        public Mesh ButtonMeshBlue { get; private set; }

        public Mesh ButtonMeshBrown { get; private set; }

        public Mesh ButtonMeshRed { get; private set; }

        public Material ButtonMaterial { get; private set; }

        public Material ButtonHoverMaterial { get; private set; }

        public Mesh MenuBoxMesh { get; private set; }

        public Material MenuBoxMaterial { get; private set; }

        public static DemeoResource Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            if (!IsReady())
            {
                throw new InvalidOperationException("Demeo UI resources not yet available.");
            }

            _instance = new DemeoResource();
            return _instance;
        }

        private DemeoResource()
        {
            Initialize();
        }

        /// <summary>
        /// Returns true when all required Demeo resources are found and accounted for.
        /// </summary>
        public static bool IsReady()
        {
            return Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Any(x => x.name == "Demeo SDF")
                   && Resources.FindObjectsOfTypeAll<TMP_ColorGradient>().Any(x => x.name == "Demeo - Main Menu Buttons")
                   && Resources.FindObjectsOfTypeAll<Mesh>().Any(x => x.name == "UIMenuMainButton")
                   && Resources.FindObjectsOfTypeAll<Material>().Any(x => x.name == "MainMenuMat")
                   && Resources.FindObjectsOfTypeAll<Material>().Any(x => x.name == "MainMenuHover")
                   && Resources.FindObjectsOfTypeAll<Mesh>().Any(x => x.name == "MenuBox_SettingsButton")
                   && Resources.FindObjectsOfTypeAll<Material>().Any(x => x.name == "MainMenuMat (Instance)")
                   && IsAnchorReady();
        }

        private static bool IsAnchorReady()
        {
            return Resources.FindObjectsOfTypeAll<charactersoundlistener>().Count(x => x.name == "MenuBox_BindPose") > 1
                   || Resources.FindObjectsOfTypeAll<GameObject>().Any(x => x.name == "GroupLaunchTable");
        }

        public void Initialize()
        {
            Font = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First(x => x.name == "Demeo SDF");
            FontColorGradient = Resources
                .FindObjectsOfTypeAll<TMP_ColorGradient>()
                .First(x => x.name == "Demeo - Main Menu Buttons");
            ButtonMeshBlue = Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "UIMainButtonBlue");
            ButtonMeshBrown = Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "UIMainButtonBrown");
            ButtonMeshRed = Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "UIMainButtonRed");
            ButtonMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuMat");
            ButtonHoverMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuHover");
            MenuBoxMesh = Resources.FindObjectsOfTypeAll<Mesh>().First(x => x.name == "MenuBox_SettingsButton");
            MenuBoxMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "MainMenuMat (Instance)");

            InitializeAnchors();
        }

        private void InitializeAnchors()
        {
            VrLobbyTableAnchor = Resources.FindObjectsOfTypeAll<charactersoundlistener>()
                .FirstOrDefault(x => x.name == "MenuBox_BindPose");
            HangoutsTableAnchor = Resources.FindObjectsOfTypeAll<GameObject>()
                .FirstOrDefault(x => x.name == "GroupLaunchTable");
        }
    }
}
