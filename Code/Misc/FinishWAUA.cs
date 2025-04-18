using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;
using System;
using HUD;
using RWCustom;
using UnityEngine;

namespace WatcherExpeditions
{
    public class FinishWAUA
    {
        public static void Start()
        {
            IL.Room.Loaded += AddDepthsScript;
            On.Expedition.DepthsFinishScript.Update += DepthsFinishScript_Update;
            On.HUD.ExpeditionHUD.Update += AddWarning_ExpeditionHUD_Update;

            //IL.OverWorld.InitiateSpecialWarp_WarpPoint += OverwrideWAUADestination;
            IL.Room.Loaded += AddToysInExpeditionModeVeryImportant;

            On.SpinningTopData.FromString += AlwaysSpawnST_WARA;
        }

        private static void AlwaysSpawnST_WARA(On.SpinningTopData.orig_FromString orig, SpinningTopData self, string s)
        {
            orig(self, s);
            if (Custom.rainWorld.ExpeditionMode)
            {
                if(self.spawnIdentifier == 1)
                {
                    //WARA is 1
                    if (self.rippleWarp)
                    {
                        self.rippleWarp = false;
                        self.destRoom = "WAUA_E02B";
                        self.destPos = new Vector2?(new Vector2(480f, 360f));
                    }
                }
              
               
            }
        }

        private static void OverwrideWAUADestination(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdstr("WAUA_B02B")))
            {
                c.EmitDelegate<Func<string, string>>((room) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        return "WAUA_E02B";
                    }
                    return room;
                });
            }
            if (c.TryGotoNext(MoveType.After,
           x => x.MatchLdcR4(550f)))
            {
                c.EmitDelegate<Func<float, float>>((room) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        return 480f;
                    }
                    return room;
                });
            }
          


           /*if (c.TryGotoNext(MoveType.Before,
           x => x.MatchStfld("Watcher.WarpPoint/WarpPointData","destPos")))
            {
                c.EmitDelegate<Func<Vector2, Vector2>>((room) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        return new Vector2(480f,360f);
                    }
                    return room;
                });
            }*/
        }

        private static void AddToysInExpeditionModeVeryImportant(ILContext il)
        {
            ILCursor c = new(il);
            c.Index = 4900;
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdfld("DeathPersistentSaveData", "sawVoidBathSlideshow")))
            {
                c.EmitDelegate<Func<bool, bool>>((room) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        return true;
                    }
                    return room;
                });
            }
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdfld("DeathPersistentSaveData", "sawVoidBathSlideshow")))
            {
                c.EmitDelegate<Func<bool, bool>>((room) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        return true;
                    }
                    return room;
                });
            }
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdfld("DeathPersistentSaveData", "sawVoidBathSlideshow")))
            {
                c.EmitDelegate<Func<bool, bool>>((room) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        return true;
                    }
                    return room;
                });
            }
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdfld("DeathPersistentSaveData", "sawVoidBathSlideshow")))
            {
                c.EmitDelegate<Func<bool, bool>>((room) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        return true;
                    }
                    return room;
                });
            }
        }

        private static void AddWarning_ExpeditionHUD_Update(On.HUD.ExpeditionHUD.orig_Update orig, HUD.ExpeditionHUD self)
        {
            orig(self);
            if (!self.voidSeaWarning && ExpeditionData.activeMission == "" && self.hud.owner.GetOwnerType() == HUD.HUD.OwnerType.Player)
            {
                if ((self.hud.owner as Player).room != null && (self.hud.owner as Player).room.world != null && (self.hud.owner as Player).room.world.region.name == "WAUA")
                {
                    //self.hud.textPrompt.AddMessage(Custom.rainWorld.inGameTranslator.Translate("The Expedition will end upon entering the Depths..."), 10, 300, true, true);
                    self.hud.textPrompt.AddMessage("The Expedition will end upon entering the Bedroom...", 10, 600, true, true);
                    self.voidSeaWarning = true;
                  
                }
            }
        }

        private static void DepthsFinishScript_Update(On.Expedition.DepthsFinishScript.orig_Update orig, DepthsFinishScript self, bool eu)
        {
            orig(self, eu);
            if (self.room != null)
            {
                if (!self.triggered)
                {
                    for (int i = 0; i < self.room.updateList.Count; i++)
                    {
                        if (ExpeditionData.activeMission != "")
                        {
                            return;
                        }
                        if (self.room.abstractRoom.name == "WAUA_TOYS" && self.room.updateList[i] is Player && (self.room.updateList[i] as Player).mainBodyChunk.pos.x < 300f && (self.room.updateList[i] as Player).mainBodyChunk.pos.y > 1550)
                        {
                            ExpeditionGame.voidSeaFinish = true;
                            ExpeditionGame.expeditionComplete = true;
                            self.triggered = true;
                        }
                    }
                }
            }
        }

        private static void AddDepthsScript(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdstr("SB_A14")))
            {
                c.EmitDelegate<Func<string, string>>((room) =>
                {
                    if (room == "WAUA_TOYS")
                    {
                        return "SB_A14";
                    }
                    return room;
                });
            }

        }
    }
}