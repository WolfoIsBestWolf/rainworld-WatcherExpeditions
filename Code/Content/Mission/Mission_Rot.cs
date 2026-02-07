using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;
using JetBrains.Annotations;
using RWCustom;
using UnityEngine;
using System.IO;

namespace WatcherExpeditions
{
    public class Mission_Rot
    {
 
        public static void Start()
        {
          
            //On.StoryGameSession.ctor += StoryGameSession_ctor;    

            On.Expedition.ExpeditionProgression.MissionFromJson += ExpeditionProgression_MissionFromJson;

        }

 
      

        private static ExpeditionProgression.Mission ExpeditionProgression_MissionFromJson(On.Expedition.ExpeditionProgression.orig_MissionFromJson orig, string jsonPath)
        {
            var temp = orig(jsonPath);
            if (temp.key == "WEM_rot")
            {
                var a = new ST_EchoChallenge();
                a.FromString("WARF><0><0><0");
                temp.challenges.Add(a);

                /*var b= new InfestRegion_Challenge();
                b.FromString("WARD><0><0><0");
                temp.challenges.Add(b);*/
            }

            return temp;
        }
 

     }
}