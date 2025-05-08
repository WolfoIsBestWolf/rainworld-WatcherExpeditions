using JetBrains.Annotations;
using Menu.Remix;
using Menu.Remix.MixedUI;
using MoreSlugcats;
using RWCustom;
using System;
using UnityEngine;

namespace WatcherExpeditions
{
    public class WConfig : OptionInterface
    {

		public static WConfig instance = new WConfig();


        public static Configurable<bool> cfgCustomColorFix = instance.config.Bind("cfgCustomColorFix", true,
         new ConfigurableInfo("Watcher has his body color overritten with the areas shade of black, ignoring Custom Colors, Jolly Custom Colors and his default set dark blue. This makes it so Remix Custom and Jolly Custom colors work.", null, "", new object[]
         {
              "Fix Custom Color"
         }));
        public static Configurable<bool> cfgSandbox = instance.config.Bind("cfgSandbox", true,
                 new ConfigurableInfo("Add Watcher creatures & items to Sandbox & Arena. They will add this officially eventually. No unlock requirement. Roz Lizards aren't really possible since they aren't a unique creature.", null, "", new object[]
                 {
              "Watcher Sandbox"
                 }));

        public static Configurable<bool> cfgSpinningTopDialogue = instance.config.Bind("cfgSpinningTopDialogue", true,
          new ConfigurableInfo("Allow Spinning Top to speak. It will choose dialogues in order.", null, "", new object[]
          {
                        "Spinning Top speaking"
          }));
        public static Configurable<bool> cfgDebugInfo = instance.config.Bind("cfgDebugInfo", false,
           new ConfigurableInfo("Dump a bunch of info", null, "", new object[]
           {
                "Info Dump"
           }));
        public static Configurable<bool> cfgVanillaPassage = instance.config.Bind("cfgVanillaPassage", false,
           new ConfigurableInfo("Enable functional but black screen passages in the Watcher Campaign.  Blackscreens for areas as it's not compatible with the Warp Sphere and there is no art for the new areas.", null, "", new object[]
           {
                "Passages for Watcher Campaign"
           }));

        public static Configurable<bool> cfgButtonCombo = instance.config.Bind("cfgButtonCombo", true,
                   new ConfigurableInfo("For Ripple 9, if you press Special + Down, you will Camo without entering Ripplespace. This makes Ripple 9 not a trade off.", null, "", new object[]
                   {
                "Ripplespace button Combo"
                   }));



        public static Configurable<bool> cfgRotChallenge = instance.config.Bind("cfgRotChallenge", true,
           new ConfigurableInfo("Challenge where you need to infect regions to beat it.  5 with normal difficulty, range of 3-9.", null, "", new object[]
           {
                "Rot Spreading Challenge"
           }));

