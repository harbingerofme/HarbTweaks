using BepInEx.Configuration;
using RoR2;
using UnityEngine;

namespace HarbTweaks
{

    //Code provided by: https://github.com/SushiDevshi

    [Tweak(TweakName, DefaultEnabled, Description, Target)]
    internal sealed class GrandParentEffectFix : Tweak
    {
        private const string TweakName = "GrandParent Effect Disable";
        private const bool DefaultEnabled = true;
        private const string Description = "Fixes the grandparent swing effect error from clogging the console.";
        private const TweakStartupTarget Target = TweakStartupTarget.Awake;

        private EffectComponent toBeDisabled;

        public GrandParentEffectFix(ConfigFile config, string name, bool defaultEnabled, string description) : base(config, name, defaultEnabled, description)
        {
        }

        protected override void Hook()
        {
            if (!toBeDisabled)
                toBeDisabled = Resources.Load<GameObject>("prefabs/effects/GrandparentGroundSwipeTrailEffect").AddComponent<EffectComponent>();
        }

        protected override void MakeConfig()
        {
        }

        protected override void UnHook()
        {
            if (toBeDisabled)
                UnityEngine.GameObject.Destroy(toBeDisabled);
            toBeDisabled = null;
        }
    }
}