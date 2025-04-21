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
    public class Perk_PoisonSpear : Modding.Expedition.CustomPerk
    {
        public override string ID
        {
            get
            {
                return "unl-watcher_poisonspear";
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
                return new Color(0.4f,1f,0.4f);
            }
        }
        public override string SpriteName
        {
            get
            {
                return "Symbol_Spear";
            }
        }
        public override string ManualDescription
        {
            get
            {
                return "Some spears spawn with poison applied";
            }
        }
        public override string Description
        {
            get
            {
                return "Some spears spawn with poison applied";
            }
        }
        public override string DisplayName
        {
            get
            {
                return "Poison Spears";
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