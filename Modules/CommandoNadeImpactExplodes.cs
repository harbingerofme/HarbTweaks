using System;
using BepInEx.Configuration;
using RoR2.Projectile;
using UnityEngine;

namespace HarbTweaks
{

    //Code provided by: https://github.com/SushiDevshi

    [Tweak(TweakName, DefaultEnabled, Description, Target)]
    internal sealed class CommandoNadeImpactExplodes : Tweak
    {
        private const string TweakName = "Commando Grenades are Impact";
        private const bool DefaultEnabled = false;
        private const string Description = "Makes Commando's grenade explode on impact.";
        private const TweakStartupTarget Target = TweakStartupTarget.Awake;

        private float vanillaValue;

        public CommandoNadeImpactExplodes(ConfigFile config, string name, bool defaultEnabled, string description) : base(config, name, defaultEnabled, description)
        {
        }

        protected override void Hook()
        {
            var a = Resources.Load<GameObject>("prefabs/projectiles/commandogrenadeprojectile").GetComponent<ProjectileImpactExplosion>();
            vanillaValue = a.lifetimeAfterImpact; ;
             a.lifetimeAfterImpact = 0f;
        }

        protected override void MakeConfig()
        {
        }

        protected override void UnHook()
        {
            Resources.Load<GameObject>("prefabs/projectiles/commandogrenadeprojectile").GetComponent<ProjectileImpactExplosion>().lifetimeAfterImpact = vanillaValue;
        }
    }
}
