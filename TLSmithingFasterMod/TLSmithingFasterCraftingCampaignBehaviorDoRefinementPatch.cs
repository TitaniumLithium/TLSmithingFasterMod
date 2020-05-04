using TaleWorlds.Core;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.CampaignSystem;
using HarmonyLib;
using System;

namespace TLSmithingFasterMod
{
    [HarmonyPatch(typeof(CraftingCampaignBehavior), "DoRefinement")]
    public class TLSmithingFasterCraftingCampaignBehaviorDoRefinementPatch
    {
        private static void Postfix(ref CraftingCampaignBehavior __instance, Hero hero, Crafting.RefiningFormula refineFormula)
        {
            ScreenBase topScreen = ScreenManager.TopScreen;
            if (topScreen != null && Input.IsKeyDown(InputKey.LeftShift) && !TLSmithingFasterOperationCounts.Flag)
            {
                TLSmithingFasterOperationCounts.Flag = true;
                int maxcounts = TLSmithingFasterOperationCounts.GetMaxCounts(ref __instance, hero, refineFormula);
                maxcounts = Math.Min(maxcounts, 4);
                for (int i = 0; i < maxcounts; i++)
                {
                    __instance.DoRefinement(hero, refineFormula);
                }
                TLSmithingFasterOperationCounts.Flag = false;
            }
            if (topScreen != null && Input.IsKeyDown(InputKey.LeftControl) && !TLSmithingFasterOperationCounts.Flag)
            {
                TLSmithingFasterOperationCounts.Flag = true;
                int maxcounts = TLSmithingFasterOperationCounts.GetMaxCounts(ref __instance, hero, refineFormula);
                for (int i = 0; i < maxcounts; i++)
                {
                    __instance.DoRefinement(hero, refineFormula);
                }
                TLSmithingFasterOperationCounts.Flag = false;
            }
        }
    }
}