        public static Configurable<bool> cfgWatcherMusic = instance.config.Bind("cfgWatcherMusic", true,
            new ConfigurableInfo("Add Watcher soundtrack to the Jukebox. No unlock required.", null, "", new object[]
            {
                "Watcher Jukebox"
            }));
        /*public static Configurable<bool> cfgMoveJukebox = instance.config.Bind("cfgMoveJukebox", true,
           new ConfigurableInfo("Move the Jukebox button to better fit the Watcher next to the other slugcats.", null, "", new object[]
           {
                "Move Jukebox button"
           }));*/
        public static Configurable<int> cfgWatcher_StartingRipple = instance.config.Bind("cfgWatcher_StartingRipple", 6,
            new ConfigurableInfo("Starting amount of Ripple. The karma replacement.", new ConfigAcceptableRange<int>(1, 9), "", new object[]
            {
                "Starting Ripple"
            }));
        public static Configurable<int> cfgWatcher_StartingRippleMax = instance.config.Bind("cfgWatcher_StartingRippleMax", 9,
            new ConfigurableInfo("Starting amount of max Ripple. You need 3 max Ripple to open Portals. 5 Ripple to hover. 8 Ripple to teleport to all Regions. 9 Ripple to enter Daemon.", new ConfigAcceptableRange<int>(1, 9), "", new object[]
            {
                "Starting max Ripple"
            }));
        public static Configurable<int> cfgWatcher_StartingRippleMin = instance.config.Bind("cfgWatcher_StartingRippleMin", 5,
            new ConfigurableInfo("Starting amount of minimum Ripple. If you didn't know, your minimum increases as you visit Spinning Top more times.", new ConfigAcceptableRange<int>(1, 9), "", new object[]
            {
                "Starting min Ripple"
            }));
        public static Configurable<int> cfgRippleMaxDifference = instance.config.Bind("cfgRippleMaxDifference", 6,
            new ConfigurableInfo("Your max and minimum Ripple will be at most this far apart. ", new ConfigAcceptableRange<int>(1, 9), "", new object[]
            {
                "Max Ripple difference"
            }));
        public static Configurable<bool> cfgWatcher_Unlock = instance.config.Bind("cfgWatcher_Unlock", false,
            new ConfigurableInfo("Unlock the Watcher Expedition regardless of campaign completion.", null, "", new object[]
            {
                "Force unlock"
            }));
        public static Configurable<bool> cfgWatcher_KarmaFlower = instance.config.Bind("cfgWatcher_KarmaFlower", true,
           new ConfigurableInfo("Allow Karma Flowers to spawn in every region for Watcher Expeditions, as they are important for his Warp ability. If disabled they still spawn in Outer Rim, Shattered Terrance, Daemon and rot regions.", null, "", new object[]
        {
                "Karma Flowers all regions"
        }));
        public static Configurable<bool> cfgWatcher_WARA = instance.config.Bind("cfgWatcher_WARA", false,
                  new ConfigurableInfo("Expeditions can start in Shattered Terrance and Hunt challenges will count creatures in it.", null, "", new object[]
               {
                "Shattered Terrance"
               }));

        public static Configurable<bool> cfgWatcher_RotEnemies = instance.config.Bind("cfgWatcher_RotEnemies", false,
            new ConfigurableInfo("Hunts count enemies within Rot Regions, such as DLLs and Red Centipedes.  As you have no choice in what Rot Region you get teleported to, this isn't recommended.", null, "", new object[]
            {
                "Rot Dimension - Hunt"
            }));
        public static Configurable<bool> cfgWatcher_RotShelter = instance.config.Bind("cfgWatcher_RotShelter", false,
        new ConfigurableInfo("Expeditions will be able to start in Rot Regions. You will need to find an exit which then teleports you to a random region. Never Outer Rim", null, "", new object[]
        {
                "Rot Dimension - Shelters"
        }));
        public static Configurable<bool> cfgWatcher_RippleChange = instance.config.Bind("cfgWatcher_RippleChange", false,
            new ConfigurableInfo("Allow Spinning Top to increase maximum and minimum Ripple amount. Still sets Ripple to the highest it can be. Echoes do not in vanilla Expedition.", null, "", new object[]
            {
                "Echo changes max/min Ripple"
            }));

