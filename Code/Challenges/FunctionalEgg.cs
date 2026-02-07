using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;
using UnityEngine;
using System.Linq;

namespace WatcherExpeditions
{
    public class FunctionalEgg
    {
        public static void Start()
        {
            On.Expedition.ExpeditionCoreFile.FromString += ExpeditionCoreFile_FromString;
            On.Menu.StatsDialog.ResetAll_OnPressDone += StatsDialog_ResetAll_OnPressDone;
            if (!ExpeditionGame.ePos.ContainsKey("V0FSQV9QMTc=")) ExpeditionGame.ePos.Add("V0FSQV9QMTc=", new Vector2(90f, 629f));
            /*Debug.Log("Egg Spots :"+ExpeditionGame.ePos.Count);
            for (int i = 0; i < ExpeditionGame.ePos.Count; i++)
            {
                Debug.Log(ExpeditionGame.ePos.Keys.ElementAt(i));
            }*/
        }

        private static void StatsDialog_ResetAll_OnPressDone(On.Menu.StatsDialog.orig_ResetAll_OnPressDone orig, Menu.StatsDialog self, Menu.Remix.MixedUI.UIfocusable trigger)
        {
            orig(self, trigger);
            LongerArray();
        }

        public static void LongerArray()
        {
            int[] colors = new int[20];
            for (int i = 0; i < ExpeditionData.ints.Length; i++)
            {
                colors[i] = ExpeditionData.ints[i];
            }
            ExpeditionData.ints = colors;
        }

        private static void ExpeditionCoreFile_FromString(On.Expedition.ExpeditionCoreFile.orig_FromString orig, ExpeditionCoreFile self, string saveString)
        {
            orig(self, saveString);
            LongerArray();
        }


    }
}