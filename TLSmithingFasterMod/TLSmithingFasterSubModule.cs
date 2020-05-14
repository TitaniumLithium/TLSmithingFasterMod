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
            InformationManager.DisplayMessage(new InformationMessage("TLSmithingFasterMod e1.3.1.0 is successfully loaded."));
        }
    }
}
