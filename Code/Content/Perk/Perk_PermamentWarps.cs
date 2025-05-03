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
    public class Perk_PermamentWarps : Modding.Expedition.CustomPerk
    {
        public override string ID
        {
            get
            {
                return "unl-watcher-permwarp";
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
                return RainWorld.RippleGold;
            }
        }
        public override string SpriteName
        {
            get
            {
                return "Symbol_RipplePerk";
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
                return "Formed paths (Warps) are permament instead of expiring after 5 cycles.\nWatcher Exclusive";
            }
        }
        public override string DisplayName
        {
            get
            {
                return "Permament Warps";
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