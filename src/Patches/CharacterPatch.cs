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
    }

    [HarmonyPatch("RPCA_UnPassOut")]
    [HarmonyPostfix]
    private static void RPCA_UnPassOut_Postfix(Character __instance)
    {
        POM.UpdateInstance(__instance);
    }

    [HarmonyPatch("RPCA_Revive")]
    [HarmonyPostfix]
    private static void RPCA_Revive_Postfix(Character __instance)
    {
        POM.UpdateInstance(__instance);
    }

    [HarmonyPatch("RPCA_Die")]
    [HarmonyPostfix]
    private static void RPCA_Die_Postfix(Character __instance)
    {
        POM.UpdateInstance(__instance, true);
    }
}
