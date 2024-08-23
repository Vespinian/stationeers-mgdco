using HarmonyLib;
using StationeersMods.Interface;


namespace MoreGasDisplayConsoleOptions
{
    class MoreGasDisplayConsoleOptions : ModBehaviour
    {
        public override void OnLoaded(ContentHandler contentHandler)
        {
            Harmony harmony = new Harmony("MoreGasDisplayConsoleOptions");
            harmony.PatchAll();
            UnityEngine.Debug.Log("MoreGasDisplayConsoleOptions Loaded!");
        }
    }
}