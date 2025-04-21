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
                return "unl-watcher_permwarp";
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
                return "Kill_Daddy";
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
                return "Created warps are permament instead of expiring after 5 cycles.";
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
    }
}