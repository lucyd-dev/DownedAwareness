
using HarmonyLib;
using static DownedAwareness.Plugin;

namespace DownedAwareness.Patches;

[HarmonyPatch(typeof(GUIManager))]
static class GUIManagerPatch
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    private static void Start_Postfix(GUIManager __instance)
    {
        POM.Initialize();
    }

    [HarmonyPatch("LateUpdate")]
    [HarmonyPostfix]
    private static void LateUpdate_Postfix(GUIManager __instance)
    {
        if (!POM.isInitialized)
            POM.Initialize();
    }

    [HarmonyPatch("OnDestroy")]
    [HarmonyPostfix]
    private static void OnDestroy_Postfix(GUIManager __instance)
    {
        POM.OnDestroy();
    }

}
