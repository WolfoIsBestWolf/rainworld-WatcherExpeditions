using System;
//using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using Expedition;
using Menu;
using Modding.Expedition;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using Watcher;
 

namespace WatcherExpeditions
{
    [BepInPlugin("wolfo.WatcherExpeditions", "WatcherExpeditions", "1.2.4")]
    public class WatcherExpeditions : BaseUnityPlugin
    {
        public static bool initialized = false;
        public static bool initialized_late = false;
        public static bool slugbase = false;
        internal static ManualLogSource? logger;

        public void OnEnable()
        {
            logger = Logger;
            On.RainWorld.OnModsInit += RainWorld_OnModsInit;
            On.RainWorld.PostModsInit += RainWorld_PostModsInit;
        }
        public void OnDisable()
        {
            logger = null;
        }
        private void RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld self)
        {
            orig(self); 
            if (initialized_late)
            {
                return;
            }
            initialized_late = true;
            WLog.Start();
            //SandboxStuff.Start();
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
            AddWatcherRegionArt.Start();
            ST_ExpeditionStuff.Start();

            //ArenaStuff.Start();
            JukeboxStuff.Add();
            //JollyCoopAdditions.Start();
            PassageFix.Start();

            FinishWAUA.Start();
            WatcherMissions.Start();
            NewPerks.Start();
            FunctionalEgg.Start();
 
 
            slugbase = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("slime-cubed.slugbase");
            Debug.Log("WatcherExpeditions Slugbase installed : " + slugbase);

            On.Watcher.PrinceBehavior.PrinceConversation.TargetConversation += PrinceConversation_TargetConversation;
            //On.Watcher.PrinceBehavior.WillingToInspectItem +=

            //IL.PlayerGraphics.ApplyPalette += FixCustomColorsNotWorking;
            Futile.atlasManager.LoadAtlas("atlases/watcher_expedition");
        }
       

        /*private static void FixCustomColorsNotWorking(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("ModManager", "Watcher")))
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    if (WConfig.cfgCustomColorFix.Value)
                    {
                        if (PlayerGraphics.CustomColorsEnabled())
                        {
                            return false;
                        }
                        if (ModManager.CoopAvailable && Custom.rainWorld.options.jollyColorMode == Options.JollyColorMode.CUSTOM)
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
        }*/

        private Conversation.ID PrinceConversation_TargetConversation(On.Watcher.PrinceBehavior.PrinceConversation.orig_TargetConversation orig, int highestConversationSeen, int infections)
        {
            if (Custom.rainWorld.ExpeditionMode)
            {
                return orig(highestConversationSeen, 100);
            }
            return orig(highestConversationSeen, infections);
        }

     
        public static void KarmaFlowerForWatchers(ILContext il)
        {
            ILCursor c = new(il);

            bool one = c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("Preventing natural KarmaFlower spawn"));

            if (one && c.TryGotoPrev(MoveType.After,
                x => x.MatchLdsfld("ModManager", "Expedition")))
            {    
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
            }
            else
            {
                Debug.Log("WatcherExpeditions: Karma Flower Hook Failed");
                Debug.Log(c);
            }
        }

        public int StartingRegionWeight(On.Expedition.ExpeditionGame.orig_GetRegionWeight orig, string region)
        {
            return orig(region);
        }
    }
}
