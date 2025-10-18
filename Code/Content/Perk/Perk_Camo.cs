using Expedition;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RWCustom;
using System;
using System.Reflection;
using UnityEngine;
using Watcher;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.Mathematics;

namespace WatcherExpeditions
{
    public class Perk_Camo : Modding.Expedition.CustomPerk
    {
        public override string ID
        {
            get
            {
                return "unl-watcher-camo";
            }
        }
        public override bool UnlockedByDefault
        {
            get
            {
                return true;
            }
        }
        public override Color Color
        {
            get
            {
                return PlayerGraphics.DefaultSlugcatColor(WatcherEnums.SlugcatStatsName.Watcher)*3f;
            }
        }
        public override string SpriteName
        {
            get
            {
                //return "Symbol_CamoPerk";
                return "Kill_Slugcat";
            }
        }
        public override string ManualDescription
        {
            get
            {
                return Description;
            }
        }
        public override string Description
        {
            get
            {
                return T.Translate("Perk_Camo_Desc");
            }
        }
        public override string DisplayName
        {
            get
            {
                return T.Translate("Perk_Camo_Name");
            }
        }
        public override string Group
        {
            get
            {
                return "WatcherExpeditions";
            }
        }
        public override bool AvailableForSlugcat(SlugcatStats.Name name)
        {
            return name != WatcherEnums.SlugcatStatsName.Watcher;
        }
        public static bool CamoPerk
        {
            get
            {
                return Custom.rainWorld.ExpeditionMode && ExpeditionGame.activeUnlocks.Contains("unl-watcher-camo");
            }
        }
        public override void ApplyHooks()
        {
            base.ApplyHooks();
          
            ILHook hook = new ILHook(typeof(Player).GetProperty("RippleAbilityActivationButtonCondition", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), WatcherPerkInput);
            ILHook hook2 = new ILHook(typeof(Player).GetProperty("VisibilityBonus", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), WatcherPerkVisiblity);
 
            On.Player.ctor += SetRipple;
            IL.Player.WatcherUpdate += PretendIsWatcher;
            //Camo Hud not automatic
            //Invis not automatic
            IL.PlayerGraphics.InitiateSprites += AddInvisibilityOverlaySprite;
            IL.PlayerGraphics.DrawSprites += InvisiblityForNonWatchers;
            IL.HUD.HUD.InitSinglePlayerHud += AddCamoMeter;
        }

 

