using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;
using JetBrains.Annotations;
using RWCustom;
using UnityEngine;


 
namespace WatcherExpeditions
{
    public class Perk_Boomerang : Modding.Expedition.CustomPerk
    {
        public override string ID
        {
            get
            {
                return "unl-watcher-boomerang";
            }
        }
        public override bool UnlockedByDefault
        {
            get
            {
                return true;
            }
        }
        public override Color Color
        {
            get
            {
                return new Color(1f,0.8f,0.6f);
            }
        }
        public override string SpriteName
        {
            get
            {
                return "Symbol_Boomerang";
            }
        }
        public override string ManualDescription
        {
            get
            {
                return Description;
            }
        }
        public override string Description
        {
            get
            {
                return ChallengeTools.IGT.Translate("All rocks have a 50% chance to be replaced with a Boomerang");
            }
        }
        public override string DisplayName
        {
            get
            {
                return ChallengeTools.IGT.Translate("Boomerang");
            }
        }
        public override string Group
        {
            get
            {
                return "WatcherExpeditions";
            }
        }
         
    }
}
