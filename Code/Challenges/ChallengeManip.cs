using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;

namespace WatcherExpeditions
{
    public class ChallengeManip
    {
        public static void Start()
        {
            On.Expedition.EchoChallenge.Generate += ReplaceEchoChallenges;

            On.Expedition.PearlHoardChallenge.Generate += CommonPearlHoarding;
            On.Expedition.PearlHoardChallenge.Points += PearlHoardChallenge_Points;

            //On.Expedition.HuntChallenge.ValidForThisSlugcat += HuntChallenge_ValidForThisSlugcat;
        }

        private static bool HuntChallenge_ValidForThisSlugcat(On.Expedition.HuntChallenge.orig_ValidForThisSlugcat orig, HuntChallenge self, SlugcatStats.Name slugcat)
        {
            
            return true;
        }

        private static int PearlHoardChallenge_Points(On.Expedition.PearlHoardChallenge.orig_Points orig, PearlHoardChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
            {
                //Since it'd be much harder to keep track of a specific shelter with twice the regions.
                //More Points
                float bonusMult = 1.1f;
                if (self.amount > 2)
                {
                    bonusMult = 1.5f;
                }
                return (int)(orig(self) * bonusMult);
            }
            return orig(self);
        }

        private static Challenge CommonPearlHoarding(On.Expedition.PearlHoardChallenge.orig_Generate orig, PearlHoardChallenge self)
        {
            var temp = orig(self);
            if (ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
            {
                (temp as PearlHoardChallenge).common = true;
            }
            return temp;
        }



        private static Challenge ReplaceEchoChallenges(On.Expedition.EchoChallenge.orig_Generate orig, EchoChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
            {
                //better to disable echo challenges and just have the other echo challenge ig.
                return new EchoChallenge
                {
                    ghost = GhostWorldPresence.GhostID.CC
                };
                return new ST_EchoChallenge().Generate();
            }
            return orig(self);
        }

    }
}