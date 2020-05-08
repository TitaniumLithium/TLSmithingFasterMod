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
        public static void Postfix(ref CraftingCampaignBehavior __instance, Hero hero, EquipmentElement equipmentElement)
        {
            ScreenBase topScreen = ScreenManager.TopScreen;
            if (topScreen != null && Input.IsKeyDown(InputKey.LeftShift) && !TLSmithingFasterOperationCounts.Flag)
            {
                TLSmithingFasterOperationCounts.Flag = true;
                int maxcounts = TLSmithingFasterOperationCounts.GetMaxCounts(ref __instance, hero, equipmentElement);
                maxcounts = Math.Min(maxcounts, 4);
                IEnumerable<EquipmentElement> locks = Campaign.Current.GetCampaignBehavior<InventoryLockTracker>().GetLocks();
                ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
                ItemRosterElement[] ItemRosterElements = itemRoster.GetCopyOfAllElements();
                bool IsLocked = false;
                int max_item_count = ItemRosterElements.Length;
                for (int i = 0; i < max_item_count && maxcounts > 0; i++)
                {
                    itemRoster = MobileParty.MainParty.ItemRoster;
                    ItemRosterElements = itemRoster.GetCopyOfAllElements();
                    foreach (EquipmentElement EquipmentElement in locks)
                    {
                        if (EquipmentElement.IsEqualTo(ItemRosterElements[i].EquipmentElement))
                        {
                            IsLocked = true;
                            break;
                        }
                    }
                    int item_num = ItemRosterElements[i].Amount;
                    int j;
                    for (j = 0; j < item_num && !IsLocked && ItemRosterElements[i].EquipmentElement.Item.IsCraftedWeapon; j++)
                    {
                        __instance.DoSmelting(hero, ItemRosterElements[i].EquipmentElement);
                        maxcounts--;
                        if (maxcounts <= 0)
                        {
                            TLSmithingFasterOperationCounts.Flag = false;
                            return;
                        }
                    }
                    if (j == item_num)
                    {
                        i--;
                        max_item_count--;
                    }
                    IsLocked = false;
                }
                TLSmithingFasterOperationCounts.Flag = false;
            }
            if (topScreen != null && Input.IsKeyDown(InputKey.LeftControl) && !TLSmithingFasterOperationCounts.Flag)
            {
                TLSmithingFasterOperationCounts.Flag = true;
                int maxcounts = TLSmithingFasterOperationCounts.GetMaxCounts(ref __instance, hero, equipmentElement);
                IEnumerable<EquipmentElement> locks = Campaign.Current.GetCampaignBehavior<InventoryLockTracker>().GetLocks();
                ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
                ItemRosterElement[] ItemRosterElements = itemRoster.GetCopyOfAllElements();
                bool IsLocked = false;
                int max_item_count = ItemRosterElements.Length;
                for (int i = 0; i < max_item_count && maxcounts > 0; i++)
                {
                    itemRoster = MobileParty.MainParty.ItemRoster;
                    ItemRosterElements = itemRoster.GetCopyOfAllElements();
                    foreach (EquipmentElement EquipmentElement in locks)
                    {
                        if (EquipmentElement.IsEqualTo(ItemRosterElements[i].EquipmentElement))
                        {
                            IsLocked = true;
                            break;
                        }
                    }
                    int item_num = ItemRosterElements[i].Amount;
                    int j;
                    for (j = 0; j < item_num && !IsLocked && ItemRosterElements[i].EquipmentElement.Item.IsCraftedWeapon; j++)
                    {
                        __instance.DoSmelting(hero, ItemRosterElements[i].EquipmentElement);
                        maxcounts--;
                        if (maxcounts <= 0)
                        {
                            TLSmithingFasterOperationCounts.Flag = false;
                            return;
                        }
                    }
                    if (j == item_num)
                    {
                        i--;
                        max_item_count--;
                    }
                    IsLocked = false;
                }
                TLSmithingFasterOperationCounts.Flag = false;
            }
        }
    }
}

