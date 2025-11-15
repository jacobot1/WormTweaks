using HarmonyLib;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

namespace WormTweaks.Patches
{
    [HarmonyPatch(typeof(GameManager))]
    internal class GameManagerPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void StartupPatches(GameManager __instance)
        {
            if (WormTweaksMod.configGlobalGravityMultiplier.Value != 1f)
            {
                Physics.gravity = new Vector3(0, -9.81f * WormTweaksMod.configGlobalGravityMultiplier.Value, 0);
            }
            if (WormTweaksMod.configDynamiteRadiusMultiplier.Value != 1f)
            {
                Dynamite.CONST_dynamiteRadius *= WormTweaksMod.configDynamiteRadiusMultiplier.Value;
            }
        }

        [HarmonyPatch("ReturnItemToWorld_Server")]
        [HarmonyPrefix]
        static bool ReturnItemToWorld_Server(GameManager __instance, ref ulong playerID, ref NetworkObject objectToDrop, ref Vector3 position, ref Quaternion rotation, ref Vector3 throwDirection, ref bool __result)
        {
            NetworkObject playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(playerID);
            Player component = playerNetworkObject.GetComponent<Player>();
            PickUppable component2 = objectToDrop.GetComponent<PickUppable>();
            component2.SetPlayerWhoIsHoldingUsID(1000000uL);
            component2.SetUngrounded();
            if (component2.isLadder)
            {
                Vector3 vector = new Vector3(throwDirection.x, 0f, throwDirection.z);
                if (vector == Vector3.zero)
                {
                    vector = new Vector3(playerNetworkObject.transform.forward.x, 0f, playerNetworkObject.transform.forward.z);
                }
                objectToDrop.GetComponent<NetworkTransform>().Teleport(position + Vector3.up * 3f, Quaternion.LookRotation(vector), objectToDrop.transform.localScale);
            }
            else
            {
                objectToDrop.GetComponent<NetworkTransform>().Teleport(position, rotation, objectToDrop.transform.localScale);
            }
            if (component2.pickUpSize != PickUppable.PickUpSize.large)
            {
                component2.IgnoreCollisionsWithPlayer_Server(playerNetworkObject);
            }
            else if (throwDirection != Vector3.zero && Vector3.Angle(throwDirection, Vector3.down) > 35f)
            {
                component2.IgnoreCollisionsWithPlayer_Server(playerNetworkObject);
            }
            PickUppable component3 = objectToDrop.GetComponent<PickUppable>();
            component3.isInteractable = true;
            if (component3.isLadder)
            {
                component3.rb.constraints = (RigidbodyConstraints)80;
            }
            else
            {
                component3.rb.constraints = RigidbodyConstraints.None;
            }
            Collider[] ourCols = component3.ourCols;
            for (int i = 0; i < ourCols.Length; i++)
            {
                ourCols[i].enabled = true;
            }
            component3.rb.isKinematic = component3.isKinematicByDefault;
            component3.rb.useGravity = component3.useGravityByDefault;
            component3.rb.AddForce(throwDirection.normalized * (component3.throwVelocity * WormTweaksMod.configPlayerYeet.Value * (1f + component.syncedSlingShotAdditionalThrowMultip.Value)), ForceMode.VelocityChange);
            component3.OnDrop_ClientRPC();
            __result = true;
            return false;
        }
    }
}
