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
                ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
                int item_num = 0;
                if (itemRoster.FindIndexOfElement(equipmentElement) >= 0)
                    item_num = itemRoster[itemRoster.FindIndexOfElement(equipmentElement)].Amount;
                maxcounts = Math.Min(maxcounts, item_num);
                for (int i = 0;i < maxcounts; i++)
                {
                    __instance.DoSmelting(hero, equipmentElement);
                }
                TLSmithingFasterOperationCounts.Flag = false;
            }
            if (topScreen != null && Input.IsKeyDown(InputKey.LeftControl) && !TLSmithingFasterOperationCounts.Flag)
            {
                TLSmithingFasterOperationCounts.Flag = true;
                IEnumerable<EquipmentElement> locks = Campaign.Current.GetCampaignBehavior<InventoryLockTracker>().GetLocks();
                ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
                ItemRosterElement[] ItemRosterElements = itemRoster.GetCopyOfAllElements();
                bool IsLocked = false;
                int max_item_count = ItemRosterElements.Length;
                for (int i = 0; i < max_item_count; i++)
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
                        int maxcounts = TLSmithingFasterOperationCounts.GetMaxCounts(ref __instance, hero, ItemRosterElements[i].EquipmentElement);
                        if (maxcounts <= 0)
                        {
                            TLSmithingFasterOperationCounts.Flag = false;
                            return;
                        }
                        __instance.DoSmelting(hero, ItemRosterElements[i].EquipmentElement);
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

