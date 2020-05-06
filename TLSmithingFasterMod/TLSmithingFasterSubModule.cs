using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using HarmonyLib;


namespace TLSmithingFasterMod
{
    public class TLSmithingFasterSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            new Harmony("bannerlord.mod.TLSmithingFasterMod").PatchAll();
        }
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            InformationManager.DisplayMessage(new InformationMessage("TL Smithing Faster Mod beta1.3.0.2 is successfully loaded. HarmonyLib 2.0.0.10"));
        }
    }
}