        //This shit does Not work.
        public static Configurable<bool> cfgWatcher_WarpMap = instance.config.Bind("cfgWatcher_WarpMap", true,
            new ConfigurableInfo("Carry over discovered Warps that always have the same destination.", null, "", new object[]
            {
                "Carry over Warp Map progress"
            }));
        public static Configurable<bool> cfgHunt_Barnacle = instance.config.Bind("cfgHunt_Barnacle", true,
            new ConfigurableInfo("Allow Hunting challenges to pick : Barnacle. As of 1.10.2 they can be desheled with Snails.", null, "", new object[]
            {
                "Hunt - Barnacles"
            }));
        public static Configurable<bool> cfgHunt_BigSandWorm = instance.config.Bind("cfgHunt_BigSandWorm", true,
            new ConfigurableInfo("Allow Hunting challenges to pick : Sand Worm. Similiar Monster Kelp, but stun you making escape not possible once grabbed, and can hide.", null, "", new object[]
            {
                "Hunt - Sand Worms"
            }));
        public static Configurable<bool> cfgHunt_BoxWorm = instance.config.Bind("cfgHunt_BoxWorm", false,
            new ConfigurableInfo("Allow Hunting challenges to pick : Box Worm. Box Worms can easily trap you, need to be activated before being killable, and aren't much fun to fight. Uninhabitated Box Worms are unkillable.", null, "", new object[]
            {
                "Hunt - Box Worm"
            }));
        public static Configurable<bool> cfgHunt_BigMoth = instance.config.Bind("cfgHunt_BigMoth", true,
            new ConfigurableInfo("Allow Hunting challenges to pick : Big Moth. With 1.10.2 these seem more fight-able and escapeable. ", null, "", new object[]
            {
                "Hunt - Big Moth"
            }));
        public static Configurable<bool> cfgHunt_Rattler = instance.config.Bind("cfgHunt_Rattler", true,
            new ConfigurableInfo("Allow Hunting challenges to pick : Bone Shaker. They only appear in rot regions and can be killed from the top, by knocking them over. ", null, "", new object[]
            {
                "Hunt - Bone Shaker"
            }));
        public static Configurable<bool> cfgStowaway = instance.config.Bind("cfgStowaway", false,
          new ConfigurableInfo("Add Stowaway to Sandbox, it can crash the game.", null, "", new object[]
          {
                "Stowaway"
          }));
        public override void Initialize()
		{
			base.Initialize();
			this.Tabs = new OpTab[]
			{
                new OpTab(this, Translate("Watcher")),
                new OpTab(this, Translate("~~~"))
            };
			this.AddCheckbox();
        }

