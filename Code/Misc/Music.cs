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

namespace WatcherExpeditions
{
    public class Music
    {
        public static string WatcherStart;
        public static string LastTrack;


        public static void Add()
        {
            On.Expedition.ExpeditionProgression.GetUnlockedSongs += WatcherSongs;
            On.Expedition.ExpeditionProgression.TrackName += WatcherTrackName;

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
            Debug.Log(singalText);
            orig(self, menu, owner, displayText, singalText, pos, size, buttonArray, index);
            if (singalText == WatcherStart)
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
                      //"NA_43 - Isolation", //Not Watcher, Not a song
                      "RW_99 - Air Toxing",
                      "RW_100 - Lonesound 2",
                      "RW_101 - Bell of Gesture",
                      "RW_102 - Bio Oscillator",
                      "RW_103 - Cave Drop",
                      "RW_104 - Chaos",
                      "RW_105 - Delwijar",
                      "RW_106 - Evening at Peace",
                      "RW_107 - Evil Arp",
                      "RW_108 - Explorer",
                      "RW_109 - Freq Shift",
                      "RW_110 - Insect Roamer",
                      "RW_111 - Laser Cutter",
                      "RW_112 - Light Beams",
                      "RW_113 - Liquid Lead",
                      "RW_114 - Machine Riser",
                      "RW_115 - Membrane",
                      "RW_116 - Passive Flow",
                      "RW_117 - Porto",
                      "RW_118 - Ripple Visions",
                      "RW_119 - Sci Prad",
                      "RW_120 - Shimmery",
                      "RW_121 - Soft Gesture",
                      "RW_122 - Speaking Systems 2",
                      "RW_123 - Speaking Systems 3",
                      "RW_124 - Theme of Youth",
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
                      "RW_149 - Come and Go",
                      "RW_150 - Computer Tape",
                      "RW_151 - Condemned",
                      "RW_152 - Cup",
                      "RW_153 - Granular Mist",
                      "RW_154 - Groovy",
                      "RW_155 - Light of Hope",
                      "RW_156 - Nightshade Follower",
                      "RW_157 - Reactor Core",
                      "RW_158 - Rhythmic Pad",
                      "RW_159 - Roadkill",
                      "RW_160 - Rubber Bass",
                      "RW_161 - Childhoods End",
                      "RW_162 - Weaver Energy",
                      "RW_163 - Skywhale Ride",
                      "RW_164 - Rot Lizard",
                      "RW_165 - Drill Crab",
                      "RW_166 - Boxworm Firefly",
                      "RW_167 - Bone Shakers",
                      "RW_168 - Big Moths",
                      "RW_169 - Grubs",
                      "RW_170 - In Dreams",
                      "RW_171 - Efflorescence",
                      "RW_172 - Bathing",
                      "RW_173 - Spinning Top Reprise",
                      "BM_RWTW_BLUEENVOI",
                      "RWTW_ST_ELSE_01",
                      "RWTW_ST_ELSE_02",
                      "RWTW_ST_ELSE_03",
                      "RWTW_ST_ELSE_04",
                      "RWTW_ST_ELSE_05",
                };
              
                int length = original.Count+1;
                WatcherStart = "mus-" + ValueConverter.ConvertToString<int>(length);
                for (int i = 0; i < list.Count; i++)
                {
                    original["mus-" + ValueConverter.ConvertToString<int>(i + length)] = list[i];
                }
            }
            LastTrack = "mus-" + ValueConverter.ConvertToString<int>(original.Count);

            return original;
        }
    }
}