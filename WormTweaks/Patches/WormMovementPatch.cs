using HarmonyLib;

namespace WormTweaks.Patches
{
    [HarmonyPatch(typeof(WormMovement))]
    internal class WormMovementPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void WormSpeedPatch(WormMovement __instance)
        {
            if (WormTweaksMod.configWormSpeedMultiplier.Value != 1f)
            {
                __instance.surfaceSpeed *= WormTweaksMod.configWormSpeedMultiplier.Value;
                __instance.surfaceSpeed_Sneak *= WormTweaksMod.configWormSpeedMultiplier.Value;
            }
        }
    }
}