        private void PopulateWithConfigs(int tabIndex, ConfigurableBase[][] lists, [CanBeNull] string[] names, [CanBeNull] Color[] colors, int splitAfter)
        {
            new OpLabel(new Vector2(100f, 560f), new Vector2(400f, 30f), this.Tabs[tabIndex].name, FLabelAlignment.Center, true, null);
            OpTab opTab = this.Tabs[tabIndex];
            float num = 40f;
            float num2 = tabIndex == 3 ? 160f : 20f;
            float num3 = tabIndex == 1 ? 540f : 500f;
            UIconfig uiconfig = null;
            for (int i = 0; i < lists.Length; i++)
            {              
                if (i == splitAfter)
                {
                    num2 += 300f;
                    num3 = tabIndex == 1 ? 540f : 500f;
                    num = 40f;
                    uiconfig = null;
                }
                else if (names != null && names[i] == "")
                {
                    num3 += 70f;
                }
                if (!(names == null || names[i] == ""))
                {
                    var label = new OpLabel(new Vector2(num2, num3 - num + 10f), new Vector2(260f, 30f), "~ " + names[i] + " ~", FLabelAlignment.Center, true, null);
                    if (colors != null)
                    {
                        label.color = colors[i];
                    }
                    opTab.AddItems(new UIelement[]
                    {
                        label
                    });
                }
                FTextParams ftextParams = new FTextParams();
                if (InGameTranslator.LanguageID.UsesLargeFont(Custom.rainWorld.inGameTranslator.currentLanguage))
                {
                    ftextParams.lineHeightOffset = -12f;
                }
                else
                {
                    ftextParams.lineHeightOffset = -5f;
                }
                for (int j = 0; j < lists[i].Length; j++)
                {
                    switch (ValueConverter.GetTypeCategory(lists[i][j].settingType))
                    {
                        case ValueConverter.TypeCategory.Boolean:
                            {
                                num += 30f;
                                Configurable<bool> configurable = lists[i][j] as Configurable<bool>;
                                OpCheckBox opCheckBox = new OpCheckBox(configurable, new Vector2(num2, num3 - num))
                                {
                                    description = OptionInterface.Translate(configurable.info.description),
                                    sign = i
                                };
                                UIfocusable.MutualVerticalFocusableBind(opCheckBox, uiconfig ?? opCheckBox);
                                OpLabel opLabel = new OpLabel(new Vector2(num2 + 40f, num3 - num), new Vector2(240f, 30f), Custom.ReplaceLineDelimeters(OptionInterface.Translate(configurable.info.Tags[0] as string)), FLabelAlignment.Left, false, ftextParams)
                                {
                                    bumpBehav = opCheckBox.bumpBehav,
                                    description = opCheckBox.description
                                };
								if (colors != null)
								{
									opCheckBox.colorEdge = colors[i];
									opLabel.color = colors[i];
                                }
                                opTab.AddItems(new UIelement[]
                                {
                            opCheckBox,
								opLabel
                                });
                                uiconfig = opCheckBox;
                                break;
                            }
                        case ValueConverter.TypeCategory.Integrals:
                            {
                                num += 36f;
                                Configurable<int> configurable2 = lists[i][j] as Configurable<int>;
                                OpUpdown opUpdown = new OpUpdown(configurable2, new Vector2(num2, num3 - num), 70f)
                                {
                                    description = OptionInterface.Translate(configurable2.info.description),
                                    sign = i
                                };
                                UIfocusable.MutualVerticalFocusableBind(opUpdown, uiconfig ?? opUpdown);
                                OpLabel opLabel2 = new OpLabel(new Vector2(num2 + 80f, num3 - num), new Vector2(120f, 36f), Custom.ReplaceLineDelimeters(OptionInterface.Translate(configurable2.info.Tags[0] as string)), FLabelAlignment.Left, false, ftextParams)
                                {
                                    bumpBehav = opUpdown.bumpBehav,
                                    description = opUpdown.description
                                };
                                if (colors != null)
                                {
                                    opUpdown.colorEdge = colors[i];
                                    opLabel2.color = colors[i];
                                }
                                opTab.AddItems(new UIelement[]
                                {
                            opUpdown,
                            opLabel2
                                });
                                uiconfig = opUpdown;
                                break;
                            }
                        case ValueConverter.TypeCategory.Floats:
                            {
                                Configurable<float> configurable3 = lists[i][j] as Configurable<float>;
                                byte decimalPoints = 1;
                             

                                num += 36f;
                                OpUpdown opUpdown2 = new OpUpdown(configurable3, new Vector2(num2, num3 - num), 70f, decimalPoints)
                                {
                                    description = OptionInterface.Translate(configurable3.info.description),
                                    sign = i
                                };
                                UIfocusable.MutualVerticalFocusableBind(opUpdown2, uiconfig ?? opUpdown2);
                                OpLabel opLabel3 = new OpLabel(new Vector2(num2 + 80f, num3 - num), new Vector2(120f, 36f), Custom.ReplaceLineDelimeters(OptionInterface.Translate(configurable3.info.Tags[0] as string)), FLabelAlignment.Left, false, ftextParams)
                                {
                                    bumpBehav = opUpdown2.bumpBehav,
                                    description = opUpdown2.description
                                };
                                if (colors != null)
                                {
                                    opUpdown2.colorEdge = colors[i];
                                    opLabel3.color = colors[i];
                                }
                                opTab.AddItems(new UIelement[]
                                {
                            opUpdown2,
                            opLabel3
                                });
                                uiconfig = opUpdown2;
                                break;
                            }
                    }
                }
                if (names != null)
                {
                    num3 -= 70f;
                }
               
            }
            for (int k = 0; k < lists.Length; k++)
            {
                if (k == 0 || k == 1)
                {
                    lists[k][0].BoundUIconfig.SetNextFocusable(UIfocusable.NextDirection.Up, lists[k][0].BoundUIconfig);
                }
                if (k == 0 || k == lists.Length - 1)
                {
                    lists[k][lists[k].Length - 1].BoundUIconfig.SetNextFocusable(UIfocusable.NextDirection.Down, FocusMenuPointer.GetPointer(FocusMenuPointer.MenuUI.SaveButton));
                }
            }
            int num4 = 0;
            for (int l = 1; l < lists.Length; l++)
            {
                for (int m = 0; m < lists[l].Length; m++)
                {
                    if (lists[l][m].BoundUIconfig != null)
                    {
                        lists[l][m].BoundUIconfig.SetNextFocusable(UIfocusable.NextDirection.Right, lists[l][m].BoundUIconfig);
                        if (num4 < lists[0].Length)
                        {
                            if (lists[0][num4].BoundUIconfig == null)
                            {
                                num4++;
                            }
                            else
                            {
                                UIfocusable.MutualHorizontalFocusableBind(lists[0][num4].BoundUIconfig, lists[l][m].BoundUIconfig);
                                lists[0][num4].BoundUIconfig.SetNextFocusable(UIfocusable.NextDirection.Left, FocusMenuPointer.GetPointer(FocusMenuPointer.MenuUI.CurrentTabButton));
                                num4++;
                            }
                        }
                        else
                        {
                            lists[l][m].BoundUIconfig.SetNextFocusable(UIfocusable.NextDirection.Left, lists[0][lists[0].Length - 1].BoundUIconfig);
                        }
                    }
                }
            }
        }

