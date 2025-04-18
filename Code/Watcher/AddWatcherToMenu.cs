using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using System.Linq;
using RWCustom;
using Menu;
using System;
using Expedition;
using MonoMod.Cil;
using UnityEngine;
using JollyCoop;
using System.IO;

namespace WatcherExpeditions
{
    public class AddWatcherToMenu
    {
        public static void Start()
        {
            On.Menu.CharacterSelectPage.GetSlugcatPortrait += FixWatcherPortrait;
            On.Menu.CharacterSelectPage.UpdateSelectedSlugcat += FixWatcherAndSlugbaseWeirdName;
            On.Menu.MenuScene.BuildVoidBathScene += MenuScene_BuildVoidBathScene;

            On.Expedition.ExpeditionData.GetPlayableCharacters += ExpeditionData_GetPlayableCharacters;
            On.Expedition.ExpeditionProgression.CheckUnlocked += ShouldWatcherBeUnlocked;

            IL.Menu.CharacterSelectPage.ctor += MoveSlugsAndJukebox;

            On.JollyCoop.JollyMenu.JollyPlayerSelector.JollyPortraitName += FixWatcherPortrait_Jolly;
            On.JollyCoop.JollyCustom.SlugClassMenu += AllowWatcherJollyExpedition;
        }

        private static void MenuScene_BuildVoidBathScene(On.Menu.MenuScene.orig_BuildVoidBathScene orig, MenuScene self, int index)
        {       
            if (Custom.rainWorld.ExpeditionMode)
            {
                if (index != 2)
                {
                    return;
                }          
                self.sceneFolder = "Scenes" + Path.DirectorySeparatorChar.ToString() + "outro void bath " + index.ToString();
                string str = "outro void bath " + index.ToString();
                if (self.flatMode)
                {
                    self.useFlatCrossfades = true;
                    self.AddIllustration(new MenuIllustration(self.menu, self, self.sceneFolder, str + " - flat - b", new Vector2(683f, 384f), false, true));
                }
                else
                {
                    self.AddIllustration(new MenuDepthIllustration(self.menu, self, self.sceneFolder, str + " background - 9", new Vector2(0f, 0f), 8f, MenuDepthIllustration.MenuShader.Normal));
                    self.AddIllustration(new MenuDepthIllustration(self.menu, self, self.sceneFolder, str + " pillars distort - 8", new Vector2(0f, 0f), 6.2f, MenuDepthIllustration.MenuShader.Normal));
                    self.AddIllustration(new MenuDepthIllustration(self.menu, self, self.sceneFolder, str + " candle row - 7", new Vector2(0f, 0f), 3.4f, MenuDepthIllustration.MenuShader.Normal));
                    self.AddIllustration(new MenuDepthIllustration(self.menu, self, self.sceneFolder, str + " big candles - 6", new Vector2(0f, 0f), 3.2f, MenuDepthIllustration.MenuShader.Normal));
                    self.AddIllustration(new MenuDepthIllustration(self.menu, self, self.sceneFolder, str + " echo remains - 2b", new Vector2(0f, 0f), 3.2f, MenuDepthIllustration.MenuShader.Basic));
                    self.AddIllustration(new MenuDepthIllustration(self.menu, self, self.sceneFolder, str + " slugcat watching - 1", new Vector2(0f, 0f), 1.8f, MenuDepthIllustration.MenuShader.Normal));
                    return;
                }
                return;
            }
            orig(self, index);
        }

        private static SlugcatStats.Name AllowWatcherJollyExpedition(On.JollyCoop.JollyCustom.orig_SlugClassMenu orig, int playerNumber, SlugcatStats.Name fallBack)
        {
            if (Custom.rainWorld.ExpeditionMode)
            {
                SlugcatStats.Name name = Custom.rainWorld.options.jollyPlayerOptionsArray[playerNumber].playerClass;
                if (ModManager.Watcher && name == WatcherEnums.SlugcatStatsName.Watcher)
                {
                    return WatcherEnums.SlugcatStatsName.Watcher;
                }
            }

            return orig(playerNumber, fallBack);
        }

        private static string FixWatcherPortrait_Jolly(On.JollyCoop.JollyMenu.JollyPlayerSelector.orig_JollyPortraitName orig, JollyCoop.JollyMenu.JollyPlayerSelector self, SlugcatStats.Name className, int colorIndexFile)
        {
            if (className == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return "multiplayerportrait31";
            }
            return orig(self, className, colorIndexFile);
        }

     

        private static int JollySetupDialog_GetFileIndex(On.JollyCoop.JollyMenu.JollySetupDialog.orig_GetFileIndex orig, JollyCoop.JollyMenu.JollySetupDialog self, SlugcatStats.Name name)
        {
            if (name == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return 3;
            }
            return orig(self, name);
        }

