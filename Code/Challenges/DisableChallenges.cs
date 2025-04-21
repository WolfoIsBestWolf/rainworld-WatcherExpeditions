using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;

namespace WatcherExpeditions
{
    public class DisableChallenges
    {
        public static void Start()
        {
            //On.Expedition.ExpeditionGame.ExSpawn += DisableEgg_ExpeditionGame_ExSpawn;
            //On.Expedition.ExpeditionGame.ExIndex += DisableEggExpeditionGame_ExIndex;

            On.Expedition.NeuronDeliveryChallenge.ValidForThisSlugcat += NeuronDeliveryChallenge_ValidForThisSlugcat;
            On.Expedition.PearlDeliveryChallenge.ValidForThisSlugcat += PearlDeliveryChallenge_ValidForThisSlugcat;
           
            On.Expedition.PearlDeliveryChallenge.Generate += PearlDeliveryChallenge_Generate;
            On.Expedition.AchievementChallenge.ValidForThisSlugcat += AchievementChallenge_ValidForThisSlugcat;

            On.Expedition.Challenge.ValidForThisSlugcat += Challenge_ValidForThisSlugcat;
        }

        private static bool Challenge_ValidForThisSlugcat(On.Expedition.Challenge.orig_ValidForThisSlugcat orig, Challenge self, SlugcatStats.Name slugcat)
        {
            if (slugcat == WatcherEnums.SlugcatStatsName.Watcher)
            {
                if (self is EchoChallenge)
                {
                    return false;
                }
            }
            return orig(self, slugcat);
        }

        private static bool AchievementChallenge_ValidForThisSlugcat(On.Expedition.AchievementChallenge.orig_ValidForThisSlugcat orig, AchievementChallenge self, SlugcatStats.Name slugcat)
        {
            if (slugcat == WatcherEnums.SlugcatStatsName.Watcher)
            {
                if (self.ID == WinState.EndgameID.Scholar || self.ID == WinState.EndgameID.Traveller || self.ID == MoreSlugcatsEnums.EndgameID.Nomad || self.ID == MoreSlugcatsEnums.EndgameID.Pilgrim)
                {
                    return false;
                }
            }
            return orig(self, slugcat);
        }

        private static Challenge PearlDeliveryChallenge_Generate(On.Expedition.PearlDeliveryChallenge.orig_Generate orig, PearlDeliveryChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
            {
                //Filler
                return new PearlDeliveryChallenge
                {
                    region = "WAUA"
                };
            }
            return orig(self);
        }


        private static void EchoChallenge_ctor(On.Expedition.EchoChallenge.orig_ctor orig, EchoChallenge self)
        {
            if (ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return;
            }
        }

        private static int DisableEggExpeditionGame_ExIndex(On.Expedition.ExpeditionGame.orig_ExIndex orig, SlugcatStats.Name slug)
        {
            if (slug == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return -1;
            }
            return orig(slug);
        }

        private static void DisableEgg_ExpeditionGame_ExSpawn(On.Expedition.ExpeditionGame.orig_ExSpawn orig, Room room)
        {
            if (ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return;
            }
            else if (ExpeditionData.slugcatPlayer == MoreSlugcatsEnums.SlugcatStatsName.Sofanthiel)
            {
                return;
            }
            orig(room);
        }



        private static bool PearlDeliveryChallenge_ValidForThisSlugcat(On.Expedition.PearlDeliveryChallenge.orig_ValidForThisSlugcat orig, PearlDeliveryChallenge self, SlugcatStats.Name slugcat)
        {
            if (slugcat == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return false;
            }
            return orig(self, slugcat);
        }

        private static bool NeuronDeliveryChallenge_ValidForThisSlugcat(On.Expedition.NeuronDeliveryChallenge.orig_ValidForThisSlugcat orig, NeuronDeliveryChallenge self, SlugcatStats.Name slugcat)
        {
            if (slugcat == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return false;
            }
            return orig(self, slugcat);
        }

    

  
    }
}