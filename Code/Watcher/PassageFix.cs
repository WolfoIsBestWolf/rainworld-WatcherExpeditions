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
using Mono.Cecil.Cil;


namespace WatcherExpeditions
{
    internal class PassageFix
    {
        public static void Start()
        {
            IL.Menu.FastTravelScreen.ctor += FixPassagesWatcher;
            On.Menu.SleepAndDeathScreen.FoodMeterXPos += SleepAndDeathScreen_FoodMeterXPos;
            On.Menu.SleepAndDeathScreen.AddExpeditionPassageButton += SleepAndDeathScreen_AddExpeditionPassageButton;

            On.Menu.FastTravelScreen.SpawnChoiceMenu += FastTravelScreen_SpawnChoiceMenu;
            IL.Menu.SleepAndDeathScreen.AddPassageButton += PassageWatchesVanilla;
            IL.Menu.SleepAndDeathScreen.GetDataFromGame += Passages2;

            //On.MoreSlugcats.CollectiblesTracker.ctor += CollectiblesTracker_ctor;
           

        }


        private static void CollectiblesTracker_ctor(On.MoreSlugcats.CollectiblesTracker.orig_ctor orig, CollectiblesTracker self, Menu.Menu menu, MenuObject owner, Vector2 pos, FContainer container, SlugcatStats.Name saveSlot)
        {
            orig(self,menu,owner,pos,container,saveSlot);
        }

        private static void SleepAndDeathScreen_AddExpeditionPassageButton(On.Menu.SleepAndDeathScreen.orig_AddExpeditionPassageButton orig, SleepAndDeathScreen self)
        {
            if (self.RippleLadderMode)
            {
                return;
            }
            orig(self);
        }

        private static void Passages2(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("ModManager", "Watcher")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<bool,SleepAndDeathScreen, bool>>((self, package) =>
                {
                    if (!package.RippleLadderMode && WConfig.cfgVanillaPassage.Value)
                    {
                        return false;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log("PassageWatchesVanilla Hook Failed");
            }
        }

        private static void FastTravelScreen_SpawnChoiceMenu(On.Menu.FastTravelScreen.orig_SpawnChoiceMenu orig, FastTravelScreen self)
        {
            if (WConfig.cfgVanillaPassage.Value)
            {
                if (self.activeMenuSlugcat == WatcherEnums.SlugcatStatsName.Watcher)
                {
                    if (self.IsFastTravelScreen)
                    {
                        for (int i = self.accessibleRegions.Count - 1; 0 <= i; i--)
                        {
                            Debug.Log(self.manager.rainWorld.progression.regionNames[self.accessibleRegions[i]]);

                            if (Region.IsWatcherVanillaRegion(self.manager.rainWorld.progression.regionNames[self.accessibleRegions[i]]))
                            {
                                self.accessibleRegions.Remove(self.accessibleRegions[i]);
                            }
                        }
                    }
                }
            }
            orig(self);
        }

        private static void PassageWatchesVanilla(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("ModManager", "Watcher")))
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    if (WConfig.cfgVanillaPassage.Value)
                    {
                        return false;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log("PassageWatchesVanilla Hook Failed");
            }
        }

        public static float SleepAndDeathScreen_FoodMeterXPos(On.Menu.SleepAndDeathScreen.orig_FoodMeterXPos orig, SleepAndDeathScreen self, float down)
        {
            if (self.UsesWarpMap)
            {
                return Custom.LerpMap(self.manager.rainWorld.options.ScreenSize.x, 1024f, 1366f, self.manager.rainWorld.options.ScreenSize.x / 2f - 110f, 540f);
            }
            return orig(self, down);
        }

        public static void FixPassagesWatcher(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchCall("Menu.FastTravelScreen", "get_WarpPointModeActive")))
            {
                c.EmitDelegate<System.Func<FastTravelScreen, FastTravelScreen>>((self) =>
                {
                    if (Custom.rainWorld.ExpeditionMode || WConfig.cfgVanillaPassage.Value)
                    {
                        if (self.IsFastTravelScreen)
                        {
                            self.warpPointModeAvailable = false;
                        }
                    }
                    return self;
                });
            }
            else
            {
                UnityEngine.Debug.Log("WatcherExpeditions: FixPassagesWatcher");
            }
        }



    }
}
