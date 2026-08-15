using HarmonyLib;
using static DownedAwareness.Plugin;

namespace DownedAwareness.Patches;

[HarmonyPatch(typeof(Character))]
static class CharacterPatch
{
    [HarmonyPatch("RPCA_PassOut")]
    [HarmonyPostfix]
    private static void RPCA_PassOut_Postfix(Character __instance)
    {
        POM.CreateInstance(__instance);
        Log.LogDebug($"PassedOut {__instance.name}");
    }

    [HarmonyPatch("RPCA_UnPassOut")]
    [HarmonyPostfix]
    private static void RPCA_UnPassOut_Postfix(Character __instance)
    {
        POM.RemoveInstance(__instance);
        Log.LogDebug($"UnPassedOut {__instance.name}");
    }

    [HarmonyPatch("RPCA_ReviveAtPosition")]
    [HarmonyPostfix]
    private static void RPCA_ReviveAtPosition_Postfix(Character __instance)
    {
        POM.RemoveInstance(__instance);
        Log.LogDebug($"Revived {__instance.name}");
    }

    [HarmonyPatch("RPCA_Die")]
    [HarmonyPostfix]
    private static void RPCA_Die_Postfix(Character __instance)
    {
        POM.RemoveInstance(__instance, true);
        Log.LogDebug($"Died {__instance.name}");
    }

    [HarmonyPatch("OnDestroy")]
    [HarmonyPostfix]
    private static void OnDestroy_Postfix(Character __instance)
    {
        POM.RemoveInstance(__instance);
        Log.LogDebug($"Destroyed {__instance.name}");
    }
}
