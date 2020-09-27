using BepInEx.Configuration;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace HarbTweaks
{

    //Code provided by: https://github.com/SushiDevshi

    [Tweak(TweakName, DefaultEnabled, Description, Target)]
    internal sealed class HideEffectLog : Tweak
    {
        private const string TweakName = "Hides the missing effects";
        private const bool DefaultEnabled = true;
        private const string Description = "Fixes the certain effect errors from clogging the console.";
        private const TweakStartupTarget Target = TweakStartupTarget.Awake;

        private readonly string[] effects = new string[] { "DroneFlamethrowerEffect", "FireMeatBallPool", "LunarWispTrackingBombExp_Prf", "LunarWispMinigunChargeUp", "SiphonTetherHealing" };
        private List<EffectComponent> addedObjects;

        public HideEffectLog(ConfigFile config, string name, bool defaultEnabled, string description) : base(config, name, defaultEnabled, description)
        {
            addedObjects = new List<EffectComponent>();
        }

        protected override void Hook()
        {
            if (addedObjects.Count == 0) {
                foreach(string effect in effects)
                {
                    addedObjects.Add(Resources.Load<GameObject>($"prefabs/effects/{effect}").AddComponent<EffectComponent>());
                }
            }
        }

        protected override void MakeConfig()
        {
        }

        protected override void UnHook()
        {
            foreach(EffectComponent ec in addedObjects)
            {
                GameObject.Destroy(ec);
            }
            addedObjects.Clear();
        }
    }
}