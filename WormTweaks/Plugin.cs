using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;
using WormTweaks.Patches;

namespace WormTweaks
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class WormTweaksMod : BaseUnityPlugin
    {
        // Mod metadata
        public const string modGUID = "com.jacobot5.wormtweaks";
        public const string modName = "WormTweaks";
        public const string modVersion = "1.0.0";

        // Configuration
        public static ConfigEntry<float> configPlayerSpeedMultiplier;
        public static ConfigEntry<float> configPlayerYeet;
        public static ConfigEntry<float> configWormSpeedMultiplier;
        public static ConfigEntry<float> configGlobalGravityMultiplier;
        public static ConfigEntry<float> configDynamiteRadiusMultiplier;

        // Initalize Harmony
        private readonly Harmony harmony = new Harmony(modGUID);

        // Create static instance
        private static WormTweaksMod Instance;

        // Initialize logging
        public static ManualLogSource mls;

        private void Awake()
        {
            // Ensure static instance
            if (Instance == null)
            {
                Instance = this;
            }

            // Send alive message
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("WormTweaks has awoken.");

            // Bind configuration
            configPlayerSpeedMultiplier = Config.Bind("Player",
                                                "PlayerSpeedMultiplier",
                                                1f,
                                                "Speed multiplier for Pardner players. Default is 1");
            
            configPlayerYeet = Config.Bind("Player",
                                                "PlayerYeet",
                                                1f,
                                                "Object throw speed multiplier for Pardner players. Default is 1");
            
            configWormSpeedMultiplier = Config.Bind("Worm",
                                                "WormSpeedMultiplier",
                                                1f,
                                                "Speed multiplier for Worm players. Default is 1.");

            configGlobalGravityMultiplier = Config.Bind("Global",
                                                "GlobalGravity",
                                                1f,
                                                "Gravity multiplier for all objects using default gravity. Default is 1.");

            configDynamiteRadiusMultiplier = Config.Bind("Dynamite",
                                                "DynamiteRadiusMultiplier",
                                                1f,
                                                "Multiplied by defualt dynamite radius to change the size of the explosion");

            // Do the patching
            harmony.PatchAll(typeof(WormTweaksMod));
            harmony.PatchAll(typeof(PlayerMovementPatch));
            harmony.PatchAll(typeof(GameManagerPatch));
            harmony.PatchAll(typeof(WormMovementPatch));
        }
    }
}
