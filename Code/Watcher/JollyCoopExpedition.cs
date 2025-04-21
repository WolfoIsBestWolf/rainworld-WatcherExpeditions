using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Watcher;
using MoreSlugcats;
using RWCustom;
using Menu;
using Expedition;
using MonoMod.Cil;
using UnityEngine;
using JollyCoop;


namespace WatcherExpeditions
{
    internal class JollyCoopAdditions
    {
        //public static bool ForceShowCamoHud;

        public static void Start()
        {
            Hook hook = new Hook(typeof(Player).GetProperty("rippleLevel", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), rippleLevel);

            //Idk how Hud ownership works, presumably always gets Hud of player 1 so
            //Even if Camo Meter showed it wouldnt update because its not their camo

            //Also if they have max ripple they can't like open karma gates or smth
            //Hook hook2 = new Hook(typeof(Player).GetProperty("maxRippleLevel", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), maxRippleLevel);
            //Hook hook5 = new Hook(typeof(RainWorld).GetProperty("RippleColor", BindingFlags.Static | BindingFlags.Public).GetGetMethod(), newColor);

        }
        public delegate float orig_rippleLevel(Player self);
        //public delegate float orig_maxRippleLevel(Player self);

        //public delegate Color orig_RippleColor();
        /*public static Color newColor(orig_RippleColor orig)
        {
            return Color.yellow;
        }*/

        public static float rippleLevel(orig_rippleLevel orig, Player self)
        {
            if (Custom.rainWorld.ExpeditionMode)
            {
                if (ExpeditionData.slugcatPlayer != WatcherEnums.SlugcatStatsName.Watcher)
                {
                    if (self.SlugCatClass == WatcherEnums.SlugcatStatsName.Watcher)
                    {
                        return 3f;
                    }
                }
            }
            return orig(self);
        }
        /*public static float maxRippleLevel(orig_maxRippleLevel orig, Player self)
        {
            if (Custom.rainWorld.ExpeditionMode)
            {
                if (ExpeditionData.slugcatPlayer != WatcherEnums.SlugcatStatsName.Watcher)
                {
                    if (self.SlugCatClass == WatcherEnums.SlugcatStatsName.Watcher)
                    {
                        return 0.5f;
                    }
                }
            }
            return orig(self);
        }*/
    }
}
