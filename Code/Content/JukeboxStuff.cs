using BepInEx;
using UnityEngine;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using System.Linq;
using System.Collections.Generic;
using MoreSlugcats;
using Expedition;
using RWCustom;
using Menu.Remix;
using Menu;
using UnityEngine.UIElements;
using HarmonyLib;
 
namespace WatcherExpeditions
{
    public class JukeboxStuff
    {
        public static string FirstTrack;
        public static string LastTrack;
        public static bool dump = false;
        public static Dictionary<string, string>? tempDict;


        public static void Add()
        {
            On.Menu.MusicTrackButton.ctor += UnlockSongs;
            On.Expedition.ExpeditionProgression.GetUnlockedSongs += WatcherSongs;
            On.Expedition.ExpeditionProgression.TrackName += WatcherTrackName;

            On.Menu.MusicTrackContainer.ctor += MusicTrackContainer_ctor;
        }

        private static void MusicTrackContainer_ctor(On.Menu.MusicTrackContainer.orig_ctor orig, MusicTrackContainer self, Menu.Menu menu, MenuObject owner, Vector2 pos, List<string> trackFilenames)
        {
            tempDict = ExpeditionProgression.GetUnlockedSongs();
            orig(self, menu, owner, pos, trackFilenames);
            tempDict = null;
        }

        private static string WatcherTrackName(On.Expedition.ExpeditionProgression.orig_TrackName orig, string filename)
        {
            if (filename == "BM_RWTW_BLUEENVOI")
            {
                return "Blue Envoy";
            }
            if (filename == "RWTW_ST_ELSE_01")
            {
                return "ST ELSE I";
            }
            if (filename == "RWTW_ST_ELSE_02")
            {
                return "ST ELSE II";
            }
            if (filename == "RWTW_ST_ELSE_03")
            {
                return "ST ELSE III";
            }
            if (filename == "RWTW_ST_ELSE_04")
            {
                return "ST ELSE IV";
            }
            if (filename == "RWTW_ST_ELSE_05")
            {
                return "ST ELSE V";
            }
            return orig(filename);
        }

        private static void UnlockSongs(On.Menu.MusicTrackButton.orig_ctor orig, Menu.MusicTrackButton self, Menu.Menu menu, Menu.MenuObject owner, string displayText, string singalText, Vector2 pos, Vector2 size, Menu.SelectOneButton[] buttonArray, int index)
        {
            if (WConfig.cfgDebugInfo.Value)
            {
                ExpLog.Log(index + " | " + singalText + " | " + tempDict.ElementAt(index));
            }
           
            singalText = tempDict.Keys.ElementAt(index);
            orig(self, menu, owner, displayText, singalText, pos, size, buttonArray, index);
            if (singalText == FirstTrack)
            {
                if (WConfig.cfgWatcherMusic.Value)
                { 
                    (menu as ExpeditionJukebox).demoMode = true; 
                } 
            }
            else if (singalText == LastTrack)
            {
                if (WConfig.cfgWatcherMusic.Value)
                {
                    (menu as ExpeditionJukebox).demoMode = false;
                }
            }
        }

