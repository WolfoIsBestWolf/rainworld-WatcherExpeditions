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
    public class RotMissions
    {
        public static bool Added = false;
        public static void Start()
        {
            On.Menu.ChallengeSelectPage.StartGame += ChallengeSelectPage_StartGame;
            On.Menu.CharacterSelectPage.LoadGame += CharacterSelectPage_LoadGame;
            //On.StoryGameSession.ctor += StoryGameSession_ctor;    

            On.Expedition.ExpeditionProgression.MissionFromJson += ExpeditionProgression_MissionFromJson;
        }

        private static void ChallengeSelectPage_StartGame(On.Menu.ChallengeSelectPage.orig_StartGame orig, Menu.ChallengeSelectPage self)
        {
            orig(self);
            AddRotCode();
        }
        private static void CharacterSelectPage_LoadGame(On.Menu.CharacterSelectPage.orig_LoadGame orig, Menu.CharacterSelectPage self)
        {
            orig(self);
            AddRotCode();
        }
        public static void AddRotCode()
        {
            if (ExpeditionData.activeMission == "WEM_rot")
            {
                if (!Added)
                {
                    Debug.Log("Adding Rot_World_Mission");
                    Added = true;
                    On.World.LoadWorld_Timeline_List1_Int32Array_Int32Array_Int32Array += World_LoadWorld_Timeline_List1_Int32Array_Int32Array_Int32Array;

                }
            }
        }


       
      

        private static ExpeditionProgression.Mission ExpeditionProgression_MissionFromJson(On.Expedition.ExpeditionProgression.orig_MissionFromJson orig, string jsonPath)
        {
            var temp = orig(jsonPath);
            if (temp.key == "WEM_rot")
            {
                var a = new ST_EchoChallenge();
                a.FromString("WARD><0><0><0");
                temp.challenges.Add(a);

                /*var b= new InfestRegion_Challenge();
                b.FromString("WARD><0><0><0");
                temp.challenges.Add(b);*/
            }

            return temp;
        }
 

        private static void World_LoadWorld_Timeline_List1_Int32Array_Int32Array_Int32Array(On.World.orig_LoadWorld_Timeline_List1_Int32Array_Int32Array_Int32Array orig, World self, SlugcatStats.Timeline timelinePosition, List<AbstractRoom> abstractRoomsList, int[] swarmRooms, int[] shelters, int[] gates)
        {
            //RotWorld(self);
            //Debug.Log("Rot_World_Mission : World_LoadWorld1"); 
            orig(self, timelinePosition, abstractRoomsList, swarmRooms, shelters, gates);
            //Debug.Log("Rot_World_Mission : World_LoadWorld2");
            RotWorld(self);
        }


        public static void RotWorld(World world)
        {
            if (world == null)
            {
                Debug.Log("Rot_World_Mission : Null World");
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
            if (world.game.GetStorySession.saveState == null)
            {
                Debug.Log("Rot_World_Mission : Null saveState");
                return;
            }
            if (!Custom.rainWorld.ExpeditionMode)
            {
                Debug.Log("Rot_World_Mission : Remove");
                Added = false;
                On.World.LoadWorld_Timeline_List1_Int32Array_Int32Array_Int32Array -= World_LoadWorld_Timeline_List1_Int32Array_Int32Array_Int32Array;
                return;
            }
            if (ExpeditionData.activeMission != "WEM_rot")
            {
                Debug.Log("Rot_World_Mission : Remove");
                Added = false;
                On.World.LoadWorld_Timeline_List1_Int32Array_Int32Array_Int32Array -= World_LoadWorld_Timeline_List1_Int32Array_Int32Array_Int32Array;
                return;
            }
            if (Region.HasSentientRotResistance(world.name))
            {
                return;
            }
            //Debug.Log("Rot_World_Mission");
            if (!world.game.GetStorySession.saveState.miscWorldSaveData.regionsInfectedBySentientRot.Contains(world.name.ToLowerInvariant()))
            {
                Debug.Log("Rot_World_Mission : New world " + world.name);
                world.game.GetStorySession.saveState.miscWorldSaveData.regionsInfectedBySentientRot.Add(world.name.ToLowerInvariant());
                for (int i = 0; world.abstractRooms.Length > i; i++)
                {
                    InfectRegionFaster(world.regionState, 10, world.abstractRooms[i].name);
                }
            }
            else
            {
                Debug.Log("Rot_World_Mission : Already Rot " + world.name);
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
     }
}