        private void AddCamoMeter(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher"),
                x => x.MatchCall("ExtEnum`1<SlugcatStats/Name>", "op_Equality")))
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    if (CamoPerk)
                    {
                        return true;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log("WE CamoPerk Fail : HUD_InitSinglePlayerHud");
            }
        }

        private void AddInvisibilityOverlaySprite(ILContext il)
        {
            
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher"));

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchNewarr("FSprite")))
            {
                c.EmitDelegate<Func<int, int>>((self) =>
                {
                    if (CamoPerk)
                    {
                        //+1 Sprite Array Size
                        return self + 1;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log(c);
                Debug.Log("WE CamoPerk Fail : IL.PlayerGraphics.InitiateSprites1");
                return;
            }

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher"),
                x => x.MatchCall("ExtEnum`1<SlugcatStats/Name>", "op_Equality")))
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    if (CamoPerk)
                    {
                        //Watcher sprite segment
                        return true;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log(c);
                Debug.LogWarning("WE CamoPerk Fail : IL.PlayerGraphics.InitiateSprites2");
                return;
            }

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdcI4(12)))
            {
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<int, RoomCamera.SpriteLeaser, int>>((self, sprites) =>
                {
                    if (CamoPerk)
                    {
                        //Watcher Sprites being made (Camo stuff)
                        Debug.Log("Sprites"+sprites.sprites.Length);
                        return sprites.sprites.Length-1;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log(c);
                Debug.LogWarning("WE CamoPerk Fail : IL.PlayerGraphics.InitiateSprites3");
                return;
            }


        }

        private void InvisiblityForNonWatchers(ILContext il)
        {
            ILCursor c = new(il);

            bool bool1 = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher")); //Void sea??
            bool bool2 = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher")); //Arena?
            bool bool3 = c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher"), //Camo progress
                x => x.MatchCall("ExtEnum`1<SlugcatStats/Name>", "op_Equality"));

            if (bool1&&bool2&& bool3)
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    if (CamoPerk)
                    {
                        return true;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log("WE CamoPerk Fail IL.PlayerGraphics.DrawSprites1"+ bool1 +bool2 +bool3);
                return;
            }
            bool bool4 = c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher")); //The black thing
            bool bool5 = c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher"), //Camoprogress 2
                x => x.MatchCall("ExtEnum`1<SlugcatStats/Name>", "op_Equality"));
            if (bool4 && bool5)
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    if (CamoPerk)
                    {
                        return true;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log("WE CamoPerk Fail IL.PlayerGraphics.DrawSprites2" + bool4+ bool5);
                return;
            }

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher"), //Camo 3 sprite stuff idk
                x => x.MatchCall("ExtEnum`1<SlugcatStats/Name>", "op_Equality")))
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    if (CamoPerk)
                    {
                        return true;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log("WE CamoPerk Fail IL.PlayerGraphics.DrawSprites3");
                return;
            }
            for (int i = 0; i < 6; i++)
            {
                if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdcI4(12)))
                {
                    c.Emit(OpCodes.Ldarg_1);
                    c.EmitDelegate<Func<int, RoomCamera.SpriteLeaser, int>>((self, sprites) =>
                    {
                        if (CamoPerk)
                        {
                            return sprites.sprites.Length - 1;
                        }
                        return self;
                    });
                }
                else
                {
                    Debug.Log("WE CamoPerk Fail IL.PlayerGraphics.DrawSprites4" + i);
                    return;
                }
            }
            


            /*if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher"), //Smth smth Mud idk
                x => x.MatchCall("ExtEnum`1<SlugcatStats/Name>", "op_Equality")))
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    if (CamoPerk)
                    {
                        return true;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log("WatcherExpeditions: fail CamoPerk_IL51");
            }*/
        }

        private void SetRipple(On.Player.orig_ctor orig, Player self, AbstractCreature abstractCreature, World world)
        {
            orig(self, abstractCreature, world);
            if (CamoPerk)
            {
                (self.abstractCreature.world.game.session as StoryGameSession).saveState.deathPersistentSaveData.maximumRippleLevel = 0.5f;
                (self.abstractCreature.world.game.session as StoryGameSession).saveState.deathPersistentSaveData.rippleLevel = 0.5f;
            }
        }

       
        public void WatcherPerkInput(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher"),
                x => x.MatchCall("ExtEnum`1<SlugcatStats/Name>", "op_Equality")))
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    return self || CamoPerk;
                });
            }
            else
            {
                Debug.Log("WatcherExpeditions: fail CamoPerk_IL4");
            }
        }
        public void WatcherPerkVisiblity(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher"),
                x => x.MatchCall("ExtEnum`1<SlugcatStats/Name>", "op_Equality")))
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    return self || CamoPerk;
                });
            }
            else
            {
                Debug.Log("WatcherExpeditions: fail CamoPerk_IL5");
            }
        }

        private void PretendIsWatcher(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("Watcher.WatcherEnums/SlugcatStatsName", "Watcher"),
                x => x.MatchCall("ExtEnum`1<SlugcatStats/Name>", "op_Equality")))
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    //Run whole thing if Camo Perk active
                    return self || CamoPerk;
                });
            }
            else
            {
                Debug.Log(c);
                Debug.Log("WatcherExpeditions: fail CamoPerk_IL1"); return;
            }
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchCall("Player", "get_rippleLevel"),
                x => x.MatchLdcR4(0)))
            {
                c.EmitDelegate<Func<float, float>>((self) =>
                {
                    if (CamoPerk)
                    {
                        //Remove forced free glow.
                        return 0f;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log(c);
                Debug.Log("WatcherExpeditions: fail CamoPerk_IL2"); return;
            }
           /* if (c.TryGotoNext(MoveType.After,
               x => x.MatchCall("Player", "get_rippleLevel"),
               x => x.MatchLdcR4(0.5f)))
            {
                c.EmitDelegate<Func<float, float>>((self) =>
                {
                    if (CamoPerk)
                    {
                        return 1f;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log("WatcherExpeditions: fail CamoPerk_IL3");
            }*/
        }

        
    }
}