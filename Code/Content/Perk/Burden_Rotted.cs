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
    public class Burden_Rotted : Modding.Expedition.CustomBurden
    {
        public override string ID
        {
            get
            {
                return "bur-watcher_rot";
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
                return ChallengeTools.IGT.Translate("All regions start entirely overtaken and infected with rot. This is meant to be for the challenge only.");
            }
        }
        public override string DisplayName
        {
            get
            {
                return ChallengeTools.IGT.Translate("Rotten");
            }
        }
        public override void ApplyHooks()
        {
            base.ApplyHooks();
            On.World.LoadWorld_Timeline_List1_Int32Array_Int32Array_Int32Array += RottenBurden;
            On.DaddyCorruption.SentientRotMode += DaddyCorruption_SentientRotMode;
            On.Region.ctor_string_int_int_Timeline += Region_ctor_string_int_int_Timeline;
        }

        private void Region_ctor_string_int_int_Timeline(On.Region.orig_ctor_string_int_int_Timeline orig, Region self, string name, int firstRoomIndex, int regionNumber, SlugcatStats.Timeline timelineIndex)
        {
            orig(self, name, firstRoomIndex, regionNumber, timelineIndex);
            if (Custom.rainWorld.ExpeditionMode && ExpeditionGame.activeUnlocks.Contains("bur-watcher_rot"))
            {
                self.regionParams.corruptionEffectColor = RainWorld.RippleColor;
                self.regionParams.corruptionEyeColor = RainWorld.RippleColor;
            }
        }

      
        private bool DaddyCorruption_SentientRotMode(On.DaddyCorruption.orig_SentientRotMode orig, Room rm)
        {
            if (Custom.rainWorld.ExpeditionMode && ExpeditionGame.activeUnlocks.Contains("bur-watcher_rot"))
            {
                return true;
            }
            return orig(rm);
        }

        private static void RottenBurden(On.World.orig_LoadWorld_Timeline_List1_Int32Array_Int32Array_Int32Array orig, World self, SlugcatStats.Timeline timelinePosition, List<AbstractRoom> abstractRoomsList, int[] swarmRooms, int[] shelters, int[] gates)
        {
            orig(self, timelinePosition, abstractRoomsList, swarmRooms, shelters, gates);
            if (Custom.rainWorld.ExpeditionMode && ExpeditionGame.activeUnlocks.Contains("bur-watcher_rot"))
            {
                RotWorld(self);
            }
        }


        public static void RotWorld(World world)
        {
            if (world == null)
            {
                return;
            }
            if (world.game == null)
            {
                Debug.Log("Rot_World_Mission : Null Game");
                return;
            }
            if (world.game.GetStorySession == null)
            {
                Debug.Log("Rot_World_Mission : Null GetStorySession");
                return;
            }
            if (Region.HasSentientRotResistance(world.name))
            {
                return;
            }

            //Debug.Log("Rot_World_Mission");
            //world.game.GetStorySession.saveState.progression.miscProgressionData.beaten_Watcher_SentientRot = true;
            if (!world.game.GetStorySession.saveState.miscWorldSaveData.regionsInfectedBySentientRot.Contains(world.name.ToLowerInvariant()))
            {
                Debug.Log("Rot_World : New world " + world.name);
                world.game.GetStorySession.saveState.miscWorldSaveData.regionsInfectedBySentientRot.Add(world.name.ToLowerInvariant());
                for (int i = 0; world.abstractRooms.Length > i; i++)
                {
                    InfectRegionFaster(world.regionState, 2, world.abstractRooms[i].name);
                }
            }
            else
            {
                Debug.Log("Rot_World : Already Rot " + world.name);
            }

        }




        public static void InfectRegionFaster(RegionState regionState, float amount, string roomName)
        {
            if (!regionState.sentientRotProgression.ContainsKey(roomName))
            {
                RegionState.SentientRotState value = new RegionState.SentientRotState();
                regionState.sentientRotProgression[roomName] = value;
            }
            regionState.sentientRotProgression[roomName].rotIntensity = amount;
        }

        public override string Group
        {
            get
            {
                return "WatcherExpeditions";
            }
        }
        public override float ScoreMultiplier => 25f;
       
    }
}