        private static Dictionary<string, string> WatcherSongs(On.Expedition.ExpeditionProgression.orig_GetUnlockedSongs orig)
        {
            var original = orig();
            if (ModManager.Watcher && WConfig.cfgWatcherMusic.Value)
            {
                List<string> list = new List<string>
                {
                      "RW_99 - Air Toxing",
                      "RW_100 - Lonesound 2",
                      "RW_101 - Bell of Gesture",
                      "RW_102 - Bio Oscillator",
                      "RW_103 - Cave Drop",
                     
                      "RW_106 - Evening at Peace",
                      "RW_107 - Evil Arp",
                      "RW_108 - Explorer",
                      "RW_109 - Freq Shift",
                      "RW_110 - Insect Roamer",
                      "RW_111 - Laser Cutter",
                      "RW_112 - Light Beams",
                      "RW_113 - Liquid Lead",
                      "RW_114 - Machine Riser",
                      
                      
                      "RW_117 - Porto",
                      "RW_118 - Ripple Visions",
                      "RW_119 - Sci Prad",
                      "RW_120 - Shimmery",
                      "RW_121 - Soft Gesture",

                      
                      "RW_125 - Timelapse",
                      "RW_126 - Distant Breeze",
                      "RW_127 - Whale Song",
                      "RW_128 - Research",
                      "RW_129 - Seaside",
                      "RW_130 - Steel Pines",
                      "RW_131 - Versat",
                      "RW_132 - Pitter Patter",
                      "RW_133 - Ponder",
                      "RW_134 - Puddles",
                      "RW_135 - Fe56",
                      "RW_136 - Lullaby",
                      "RW_137 - Luminous",
                      "RW_138 - Teeth",
                      "RW_139 - Haldec",
                      "RW_140 - Merciful Eclipse",
                      "RW_141 - Advent Eclipse",
                      "RW_142 - Passerby",
                      "RW_143 - Bio Bop",
                      "RW_144 - Basilisk Beat",
                      "RW_145 - Moon Dog",
                      "RW_146 - Glass Fields",
                      "RW_147 - Afterhours",
                      "RW_148 - Ambient Arp",          
                      "RW_150 - Computer Tape",
                      "RW_152 - Cup",
                      "RW_154 - Groovy",
                      "RW_155 - Light of Hope",
                      "RW_156 - Nightshade Follower",                    
                      "RW_158 - Rhythmic Pad",
                      "RW_159 - Roadkill",
                      "RW_160 - Rubber Bass",                                    
                      "RW_163 - Skywhale Ride",
                      "RW_165 - Drill Crab",
                      "RW_166 - Boxworm Firefly",
                      "RW_168 - Big Moths",
                      "RW_169 - Grubs",
                      "BM_RWTW_BLUEENVOI",
                };
                List<string> listEcho = new List<string>
                {
                      "RW_161 - Childhoods End", //Intro theme             
                      "RWTW_ST_ELSE_01",
                      "RWTW_ST_ELSE_02",
                      "RWTW_ST_ELSE_03",
                      "RWTW_ST_ELSE_04",
                      "RWTW_ST_ELSE_05",
                      "RW_170 - In Dreams",
                      "RW_149 - Come and Go",
                      "RW_172 - Bathing",
                      "RW_173 - Spinning Top Reprise",
                      "RW_124 - Theme of Youth",
                };
                List<string> listRot = new List<string>
                {
                    "RW_104 - Chaos",
                    "RW_105 - Delwijar",
                    "RW_115 - Membrane",
                    "RW_116 - Passive Flow",
                    "RW_153 - Granular Mist",
                    "RW_164 - Rot Lizard",
                    "RW_167 - Bone Shakers",   
                    "RW_157 - Reactor Core",
                    "RW_122 - Speaking Systems 2",
                    "RW_123 - Speaking Systems 3",
                    "RW_171 - Efflorescence", //Rot End
                    "RW_151 - Condemned",
                    "RW_162 - Weaver Energy",
                };
                FirstTrack = "mus-w0";
                for (int i = 0; i < list.Count; i++)
                {
                    original["mus-w" + ValueConverter.ConvertToString<int>(i)] = list[i];
                }
                for (int i = 0; i < listEcho.Count; i++)
                {
                    original["mus-we" + ValueConverter.ConvertToString<int>(i)] = listEcho[i];
                }
                for (int i = 0; i < listRot.Count; i++)
                {
                    original["mus-wr" + ValueConverter.ConvertToString<int>(i)] = listRot[i];
                }
                LastTrack = "mus-wr" + listRot.Count;




                /*
                int length = original.Count+1;
                FirstTrack = "mus-w" + ValueConverter.ConvertToString<int>(original.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    original["mus-w" + ValueConverter.ConvertToString<int>(i +length)] = list[i];
                }
                LastTrack = "mus-w" + ValueConverter.ConvertToString<int>(original.Count);
                */
                if (!dump)
                {
                    if (WConfig.cfgDebugInfo.Value)
                    {
                        ExpLog.Log(FirstTrack);
                        ExpLog.Log(LastTrack);
                        dump = true;

                        for (int i = 0; i < original.Count; i++)
                        {
                            ExpLog.Log(original.ElementAt(i).ToString());
                        };
                        /*string enw = "";
                        for (int i = 0; i < original.Count+1; i++)
                        {
                            original.TryGetValue("mus-" + i, out enw);
                            ExpLog.Log("mus-" + i + " : " + enw);
                        }*/
                    }                
                }
            }
            return original;
        }
    }
}