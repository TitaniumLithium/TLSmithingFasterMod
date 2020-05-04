using System;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.CampaignSystem;
using HarmonyLib;

namespace TLSmithingFasterMod
{
    [HarmonyPatch(typeof(CraftingCampaignBehavior), "CreateCraftedWeapon")]
    public class TLSmithingFasterCraftingCampaignBehaviorCreateCraftedWeaponPatch
    {
        public static void Postfix(ref CraftingCampaignBehavior __instance, Hero hero, WeaponDesign weaponDesign, int modifierTier, Crafting.OverrideData overrideData)
        {
            ScreenBase topScreen = ScreenManager.TopScreen;
            if (topScreen != null && Input.IsKeyDown(InputKey.LeftShift) && !TLSmithingFasterOperationCounts.Flag)
            {
                TLSmithingFasterOperationCounts.Flag = true;
                int maxcounts = TLSmithingFasterOperationCounts.GetMaxCounts(ref __instance, hero, weaponDesign, modifierTier, overrideData);
                maxcounts = Math.Min(maxcounts, 4);
                for (int i = 0; i < maxcounts; i++)
                {
                    __instance.CreateCraftedWeapon(hero, weaponDesign, modifierTier, overrideData);
                }
                TLSmithingFasterOperationCounts.Flag = false;
            }
            if (topScreen != null && Input.IsKeyDown(InputKey.LeftControl) && !TLSmithingFasterOperationCounts.Flag)
            {
                TLSmithingFasterOperationCounts.Flag = true;
                int maxcounts = TLSmithingFasterOperationCounts.GetMaxCounts(ref __instance, hero, weaponDesign, modifierTier, overrideData);
                for (int i = 0; i < maxcounts; i++)
                {
                    __instance.CreateCraftedWeapon(hero, weaponDesign, modifierTier, overrideData);
                }
                TLSmithingFasterOperationCounts.Flag = false;
            }
        }
    }
}
