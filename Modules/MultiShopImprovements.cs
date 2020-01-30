using BepInEx;
using UnityEngine;
using R2API.Utils;
using RoR2;
using BepInEx.Configuration;
using System.Collections.Generic;

namespace HarbTweaks
{
    [TweakAttribute(TweakName, DefaultEnabled, Description)]
    internal sealed class MultiShopImprovements : Tweak
    {
        private const string TweakName = "Multishop Improvements";
        private const bool DefaultEnabled = true;
        private const string Description = "This tweak aims to always leave more choice when interacting with a multishop.";

        private ConfigEntry<int> maxQuestions;
        private ConfigEntry<int> maxSame;

        public MultiShopImprovements(ConfigFile config, string name, bool defaultEnabled, string description) : base(config, name, defaultEnabled, description)
        { }

        protected override void MakeConfig()
        {
            maxQuestions = AddConfig("Max amount of question marks.", 1, new ConfigDescription("Amount of question marks that can appear at once on a multishop. Vanilla is 2.",new AcceptableValueRange<int>(0,2)));
            maxSame = AddConfig("Max amount of duplicates.", 1, new ConfigDescription("Max amount of duplicates in a shop. Vanilla is 2.", new AcceptableValueRange<int>(0, 2)));
        }

        protected override void Hook()
        {
            On.RoR2.MultiShopController.CreateTerminals += MultiShopController_on_CreateTerminals1;
        }

        protected override void UnHook()
        {
            On.RoR2.MultiShopController.CreateTerminals -= MultiShopController_on_CreateTerminals1;
        }

        private void MultiShopController_on_CreateTerminals1(On.RoR2.MultiShopController.orig_CreateTerminals orig, RoR2.MultiShopController self)
        {
            orig(self);

            int questionCount = 0;
            int sameCount = 0;
            List<PickupIndex> pickups = new List<PickupIndex>();

            Xoroshiro128Plus rng = self.GetFieldValue<Xoroshiro128Plus>("rng");
            GameObject[] terminals = self.GetFieldValue<GameObject[]>("terminalGameObjects");
            ShopTerminalBehavior[] behaviors = new ShopTerminalBehavior[3];
            for (int i=0;i<3;i++)
            {
                GameObject terminal = terminals[i];
                ShopTerminalBehavior stb = terminal.GetComponent<ShopTerminalBehavior>();
                
                behaviors[i] = stb;
                if (stb)
                {
                    bool shopDirty = false;
                    bool hidden = stb.pickupIndexIsHidden;
                    if (hidden)
                    {
                        questionCount++;
                        if (questionCount > maxQuestions.Value)
                        {
                            hidden = false;
                            shopDirty |= true;
                        }
                    }
                    PickupIndex pickupIndex = stb.CurrentPickupIndex();
                    if (pickups.Contains(pickupIndex))
                    {
                        sameCount++;
                        if (sameCount > maxSame.Value)
                        {
                            shopDirty |= true;
                            switch (self.itemTier)
                            {
                                case ItemTier.Tier1:
                                    pickupIndex = rng.NextElementUniform(Run.instance.availableTier1DropList);
                                    break;
                                case ItemTier.Tier2:
                                    pickupIndex = rng.NextElementUniform(Run.instance.availableTier2DropList);
                                    break;
                                case ItemTier.Tier3:
                                    pickupIndex = rng.NextElementUniform(Run.instance.availableTier3DropList);
                                    break;
                                case ItemTier.Lunar:
                                    pickupIndex = rng.NextElementUniform(Run.instance.availableLunarDropList);
                                    break;
                            }
                        }
                    }
                    pickups.Add(pickupIndex);

                    if(shopDirty)
                    {
                        stb.SetPickupIndex(pickupIndex, hidden);
                    }
                }
                else
                {
                    TweakLogger.LogWarning("MultiShopImprovements", "Something was wrong with a terminal, aborting.");
                    return;
                }
            }
            while (questionCount > maxQuestions.Value)
            {
                questionCount--;
                behaviors[questionCount].SetPickupIndex(pickups[questionCount], false);
            }
            if (sameCount > maxSame.Value)
            {

            }
        }
    }
}
