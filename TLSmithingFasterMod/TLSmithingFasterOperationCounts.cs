using System;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;

namespace TLSmithingFasterMod
{
    public static class TLSmithingFasterOperationCounts
    {
        public static int GetMaxCounts(ref CraftingCampaignBehavior instance, Hero hero, Crafting.RefiningFormula refineFormula)
        {
            ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
            int energyCostForRefining = Campaign.Current.Models.SmithingModel.GetEnergyCostForRefining(ref refineFormula, hero);
            int result = instance.GetHeroCraftingStamina(hero) / energyCostForRefining;
            if (refineFormula.Input1Count > 0)
            {
                ItemObject craftingMaterialItem1;
                int input1result;
                craftingMaterialItem1 = Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem(refineFormula.Input1);
                input1result = MaxForInput(itemRoster, craftingMaterialItem1, refineFormula.Input1Count);
                result = input1result;
            }
            if (refineFormula.Input2Count > 0)
            {
                ItemObject craftingMaterialItem2;
                int input2result;
                craftingMaterialItem2 = Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem(refineFormula.Input2);
                input2result = MaxForInput(itemRoster, craftingMaterialItem2, refineFormula.Input2Count);
                return Math.Min(result, input2result);
            }
            return result;
        }
        public static int GetMaxCounts(ref CraftingCampaignBehavior instance, Hero hero, ItemObject item)
        {
            ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
            int energyCostForSmelting = Campaign.Current.Models.SmithingModel.GetEnergyCostForSmelting(item, hero);
            int result = instance.GetHeroCraftingStamina(hero) / energyCostForSmelting;
            string charcoal = "charcoal";
            int charcoal_num = 0;
            int max_item_count = itemRoster.Count;
            for (int i = 0; i < max_item_count; i++)
            {
                if (itemRoster.GetItemAtIndex(i).ToString() == charcoal)
                {
                    charcoal_num = itemRoster.GetItemNumber(itemRoster.GetItemAtIndex(i));
                }
            }
            result = Math.Min(result, charcoal_num);
            //return Math.Min(result, MaxForInput(itemRoster, item, 1));
            return result;
        }
        public static int GetMaxCounts(ref CraftingCampaignBehavior instance, Hero hero, WeaponDesign weaponDesign, int modifierTier, Crafting.OverrideData overrideData)
        {
            ItemRoster itemRoster = MobileParty.MainParty.ItemRoster;
            int[] smithingCostsForWeaponDesign = Campaign.Current.Models.SmithingModel.GetSmithingCostsForWeaponDesign(weaponDesign);
            //Taleworlds may fix "9" in following days
            CraftingState craftingState;
            ItemObject currentCraftedItemObject;
            if ((craftingState = (GameStateManager.Current.ActiveState as CraftingState)) != null)
                //false == don't force recreate. shall this effect item creation?
                currentCraftedItemObject = craftingState.CraftingLogic.GetCurrentCraftedItemObject(false, null);
            else return 0;
            int energyCostForSmithing = Campaign.Current.Models.SmithingModel.GetEnergyCostForSmithing(currentCraftedItemObject, hero);
            int result = instance.GetHeroCraftingStamina(hero) / energyCostForSmithing;
            for (int i = 0; i < 9; i++)
            {
                if (smithingCostsForWeaponDesign[i] != 0)
                {
                    result = Math.Min(result, MaxForInput(itemRoster, Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem((CraftingMaterials)i), 0-smithingCostsForWeaponDesign[i]));
                }
            }
            return result;
        }
        private static int MaxForInput(ItemRoster itemRoster, ItemObject inputitem, int inputcount)
        {
            int itemnumber = itemRoster.GetItemNumber(inputitem);
            if (itemnumber <= 0)
                return 0;
            return itemnumber / inputcount;
        }
        public static bool Flag = false;
    }
}
