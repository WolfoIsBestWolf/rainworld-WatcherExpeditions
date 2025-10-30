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


        /*public static Configurable<bool> cfgCustomColorFix = instance.config.Bind("cfgCustomColorFix", true,
         new ConfigurableInfo("WEConfig_Desc_ColorFix", null, "", new object[]
         {
              "WEConfig_Name_ColorFix"
         }));*/
        public static Configurable<bool> cfgSandbox = instance.config.Bind("cfgSandbox", true,
                 new ConfigurableInfo("WEConfig_Desc_Sandbox", null, "", new object[]
                 {
              "WEConfig_Name_Sandbox"
                 }));

        public static Configurable<bool> cfgSpinningTopDialogue = instance.config.Bind("cfgSpinningTopDialogue", true,
          new ConfigurableInfo("WEConfig_Desc_STSpeak", null, "", new object[]
          {
              "WEConfig_Name_STSpeak"
          }));
        public static Configurable<bool> cfgDebugInfo = instance.config.Bind("cfgDebugInfo", false,
           new ConfigurableInfo("Dump a bunch of info", null, "", new object[]
           {
                "Info Dump"
           }));
        public static Configurable<bool> cfgVanillaPassage = instance.config.Bind("cfgVanillaPassage", false,
           new ConfigurableInfo("WEConfig_Desc_VanillaPassage", null, "", new object[]
           {
                "WEConfig_Name_VanillaPassage"
           }));

        /*public static Configurable<bool> cfgButtonCombo = instance.config.Bind("cfgButtonCombo", true,
                   new ConfigurableInfo("For Ripple 9, if you press Special + Down, you will Camo without entering Ripplespace. This makes Ripple 9 not a trade off.", null, "", new object[]
                   {
                "Ripplespace button Combo"
                   }));*/



        public static Configurable<bool> cfgRotChallenge = instance.config.Bind("cfgRotChallenge", true,
           new ConfigurableInfo("WEConfig_Desc_Challenge_Rot", null, "", new object[]
           {
                "WEConfig_Name_Challenge_Rot"
           }));

        public static Configurable<bool> cfgWatcherMusic = instance.config.Bind("cfgWatcherMusic", true,
            new ConfigurableInfo("WEConfig_Desc_Music", null, "", new object[]
            {
                "WEConfig_Name_Music"
            }));
 
        public static Configurable<int> cfgWatcher_StartingRipple = instance.config.Bind("cfgWatcher_StartingRipple", 6,
            new ConfigurableInfo("WEConfig_Desc_RippleStart", new ConfigAcceptableRange<int>(1, 9), "", new object[]
            {
                "WEConfig_Name_RippleStart"
            }));
        public static Configurable<int> cfgWatcher_StartingRippleMax = instance.config.Bind("cfgWatcher_StartingRippleMax", 9,
            new ConfigurableInfo("WEConfig_Desc_RippleMax", new ConfigAcceptableRange<int>(1, 9), "", new object[]
            {
                "WEConfig_Name_RippleMax"
            }));
        public static Configurable<int> cfgWatcher_StartingRippleMin = instance.config.Bind("cfgWatcher_StartingRippleMin", 5,
            new ConfigurableInfo("WEConfig_Desc_RippleMin", new ConfigAcceptableRange<int>(1, 9), "", new object[]
            {
                "WEConfig_Name_RippleMin"
            }));
       /* public static Configurable<int> cfgRippleMaxDifference = instance.config.Bind("cfgRippleMaxDifference", 6,
            new ConfigurableInfo("Your max and minimum Ripple will be at most this far apart. ", new ConfigAcceptableRange<int>(1, 9), "", new object[]
            {
                "Max Ripple difference"
            }));*/
        public static Configurable<bool> cfgWatcher_Unlock = instance.config.Bind("cfgWatcher_Unlock", false,
            new ConfigurableInfo("WEConfig_Desc_Unlock", null, "", new object[]
            {
                "WEConfig_Name_Unlock"
            }));
        public static Configurable<bool> cfgWatcher_KarmaFlower = instance.config.Bind("cfgWatcher_KarmaFlower", true,
           new ConfigurableInfo("WEConfig_Desc_KarmaFlower", null, "", new object[]
        {
                "WEConfig_Name_KarmaFlower"
        }));
        /*public static Configurable<bool> cfgWatcher_WARA = instance.config.Bind("cfgWatcher_WARA", false,
                  new ConfigurableInfo("Expeditions can start in Shattered Terrance and Hunt challenges will count creatures in it.", null, "", new object[]
               {
                "Shattered Terrance"
               }));*/

        public static Configurable<bool> cfgWatcher_RotEnemies = instance.config.Bind("cfgWatcher_RotEnemies", false,
            new ConfigurableInfo("WEConfig_Desc_RotWorld_Hunt", null, "", new object[]
            {
                "WEConfig_Name_RotWorld_Hunt"
            }));
        public static Configurable<bool> cfgWatcher_RotShelter = instance.config.Bind("cfgWatcher_RotShelter", false,
        new ConfigurableInfo("WEConfig_Desc_RotWorld_Shelter", null, "", new object[]
        {
                "WEConfig_Name_RotWorld_Shelter"
        }));
        public static Configurable<bool> cfgWatcher_RippleChange = instance.config.Bind("cfgWatcher_RippleChange", false,
            new ConfigurableInfo("WEConfig_Desc_ChangeRipple", null, "", new object[]
            {
                "WEConfig_Name_ChangeRipple"
            }));

        //This shit does Not work.
        /*public static Configurable<bool> cfgWatcher_WarpMap = instance.config.Bind("cfgWatcher_WarpMap", true,
            new ConfigurableInfo("Carry over discovered Warps that always have the same destination.", null, "", new object[]
            {
                "Carry over Warp Map progress"
            }));*/
 
        public override void Initialize()
		{
			base.Initialize();
			this.Tabs = new OpTab[]
			{
                new OpTab(this, Translate("Watcher")),
                new OpTab(this, "~~~")
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

            var array = new ConfigurableBase[3][];
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
            };
            array[2] = new ConfigurableBase[]
            {
                cfgWatcher_Unlock,
                cfgWatcherMusic,
                cfgSandbox,
                //cfgCustomColorFix,
                cfgVanillaPassage,
                cfgSpinningTopDialogue,
            };
            var names = new string[]
             {
                Translate("Ripple"),
                Translate("Rot"),
                Translate("General"),
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



            OpLabel TitleLabelW = new OpLabel(new Vector2(150f, 520), new Vector2(300f, 30f), Translate("WatcherExpeditions_Name"), FLabelAlignment.Center, true, null);
            TitleLabelW.description = Translate("WatcherExpeditions_Desc");
            TitleLabelW.color = Watcher;
 
            this.Tabs[0].AddItems(new UIelement[]
              {
                    TitleLabelW,
              });
		}


		 
	}
}
