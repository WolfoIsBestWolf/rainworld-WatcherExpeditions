using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using System.Linq;
using RWCustom;
using Menu;
using Expedition;
using MonoMod.Cil;
using BepInEx;
using Mono.Cecil.Cil;
using System;
using UnityEngine;

namespace WatcherExpeditions
{
    public class WE_Core
    {
        public static void Start()
        {
            On.SaveState.ctor += WatcherExpeditionStart;
            On.Expedition.ExpeditionGame.PrepareExpedition += ExpeditionGame_PrepareExpedition;
            On.RainWorldGame.GoToDeathScreen += WatcherDeath;

            On.SlugcatStats.SlugcatStoryRegions += WatcherRegions_SlugcatStats_SlugcatStoryRegions;
            On.Expedition.ExpeditionGame.ExpeditionRandomStarts += WatcherShelters_ExpeditionGame_ExpeditionRandomStarts;
           
            On.Expedition.ChallengeTools.GenerateCreatureScores += WatcherCreatures;
            On.Expedition.ChallengeTools.AppendAdditionalCreatureSpawns += AddAdditional;
            IL.Expedition.ChallengeTools.ParseCreatureSpawns += AddRotRegionsForCreatureSpawns;

            On.SaveState.SessionEnded += SaveChallengeProgressAfterST;
       
            On.Expedition.ExpeditionGame.IsUndesirableRoomScript += RemoveWatcherRoomScripts;

            
            IL.Player.ClassMechanicsArtificer += WatcherExplosiveJumpFix;


            IL.Menu.KarmaLadder.KarmaSymbol.Update += EvilRippleSymbolWhenAboutToDie;
            IL.OverWorld.InitiateSpecialWarp_WarpPoint += RemoveForcedWORA;
        }

