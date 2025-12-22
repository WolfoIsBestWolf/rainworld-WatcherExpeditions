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
    public class Perk_DialWarp : Modding.Expedition.CustomPerk
    {
        public override string ID
        {
            get
            {
                return "unl-watcher-dialwarp";
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
                return RainWorld.RippleColor;
            }
        }
        public override string SpriteName
        {
            get
            {
                return "Symbol_DialWarpPerk";
            }
        }
        public override string ManualDescription
        {
            get
            {
                return this.Description;
            }
        }
        public override string Description
        {
            get
            {
                return T.TranslateLineBreak("Perk_DialWarp_Desc");
                return "Dial Warp (Ripple Egg Warp) is unlocked from the start. \nWatcher Exclusive";
            }
        }
        public override string DisplayName
        {
            get
            {
                return T.Translate("Perk_DialWarp_Name");
                return "Dial Warp";
            }
        }
        public override string Group
        {
            get
            {
                return "WatcherExpeditions";
            }
        }
        public override bool AvailableForSlugcat(SlugcatStats.Name name)
        {
            return name == WatcherEnums.SlugcatStatsName.Watcher;
        }
    }
}