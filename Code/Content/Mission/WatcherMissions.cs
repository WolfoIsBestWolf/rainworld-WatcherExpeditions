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
    public class WatcherMissions
    {

        public static void Start()
        {
            On.Expedition.ExpeditionProgression.ParseMissionFiles += CorrectMissionCategoryName;
            Mission_Rot.Start();

        }


        private static void CorrectMissionCategoryName(On.Expedition.ExpeditionProgression.orig_ParseMissionFiles orig)
        {
            orig();
            Debug.Log("Fix Mission Category");

            var mod = ModManager.GetModById("WatcherExpeditions");
            Debug.Log(mod);
            Debug.Log(mod.name);
            string dir = Path.GetFileName(mod.path).ToLowerInvariant();
            Debug.Log(dir);
            
            if (ExpeditionProgression.customMissions.ContainsKey(dir))
            {
                if (!ExpeditionProgression.customMissions.ContainsKey(mod.id))
                {
                    var pair = ExpeditionProgression.customMissions[dir];
                    ExpeditionProgression.customMissions.Add(mod.id, pair);
                }           
                ExpeditionProgression.customMissions.Remove(dir);
            }

        }
 
     }
}