        private static void RemoveForcedWORA(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchCallvirt("MiscWorldSaveData", "get_highestPrinceConversationSeen")))
            {
                c.EmitDelegate<Func<int, int>>((convos) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        return 20000;
                    }
                    return convos;
                });
            }
            else
            {
                Debug.Log(": RemoveForcedWORA fail");
            }
        }

        private static void EvilRippleSymbolWhenAboutToDie(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.Before,
              x => x.MatchCallvirt("RainWorld", "get_ExpeditionMode"));
            c.TryGotoNext(MoveType.After,
                x => x.MatchLdfld("Menu.KarmaLadder", "moveToKarma"));
            if (c.TryGotoPrev(MoveType.After,
                x => x.MatchLdarg(0)))
            {
                c.EmitDelegate<Func<KarmaLadder.KarmaSymbol, KarmaLadder.KarmaSymbol>>((karmaSymbol) =>
                {
                    //Debug.Log("WATCHTHIS"+karmaSymbol.displayKarma.x);
                    //Seems to only run in Expd mode on it's own so that's good
                    if (karmaSymbol.rippleMode && karmaSymbol.displayKarma.x == karmaSymbol.parent.minRipple)
                    {
                        if (karmaSymbol.parent.moveToKarma == karmaSymbol.parent.minRipple)
                        {
                            karmaSymbol.waitForAnimate++;
                            if (karmaSymbol.waitForAnimate == 49)
                            {
                                karmaSymbol.parent.ticksInPhase = -1;
                                karmaSymbol.parent.phase = KarmaLadder.Phase.CapBump;
                            }
                            else
                            {
                                karmaSymbol.pulsateCounter++;
                            }
                        }
                    }
                    return karmaSymbol;
                });
               Debug.Log(": EvilRippleSymbolWhenAboutToDie");
            }
            else
            {
                Debug.Log(": EvilRippleSymbolWhenAboutToDie fail");
            }
        }

        private static void WatcherDeath(On.RainWorldGame.orig_GoToDeathScreen orig, RainWorldGame self)
        {
            if (ModManager.Expedition && self.rainWorld.ExpeditionMode)
            {
                if (self.StoryCharacter == WatcherEnums.SlugcatStatsName.Watcher)
                {
                    var death = self.GetStorySession.saveState.deathPersistentSaveData;
                    death.karma = (int)((death.rippleLevel - death.minimumRippleLevel) * 2f);
                    /* Debug.Log(death.karma);
                       Debug.Log(death.rippleLevel);
                       Debug.Log(death.minimumRippleLevel);
                       Debug.Log(death.rippleLevel - death.minimumRippleLevel);*/
                }
            }
            orig(self);
        }

        private static void ExpeditionGame_PrepareExpedition(On.Expedition.ExpeditionGame.orig_PrepareExpedition orig)
        {
            orig();
            if (ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
            {
                ExpeditionGame.tempKarma = WConfig.cfgWatcher_StartingRipple.Value - WConfig.cfgWatcher_StartingRippleMin.Value;
            }
        }

          

        private static void WatcherExplosiveJumpFix(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdfld("Player/InputPackage", "spec")))
            {
                //c.Index += 1;
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<bool, Player, bool>>((input, slug) =>
                {
                    if (slug.SlugCatClass == WatcherEnums.SlugcatStatsName.Watcher)
                    {
                        return false;
                    }
                    return input;
                });
                UnityEngine.Debug.Log(": WatcherExplosiveJumpFix");
            }
            else
            {
                UnityEngine.Debug.Log(": WatcherExplosiveJumpFix fail");
            }
        }

       
        private static bool RemoveWatcherRoomScripts(On.Expedition.ExpeditionGame.orig_IsUndesirableRoomScript orig, UpdatableAndDeletable item)
        {
            if (item is WatcherRoomSpecificScript.WAUA_TOYS || item is WatcherRoomSpecificScript.WAUA_BATH || item is WatcherRoomSpecificScript.WORA_AI || item is WatcherRoomSpecificScript.WORA_DESERT6 || item is WatcherRoomSpecificScript.WORA_KarmaSigils)
            {
                return true;
            }
            return orig(item);
        }

       

        private static void AddAdditional(On.Expedition.ChallengeTools.orig_AppendAdditionalCreatureSpawns orig)
        {
            orig();
            int num2;
            ChallengeTools.ExpeditionCreature item2 = new ChallengeTools.ExpeditionCreature
            {
                creature = WatcherEnums.CreatureTemplateType.FireSprite,
                points = (ChallengeTools.creatureScores.TryGetValue(WatcherEnums.CreatureTemplateType.FireSprite.value, out num2) ? num2 : 0),
                spawns = 12
            };
            ChallengeTools.ExpeditionCreature item = new ChallengeTools.ExpeditionCreature
            {
                creature = DLCSharedEnums.CreatureTemplateType.BigJelly,
                points = (ChallengeTools.creatureScores.TryGetValue(DLCSharedEnums.CreatureTemplateType.BigJelly.value, out num2) ? num2 : 0),
                spawns = 1
            };
            ChallengeTools.ExpeditionCreature item3 = new ChallengeTools.ExpeditionCreature
            {
                creature = WatcherEnums.CreatureTemplateType.Rattler,
                points = (ChallengeTools.creatureScores.TryGetValue(WatcherEnums.CreatureTemplateType.Rattler.value, out num2) ? num2 : 0),
                spawns = 8
            };
            if (ChallengeTools.creatureSpawns.ContainsKey(WatcherEnums.SlugcatStatsName.Watcher.value))
            {
                ChallengeTools.creatureSpawns[WatcherEnums.SlugcatStatsName.Watcher.value].Add(item2);
                if (WConfig.cfgHunt_Rattler.Value)
                {
                    ChallengeTools.creatureSpawns[WatcherEnums.SlugcatStatsName.Watcher.value].Add(item3);
                }         
                ChallengeTools.creatureSpawns[WatcherEnums.SlugcatStatsName.Watcher.value].Add(item);
            }
        }

        private static void AddRotRegionsForCreatureSpawns(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.After,
                x => x.MatchCallOrCallvirt("System.Collections.Generic.List`1<System.String>", "RemoveAll"));

            if (c.TryGotoPrev(MoveType.After,
                x => x.MatchLdloc(2)))
            {
                //c.Index += 1;
                c.EmitDelegate< System.Func<List<string>, List<string>> >((list) =>
                {
                    if (list.Contains("WARB"))
                    {
                        if (WConfig.cfgWatcher_RotEnemies.Value)
                        {
                            list.Add("WDSR");
                            list.Add("WHIR");
                            list.Add("WGWR");
                            list.Add("WSUR");
                        }
                        
                    }
                    return list;
                });
                UnityEngine.Debug.Log("wa: AddRotRegionsForCreatureSpawns");
            }
            else
            {
                UnityEngine.Debug.Log("wa: AddRotRegionsForCreatureSpawns");
            }
        }

        private static void SaveChallengeProgressAfterST(On.SaveState.orig_SessionEnded orig, SaveState self, RainWorldGame game, bool survived, bool newMalnourished)
        {
            orig(self, game, survived, newMalnourished);    

            if (self.sessionEndingFromSpinningTopEncounter)
            {
                if (ModManager.Expedition && game.rainWorld.ExpeditionMode && !game.GetStorySession.saveState.malnourished && ExpeditionData.challengeList != null)
                {
                    Expedition.Expedition.coreFile.Save(false);
                }
            }
        }

        private static void WatcherCreatures(On.Expedition.ChallengeTools.orig_GenerateCreatureScores orig, ref Dictionary<string, int> dict)
        {
            orig(ref dict);
            if (ModManager.Watcher)
            {
                var newDict = new Dictionary<string, int>
                {
                    {//3 hp, not too dangerous. Probably still more dangerous than a green Lizard that is 10 points
                        "DrillCrab",
                        12
                    },
                   { //Killable & Edible
                        "SandGrub",
                        4
                    },
                    //SmallMoth (WAUA only)
                    { //Apparently easily killabe from the top
                        "Rattler",
                        5
                    },
                    //SkyWhale
                    {
                        "FireSprite",
                        8
                    },
                    {
                        "ScavengerTemplar",
                        14
                    },
                    {
                        "ScavengerDisciple",
                        15
                    },
                    //Loach
                    //RotLoach
                    {
                        "BlizzardLizard",
                        25
                    },
                    {
                        "BasiliskLizard",
                        9
                    },
                    {
                        "IndigoLizard",
                        10
                    },
                    {
                        "Rat",
                        1
                    },
                    {
                        "Frog",
                        4
                    },
                    {
                        "Tardigrade",
                        4
                    }
                };


                if (WConfig.cfgHunt_Barnacle.Value)
                {
                    dict.Add("Barnacle", 5);
                }
                if (WConfig.cfgHunt_BoxWorm.Value)
                {
                    dict.Add("BoxWorm", 22);
                }
                if (WConfig.cfgHunt_BigMoth.Value)
                {
                    dict.Add("BigMoth", 16);
                }
                if (WConfig.cfgHunt_BigSandWorm.Value)
                {
                    dict.Add("BigSandGrub", 11);
                }
           
                foreach (KeyValuePair<string, int> keyValuePair in newDict)
                {
                    if (!dict.ContainsKey(keyValuePair.Key))
                    {
                        dict.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                    else
                    {
                        dict[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
            }
        }

        private static void WatcherExpeditionStart(On.SaveState.orig_ctor orig, SaveState self, SlugcatStats.Name saveStateNumber, PlayerProgression progression)
        {
            orig(self,saveStateNumber, progression);
            if (ModManager.Expedition && Custom.rainWorld.ExpeditionMode && saveStateNumber == WatcherEnums.SlugcatStatsName.Watcher)
            {
                self.miscWorldSaveData.camoTutorialCounter++;
                self.miscWorldSaveData.usedCamoAbility++;
                self.miscWorldSaveData.cycleFirstStartedWarpJourney++;
                self.miscWorldSaveData.stableWarpTutorialCounter = 5;
                self.miscWorldSaveData.badWarpTutorialCounter++;
                self.miscWorldSaveData.warpFatigueTutorialCounter++;
                self.miscWorldSaveData.warpExhaustionTutorialCounter = 5;
         

                self.deathPersistentSaveData.spinningTopRotEncounter = true;     
                self.miscWorldSaveData.numberOfPrinceEncounters = 5;
                //This would remove all convos, idk if that's good
                //self.miscWorldSaveData.highestPrinceConversationSeen = 2000; //Idk dude fuck this guy
                self.miscWorldSaveData.visitedShopRoom = true; //Makes it so you spawn in the middle of Ancient Urban

                self.miscWorldSaveData.seenSpinningTopDream = true; //Dreams are generally disabled I think?
                self.miscWorldSaveData.seenRotDream = true;

                

                //self.deathPersistentSaveData.rippleLevel = (WConfig.cfgWatcher_StartingRipple.Value+1f)/2f;
                self.deathPersistentSaveData.maximumRippleLevel = (WConfig.cfgWatcher_StartingRippleMax.Value + 1f) / 2f; ;
                self.deathPersistentSaveData.minimumRippleLevel = (WConfig.cfgWatcher_StartingRippleMin.Value + 1f) / 2f; ;
                self.deathPersistentSaveData.rippleLevel = self.deathPersistentSaveData.minimumRippleLevel + ExpeditionGame.tempKarma / 2f;
                self.deathPersistentSaveData.karmaCap = 9;
                /*if (self.deathPersistentSaveData.rippleLevel < self.deathPersistentSaveData.minimumRippleLevel)
                {
                    self.deathPersistentSaveData.rippleLevel = self.deathPersistentSaveData.minimumRippleLevel;
                }
                else if (self.deathPersistentSaveData.rippleLevel > self.deathPersistentSaveData.maximumRippleLevel)
                {
                    self.deathPersistentSaveData.rippleLevel = self.deathPersistentSaveData.maximumRippleLevel;
                }*/

                self.deathPersistentSaveData.spinningTopEncounters.Add(39);//WAUA_TOYS ST
                self.deathPersistentSaveData.spinningTopEncounters.Add(15);//CC
                self.deathPersistentSaveData.spinningTopEncounters.Add(16);//SL
                self.deathPersistentSaveData.spinningTopEncounters.Add(17);//FH

            }
        }

        private static string WatcherShelters_ExpeditionGame_ExpeditionRandomStarts(On.Expedition.ExpeditionGame.orig_ExpeditionRandomStarts orig, RainWorld rainWorld, SlugcatStats.Name slug)
        {
            if (slug == WatcherEnums.SlugcatStatsName.Watcher)
            {
                List<string> shelters = new List<string>
                {
                    /*"wara_s22",
                    "wara_s23",
                    "wara_s24",*/
                    "warb_s11",
                    "warb_s15",
                    "warb_s17",
                    "warb_s29",
                    "warc_s01",
                    "warc_s02",
                    "warc_s03",
                    "warc_s04",
                    "warc_s05",
                    "warc_s06",
                    "ward_s07",
                    "ward_s09",
                    "ward_s13",
                    "ward_s19",
                    "ward_s23",
                    "ware_s05",
                    "ware_s12",
                    "ware_s25",
                    "ware_s28",
                    "ware_s30",
                    "warf_s01",
                    "warf_s02",
                    "warf_s03",
                    "warf_s04",
                    "warf_s06",
                    "warf_s08",
                    "warf_s14",
                    "warf_s18",
                    "warg_s10",
                    "warg_s16",
                    "warg_s20",
                    "warg_s21",
                    "warg_s26",
                    "warg_s27",
                     /*"waua_s01",
                    "waua_s01b",
                    "waua_s02b",*/
                    "wbla_s01",
                    "wbla_s02",
                    "wbla_s03",
                    //"wora_s01",
                    "wpta_s01",
                    "wpta_s02",
                    "wpta_s03",
                    "wrfa_s01",
                    "wrfa_s02",
                    "wrfa_s07",
                    "wrfa_s08",
                    "wrfb_s03",
                    "wrfb_s04",
                    "wrfb_s06",
                    "wrra_s01",
                    "wrra_s02",
                    "wrra_s03",
                    "wrra_s04",
                    "wrra_s05",
                    "wrra_s06",
                    "wska_s07",
                    "wska_s08",
                    "wska_s09",
                    "wskb_s06",
                    "wskc_s01",
                    "wskc_s02",
                    "wskd_s03",
                    "wskd_s04",
                    "wskd_s05",
                    "wtda_s02",
                    "wtda_s03",
                    "wtda_s04",
                    "wtdb_s01",
                    "wtdb_s02",
                    "wtdb_s03",
                    "wvwa_s01",
                    "wvwa_s02",
                    "wvwa_s03",
                };
                
                if (WConfig.cfgWatcher_RotShelter.Value)
                {
                    string[] rotShelters = new string[]
                    {
                         //"wssr_s03",
                        "wdsr_s04",
                        "wsur_s04",
                        "wsur_s06",
                        "whir_s01",
                        "whir_s02",
                        "whir_s03",
                        "whir_s04",
                        "whir_s05",
                        "wgwr_s01",
                        "wgwr_s06",
                        "wgwr_s08",
                    };
                    shelters.AddRange(rotShelters);
                }
           

                System.Random random = new System.Random();
                int randomIndex = random.Next(0, shelters.Count);
                return shelters[randomIndex].ToUpperInvariant();
            }
            return orig(rainWorld, slug);
        }

        private static List<string> WatcherRegions_SlugcatStats_SlugcatStoryRegions(On.SlugcatStats.orig_SlugcatStoryRegions orig, SlugcatStats.Name i)
        {
            if (i == WatcherEnums.SlugcatStatsName.Watcher)
            {
                string[] source = new string[]
               {
                    //"WARA", //Shattered Terrance (Pre-Final area)
                    "WARB", //Salination
                    "WARC", //Fetid Glen
                    "WARD", //Cold Storage
                    "WARE", //Heat Ducts
                    "WARF", //Aether Ridge
                    "WARG", //The Surface
                    //"WAUA", //Ancient Urban (Final area)
                    "WBLA", //Badlands
                    //"WORA", //Outer Rim
                    "WPTA", //Signal Spires
                    "WRFA", //Coral Caves
                    "WRFB", //Turbulent Pump
                    "WRRA", //Rusted Wrecks
                    //"WRSA", //Daemon
                    "WSKA", //Torrential Railways
                    "WSKB", //Sunlit Port
                    "WSKC", //Stormy Coast
                    "WSKD", //Shrouded Coast
                    //"WSSR", //Unfortunate Evolution
                    //"WDSR", //Drainage - Rot
                    //"WGWR", //Garbage - Rot
                    //"WHIR", //Industrial - Rot
                    //"WSUR", //Outskirts - Rot
                    "WTDA", //Torrid Desert
                    "WTDB", //Desolate Tract
                    "WVWA", //Verdant Waterways
                };
                return source.ToList<string>();
            }
            return orig(i);
        }

    

         
    }
}