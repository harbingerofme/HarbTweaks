using System;
using BepInEx.Configuration;
using MonoMod.RuntimeDetour;
using R2API.Utils;
using UnityEngine;

namespace HarbTweaks
{
    [Tweak(TweakName, DefaultEnabled, Description, Target)]
    internal sealed class SilentAltar : Tweak
    {
        private const string TweakName = "Silent Altar";
        private const bool DefaultEnabled = true;
        private const string Description = "Disables that stupid windnoise that you can hear from everywhere on the Wetland Aspect.";
        private const TweakStartupTarget Target = TweakStartupTarget.Awake;

        public SilentAltar(ConfigFile config, string name, bool defaultEnabled, string description) : base(config, name, defaultEnabled, description)
        {
        }

        protected override void Hook()
        {
            RoR2.SceneDirector.onPostPopulateSceneServer += SceneDirector_onPostPopulateSceneServer;
        }

        private void SceneDirector_onPostPopulateSceneServer(RoR2.SceneDirector obj)
        {
            UnityEngine.Object.Destroy(GameObject.Find("AltarSkeletonBody").GetComponent<AkEvent>());
        }

        protected override void MakeConfig()
        {
        }

        protected override void UnHook()
        {
            RoR2.SceneDirector.onPostPopulateSceneServer -= SceneDirector_onPostPopulateSceneServer;
        }
    }
}