        private static bool ShouldWatcherBeUnlocked(On.Expedition.ExpeditionProgression.orig_CheckUnlocked orig, ProcessManager manager, SlugcatStats.Name slugcat)
        {
            if (slugcat == WatcherEnums.SlugcatStatsName.Watcher)
            {
                if (Watcher.Watcher.chtUnlockCampaign.Value || WConfig.cfgWatcher_Unlock.Value)
                {
                    return true;
                }
            }
            return orig(manager, slugcat);
        }

        private static void MoveSlugsAndJukebox(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdcI4(3)))
            {
                c.EmitDelegate<Func<int, int>>((exped) =>
                {
                    if (!WatcherExpeditions.slugbase && exped == 8)
                    {
                        return 0;
                    }
                    return exped;
                });
            }

            if (c.TryGotoNext(MoveType.After,
             x => x.MatchLdcR4(525f)))
            {
                c.EmitDelegate<Func<float, float>>((exped) =>
                {
                    if (!WatcherExpeditions.slugbase)
                    {
                        return exped - 55f;
                    }
                    return exped;
                    return exped - 110f;
                });
            }
            if (c.TryGotoNext(MoveType.After,
             x => x.MatchLdcR4(110f)))
            {
                c.Index++;
                c.EmitDelegate<Func<int, int>>((exped) =>
                {
                    if (!WatcherExpeditions.slugbase && exped == 8)
                    {
                        return 3;
                    }
                    return exped;
                });
            }
            if (c.TryGotoNext(MoveType.After,
             x => x.MatchLdcR4(110f)))
            {
                c.Index++;
                c.EmitDelegate<Func<int, int>>((exped) =>
                {
                    if (!WatcherExpeditions.slugbase && exped == 8)
                    {
                        return 3;
                    }
                    return exped;
                });
            }

            if (c.TryGotoNext(MoveType.After,
              x => x.MatchLdcR4(900f)))
            {
                c.EmitDelegate<Func<float, float>>((exped) =>
                {
                    if (!WatcherExpeditions.slugbase)
                    {
                        return exped + 55f;
                    }
                    return exped;
                });
            }
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdcR4(875f)))
            {
                c.EmitDelegate<Func<float, float>>((exped) =>
                {
                    if (!WatcherExpeditions.slugbase)
                    {
                        return exped + 55f;
                    }
                    return exped;
                });
            }
            if (c.TryGotoNext(MoveType.After,
           x => x.MatchLdcR4(440f)))
            {
                c.EmitDelegate<Func<float, float>>((exped) =>
                {
                    if (!WatcherExpeditions.slugbase)
                    {
                        return exped - 55f;
                    }
                    return exped;
                });
            }
            return;
            /*if (c.TryGotoNext(MoveType.After,
             x => x.MatchLdcI4(7)))
            {
                c.EmitDelegate<Func<int, int>>((exped) =>
                {
                    if (WConfig.cfgWatcher_Expedition.Value)
                    {
                        return exped+1;
                    }
                    return exped;
                });
            }

            if (c.TryGotoNext(MoveType.After,
             x => x.MatchLdcR4(415)))
            {
                c.EmitDelegate<Func<float, float>>((exped) =>
                {
                    if (WConfig.cfgWatcher_Expedition.Value)
                    {
                        return exped - 55f;
                    }
                    return exped;
                });
            }*/
        }

        private static List<SlugcatStats.Name> ExpeditionData_GetPlayableCharacters(On.Expedition.ExpeditionData.orig_GetPlayableCharacters orig)
        {
            var temp = orig();
            if (ModManager.Watcher)
            {
                temp.Add(WatcherEnums.SlugcatStatsName.Watcher);
            }
            return temp;
        }

        private static void FixWatcherAndSlugbaseWeirdName(On.Menu.CharacterSelectPage.orig_UpdateSelectedSlugcat orig, CharacterSelectPage self, int num)
        {
            orig(self, num);
            if (ModManager.Watcher && ExpeditionGame.playableCharacters[num] == WatcherEnums.SlugcatStatsName.Watcher)
            {
                //self.slugcatScene = WatcherEnums.MenuSceneID.MainMenu_Watcher;
                //self.slugcatScene = WatcherEnums.MenuSceneID.Ending_SpinningTop2;
                self.slugcatScene = WatcherEnums.MenuSceneID.Ending_VoidBath2;
                self.slugcatName.text = self.menu.Translate("THE WATCHER");
            }
        }

        private static Menu.MenuIllustration FixWatcherPortrait(On.Menu.CharacterSelectPage.orig_GetSlugcatPortrait orig, Menu.CharacterSelectPage self, SlugcatStats.Name slugcat, UnityEngine.Vector2 pos)
        {
            if (ModManager.Watcher && slugcat == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return new MenuIllustration(self.menu, self, "illustrations", "multiplayerportrait31", pos, true, true);
            }
            return orig(self, slugcat, pos);
        }

    }
}