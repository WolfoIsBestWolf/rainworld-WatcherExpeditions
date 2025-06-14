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
                return "unl-watcher-PoisonSpear";
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
                return new Color(0.3f,0.9f,0.33f);
            }
        }
        public override string SpriteName
        {
            get
            {
                return "Symbol_PoisonSpear";
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
                return T.Translate("Perk_PoisonSpear_Desc");
                return "Start the expedition with a poison tipped Spear";
            }
        }
        public override string DisplayName
        {
            get
            {
                return T.Translate("Perk_PoisonSpear_Name");
                return "Poison Spear";
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