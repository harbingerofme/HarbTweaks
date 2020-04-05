using MonoMod.Cil;
using Mono.Cecil.Cil;
using BepInEx.Configuration;
using R2API.Utils;
using RoR2;

/*
    Code By Guido "Harb". 
     */

namespace HarbTweaks
{

    [TweakAttribute(TweakName,DefaultEnabled,Description)]
    internal sealed class FirstStageSpawns : Tweak
    {
        private const string TweakName = "First Stage Spawns";
        private const bool DefaultEnabled = false;
        private const string Description = "This tweak aims to get you going quicker by adding more enemies to the first stage.";

        private ConfigEntry<float> scaling;
        private ConfigEntry<bool> applyToAll;
        
        public FirstStageSpawns(ConfigFile config, string name, bool defaultEnabled, string description) : base(config, name, defaultEnabled, description)
        { }

        protected override void MakeConfig()
        {
            scaling =  AddConfig(
                "First stage scaling",
                2f,
                "Vanilla gameplay is 1."
                );
            applyToAll = AddConfig("Apply to all stages", false, "Just apply the scaling to all stages, lmao");
        }

        protected override void UnHook()
        {
            On.RoR2.CombatDirector.SpendAllCreditsOnMapSpawns -= CombatDirector_SpendAllCreditsOnMapSpawns;
        }

        protected override void Hook()
        {
            On.RoR2.CombatDirector.SpendAllCreditsOnMapSpawns += CombatDirector_SpendAllCreditsOnMapSpawns;
        }

        private void CombatDirector_SpendAllCreditsOnMapSpawns(On.RoR2.CombatDirector.orig_SpendAllCreditsOnMapSpawns orig, RoR2.CombatDirector self)
        {
            if(Run.instance && (applyToAll.Value || Run.instance.stageClearCount == 0))
            {
                self.monsterCredit *= scaling.Value;
            }
            orig(self);
        }
    }
}