        private void AddCheckbox()
		{
            Color cheatColor = new Color(0.85f, 0.35f, 0.4f);
            Color Watcher = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Night) * 3.7f;
            Color WatcherDark = PlayerGraphics.DefaultSlugcatColor(SlugcatStats.Name.Night) * 2.6f;
            Color RotColor = new Color(0.666f, 0.333f, 0.9f);

            //Color(0.373f, 0.11f, 0.831f);

            WatcherDark.a = 1;

            Tabs[0].colorCanvas = WatcherDark;

            var array = new ConfigurableBase[4][];
            var colors = new Color[]
             {
                Watcher,
                RotColor,
                Menu.MenuColorEffect.rgbMediumGrey,
                Menu.MenuColorEffect.rgbMediumGrey,
             };
            array[0] = new ConfigurableBase[]
            {        
                cfgWatcher_StartingRipple,
                cfgWatcher_StartingRippleMax,
                cfgWatcher_StartingRippleMin,
                cfgWatcher_RippleChange,
                cfgWatcher_KarmaFlower,
            };
            array[1] = new ConfigurableBase[]
            {
                cfgRotChallenge,
                cfgWatcher_RotShelter,
                cfgWatcher_RotEnemies,
                cfgHunt_Rattler,
            };
            array[2] = new ConfigurableBase[]
            {
                cfgWatcher_Unlock,
                cfgWatcherMusic,
                cfgSandbox,
                cfgCustomColorFix,
                cfgVanillaPassage,
                cfgSpinningTopDialogue,
            };
            array[3] = new ConfigurableBase[]
{
                cfgHunt_Barnacle,
                cfgHunt_BigMoth,
                cfgHunt_BigSandWorm,
                cfgHunt_BoxWorm,
};
            var names = new string[]
             {
                Translate("Ripple"),
                Translate("Rot"),
                Translate("General"),
                Translate("Hunts"),
             };
            instance.PopulateWithConfigs(0, array, names, colors, 2);

            array = new ConfigurableBase[][]
            {
                new ConfigurableBase[]
                {
                    cfgDebugInfo
                }
            };
            instance.PopulateWithConfigs(1, array, null, null, 2);



            OpLabel TitleLabelW = new OpLabel(new Vector2(150f, 520), new Vector2(300f, 30f), Translate("~Watcher Expedition Mod~"), FLabelAlignment.Center, true, null);
            TitleLabelW.description = Translate("Unofficial Watcher Expeditions.");
            TitleLabelW.color = Watcher;
 
            this.Tabs[0].AddItems(new UIelement[]
              {
                    TitleLabelW,
              });
		}


		 
	}
}
