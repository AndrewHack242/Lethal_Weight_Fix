using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using GameNetcodeStuff;
using HarmonyLib;


namespace Lethal_Weight_Fix.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class Lethal_weight_fix
    {
        // make one function to deliver the patch the same way to every required location.
        static IEnumerable<CodeInstruction> deliver_patch(IEnumerable<CodeInstruction> instructions, string func_name)
        {
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; ++i)
            {
                if (codes[i].Calls(typeof(UnityEngine.Mathf).GetMethod("Clamp", new Type[] { typeof(float), typeof(float), typeof(float) })))
                {
                    Lethal_weight_fix_base.LogInfo("Found " + codes[i] + "in function " + func_name);
                    if (i < 3)
                    {
                        Lethal_weight_fix_base.LogInfo("index was too small on match in " + codes[i] + "in function " + func_name + ", index: " + i);
                        continue;
                    }
                    codes.Insert(i + 1, codes[i - 3]);
                    Lethal_weight_fix_base.LogInfo("opcode = " + codes[i - 2].opcode);
                    Lethal_weight_fix_base.LogInfo("operand = " + codes[i - 2].operand);
                    codes[i - 2].operand = 0f;
                    Lethal_weight_fix_base.LogInfo("updated opcode = " + codes[i - 2].opcode);
                    Lethal_weight_fix_base.LogInfo("updated operand = " + codes[i - 2].operand);
                    codes.RemoveAt(i - 3);
                    Lethal_weight_fix_base.LogInfo("Patched function " + func_name);
                }
            }
            return codes;
        }

        [HarmonyPatch("BeginGrabObject")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> patch_BeginGrabObject(IEnumerable<CodeInstruction> instructions)
        {
            return deliver_patch(instructions, "BeginGrabObject");
        }

        [HarmonyPatch("GrabObject")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> patch_GrabObject(IEnumerable<CodeInstruction> instructions)
        {
            return deliver_patch(instructions, "GrabObject");
        }

        [HarmonyPatch("GrabObjectClientRpc")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> patch_GrabObjectClientRpc(IEnumerable<CodeInstruction> instructions)
        {
            return deliver_patch(instructions, "GrabObjectClientRpc");
        }

        [HarmonyPatch("DespawnHeldObjectOnClient")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> patch_DespawnHeldObjectOnClient(IEnumerable<CodeInstruction> instructions)
        {
            return deliver_patch(instructions, "DespawnHeldObjectOnClient");
        }

        [HarmonyPatch("SetObjectAsNoLongerHeld")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> patch_SetObjectAsNoLongerHeld(IEnumerable<CodeInstruction> instructions)
        {
            return deliver_patch(instructions, "SetObjectAsNoLongerHeld");
        }

        [HarmonyPatch("PlaceGrabbableObject")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> patch_PlaceGrabbableObject(IEnumerable<CodeInstruction> instructions)
        {
            return deliver_patch(instructions, "PlaceGrabbableObject");
        }

        [HarmonyPatch("DestroyItemInSlot")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> patch_DestroyItemInSlot(IEnumerable<CodeInstruction> instructions)
        {
            return deliver_patch(instructions, "DestroyItemInSlot");
        }

        static void enforce_miunimum_weight(ref float ___carryWeight, string func)
        {
            if (___carryWeight < 1f)
            {
                Lethal_weight_fix_base.LogWarning("Carry Weight went below 1 {" + ___carryWeight + "}. This should not happen. Resetting weight to 1. function: " + func);
                ___carryWeight = 1f;
            }
        }

        [HarmonyPatch("DespawnHeldObjectOnClient")]
        [HarmonyPostfix]
        static void patch_DespawnHeldObjectOnClient_postfix(ref float ___carryWeight)
        {
            enforce_miunimum_weight(ref ___carryWeight, "DespawnHeldObjectOnClient");
        }

        [HarmonyPatch("SetObjectAsNoLongerHeld")]
        [HarmonyPostfix]
        static void patch_SetObjectAsNoLongerHeld_postfix(ref float ___carryWeight)
        {
            enforce_miunimum_weight(ref ___carryWeight, "SetObjectAsNoLongerHeld");
        }

        [HarmonyPatch("PlaceGrabbableObject")]
        [HarmonyPostfix]
        static void patch_PlaceGrabbableObject_postfix(ref float ___carryWeight)
        {
            enforce_miunimum_weight(ref ___carryWeight, "PlaceGrabbableObject");
        }

        [HarmonyPatch("DestroyItemInSlot")]
        [HarmonyPostfix]
        static void patch_DestroyItemInSlot_postfix(ref float ___carryWeight)
        {
            enforce_miunimum_weight(ref ___carryWeight, "DestroyItemInSlot");
        }

    }
}