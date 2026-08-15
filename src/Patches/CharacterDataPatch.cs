using HarmonyLib;
using Photon.Pun;
using static DownedAwareness.Plugin;

namespace DownedAwareness.Patches;

[HarmonyPatch(typeof(CharacterData))]
static class CharacterDataPatch
{
    [HarmonyPatch("RPC_SyncOnJoin")]
    [HarmonyPrefix]
    private static void RPC_SyncOnJoin_Prefix(CharacterData __instance, bool passedOut, bool fullyPassedOut, bool dead, bool isSprinting, PhotonView currentItem, bool isJumping, bool isClimbing, bool isRopeClimbing, bool isVineClimbing, float vinePercent, float ropePercent, bool isCrouching, bool isReaching, PhotonView heldVine, PhotonView heldRope, bool[] badgeStatus, bool cannibalismPermitted, bool isSkeleton, int petrifyAmount)
    {
        if (__instance.passedOut && !passedOut)
        {
            POM.RemoveInstance(__instance.character);
            Log.LogDebug($"Revived by checkpointFlag (RPC_SyncOnJoin) {__instance.name}");
        }
    }
}
