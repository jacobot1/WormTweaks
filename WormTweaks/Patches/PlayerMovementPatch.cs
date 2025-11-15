using HarmonyLib;

namespace WormTweaks.Patches
{
    [HarmonyPatch(typeof(PlayerMovement))]
    internal class PlayerMovementPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void PlayerSpeedPatch(PlayerMovement __instance)
        {
            if (WormTweaksMod.configPlayerSpeedMultiplier.Value != 1f)
            {
                __instance.walkSpeed *= WormTweaksMod.configPlayerSpeedMultiplier.Value;
                __instance.crouchSpeed *= WormTweaksMod.configPlayerSpeedMultiplier.Value;
                __instance.sprintSpeed *= WormTweaksMod.configPlayerSpeedMultiplier.Value;
                __instance.ghostMovementSpeed *= WormTweaksMod.configPlayerSpeedMultiplier.Value;
            }
        }
    }
}
