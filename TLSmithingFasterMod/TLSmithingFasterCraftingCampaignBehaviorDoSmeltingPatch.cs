using System;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.CampaignSystem;
using HarmonyLib;
using System.Collections.Generic;

namespace TLSmithingFasterMod
{
    [HarmonyPatch(typeof(CraftingCampaignBehavior), "DoSmelting")]
    public class TLSmithingFasterCraftingCampaignBehaviorDoSmeltingPatch
    {
        public static void Postfix(ref CraftingCampaignBehavior __instance, Hero hero, ItemObject item)
        {
            ScreenBase topScreen = ScreenManager.TopScreen;
            if (topScreen != null && Input.IsKeyDown(InputKey.LeftShift) && !TLSmithingFasterOperationCounts.Flag)
            {
                TLSmithingFasterOperationCounts.Flag = true;
                int maxcounts = TLSmithingFasterOperationCounts.GetMaxCounts(ref __instance, hero, item);
                maxcounts = Math.Min(maxcounts, 4);
                IEnumerable<EquipmentElement> locks = Campaign.Current.GetCampaignBehavior<InventoryLockTracker>().GetLocks();
                ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
                bool IsLocked = false;
                int max_item_count = itemRoster.Count;
                for (int i = 0; i < max_item_count && maxcounts > 0; i++)
                {
                    itemRoster = MobileParty.MainParty.ItemRoster;
                    foreach (EquipmentElement EquipmentElement in locks)
                    {
                        if (EquipmentElement.Item == itemRoster.GetItemAtIndex(i))
                        {
                            IsLocked = true;
                            break;
                        }
                    }
                    int item_num = itemRoster.GetItemNumber(itemRoster.GetItemAtIndex(i));
                    for (int j = 0; j < item_num && !IsLocked && itemRoster.GetItemAtIndex(i).IsCraftedWeapon; j++)
                    {
                        __instance.DoSmelting(hero, itemRoster.GetItemAtIndex(i));
                        maxcounts--;
                        if (maxcounts <= 0)
                        {
                            TLSmithingFasterOperationCounts.Flag = false;
                            return;
                        }
                    }
                    IsLocked = false;
                }
                TLSmithingFasterOperationCounts.Flag = false;
            }
            if (topScreen != null && Input.IsKeyDown(InputKey.LeftControl) && !TLSmithingFasterOperationCounts.Flag)
            {
                TLSmithingFasterOperationCounts.Flag = true;
                int maxcounts = TLSmithingFasterOperationCounts.GetMaxCounts(ref __instance, hero, item);
                IEnumerable<EquipmentElement> locks = Campaign.Current.GetCampaignBehavior<InventoryLockTracker>().GetLocks();
                ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
                bool IsLocked = false;
                int max_item_count = itemRoster.Count;
                for (int i = 0; i < max_item_count && maxcounts > 0; i++)
                {
                    itemRoster = MobileParty.MainParty.ItemRoster;
                    foreach (EquipmentElement EquipmentElement in locks)
                    {
                        if (EquipmentElement.Item == itemRoster.GetItemAtIndex(i))
                        {
                            IsLocked = true;
                            break;
                        }
                    }
                    int item_num = itemRoster.GetItemNumber(itemRoster.GetItemAtIndex(i));
                    for (int j = 0; j < item_num && !IsLocked && itemRoster.GetItemAtIndex(i).IsCraftedWeapon; j++)
                    {
                        __instance.DoSmelting(hero, itemRoster.GetItemAtIndex(i));
                        maxcounts--;
                        if (maxcounts <= 0)
                        {
                            TLSmithingFasterOperationCounts.Flag = false;
                            return;
                        }
                    }
                    IsLocked = false;
                }
                TLSmithingFasterOperationCounts.Flag = false;
            }
        }
    }
}

