using BepInEx;
using Expedition;
using Modding.Expedition;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MoreSlugcats;
using RWCustom;
//using System;
using System.Collections.Generic;
using UnityEngine;
using Watcher;
using Menu;
using System;

namespace WatcherExpeditions
{
    [BepInPlugin("wolfo.WatcherExpeditions", "WatcherExpeditions", "1.0.1")]
    public class WatcherExpeditions : BaseUnityPlugin
    {
        public static bool initialized = false;
        public static bool slugbase = false;
        public void OnEnable()
        {
            On.RainWorld.OnModsInit += RainWorld_OnModsInit;
            On.RainWorld.PostModsInit += RainWorld_PostModsInit;
        }

        private void RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld self)
        {
            orig(self);
            WLog.Start();
            SandboxStuff.Start();
        }

        public void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            if (initialized)
            {
                return;
            }
            initialized = true;
            MachineConnector.SetRegisteredOI("WatcherExpeditions", WConfig.instance);
            UnityEngine.Debug.Log("WatcherExpeditions: Mod Loaded");

            IL.Room.Loaded += KarmaFlowerForWatchers;
 
            //On.Expedition.ExpeditionGame.GetRegionWeight += StartingRegionWeight;
           
            WE_Core.Start();
            DisableChallenges.Start();
            ChallengeManip.Start();
            AddWatcherToMenu.Start();
            ST_ExpeditionStuff.Start();

          
            JukeboxStuff.Add();
            JollyCoopAdditions.Start();
            PassageFix.Start();

            FinishWAUA.Start();
            WatcherMissions.Start();
            NewPerks.Start();
            FunctionalEgg.Start();
 
 
            slugbase = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("slime-cubed.slugbase");
            Debug.Log("WatcherExpeditions Slugbase installed : " + slugbase);

            On.Watcher.PrinceBehavior.PrinceConversation.TargetConversation += PrinceConversation_TargetConversation;
            //On.Watcher.PrinceBehavior.WillingToInspectItem +=

            IL.PlayerGraphics.ApplyPalette += FixCustomColorsNotWorking;
        }

    

        private static void FixCustomColorsNotWorking(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("ModManager", "Watcher")))
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    if (WConfig.cfgCustomColorFix.Value)
                    {
                        if (Custom.rainWorld.options.jollyColorMode == Options.JollyColorMode.CUSTOM)
                        {
                            return false;
                        }
                        if (PlayerGraphics.CustomColorsEnabled())
                        {
                            return false;
                        }
                    }
                    
                    return self;
                });
            }
            else
            {
                Debug.Log("PassageWatchesVanilla Hook Failed");
            }
        }

        private Conversation.ID PrinceConversation_TargetConversation(On.Watcher.PrinceBehavior.PrinceConversation.orig_TargetConversation orig, int highestConversationSeen, int infections)
        {
            if (Custom.rainWorld.ExpeditionMode)
            {
                return orig(highestConversationSeen, 100);
            }
            return orig(highestConversationSeen, infections);
        }

        private void AddBoomerang(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("unl-lantern")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<string, Room, string>>((unlock, room) =>
                {
                    if (unlock == "unl-watcher_boomerang")
                    {
                        WorldCoordinate pos12 = new WorldCoordinate(room.abstractRoom.index, room.shelterDoor.playerSpawnPos.x, room.shelterDoor.playerSpawnPos.y, 0);
                        AbstractPhysicalObject abstractPhysicalObject13 = new AbstractPhysicalObject(room.world, WatcherEnums.AbstractObjectType.Boomerang, null, pos12, room.game.GetNewID());
                        room.abstractRoom.entities.Add(abstractPhysicalObject13);
                        abstractPhysicalObject13.Realize();
                    }
                    return unlock;
                });
            }
            else
            {
                UnityEngine.Debug.Log("WatcherExpeditions: Spawn Boomerang");
            }
        }

        public static void KarmaFlowerForWatchers(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Preventing natural KarmaFlower spawn")))
            {
                c.TryGotoPrev(MoveType.After,
                x => x.MatchLdsfld("ModManager", "Expedition"));

                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<bool, Room, bool>>((karma, room) =>
                {
                    //Maybe if WORA just always allow, how would we get that tho
                    if (ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
                    {
                        if (WConfig.cfgWatcher_KarmaFlower.Value)
                        {
                            return false;
                        }
                        else if (Region.HasSentientRotResistance(room.world.name))
                        {
                            return false;
                        }
                    }
                    return karma;
                }); 
                /*c.EmitDelegate<System.Func<bool, bool>>((karma) =>
                {
                    //Maybe if WORA just always allow, how would we get that tho
                    if (WConfig.cfgWatcher_KarmaFlower.Value && ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
                    {
                        return false;
                    }
                   
                    return karma;
                });*/

                //c.Next.OpCode = OpCodes.Brtrue_S;
                //UnityEngine.Debug.Log("WatcherExpeditions: Karma Flower Hook Succeeded");
            }
            else
            {
                UnityEngine.Debug.Log("WatcherExpeditions: Karma Flower Hook Failed");
            }
        }

        public int StartingRegionWeight(On.Expedition.ExpeditionGame.orig_GetRegionWeight orig, string region)
        {
            return orig(region);
        }




       

      
    }
}
