using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;
using System;
using HUD;
using RWCustom;
using UnityEngine;
using Mono.Cecil.Cil;

namespace WatcherExpeditions
{
    public class SandboxStuff
    {
        public static MultiplayerUnlocks.SandboxUnlockID FireSpriteLarva;    
        public static MultiplayerUnlocks.SandboxUnlockID Boomerang;
        //public static MultiplayerUnlocks.SandboxUnlockID SoftToy;

        public static MultiplayerUnlocks.SandboxUnlockID DrillCrab;
		public static MultiplayerUnlocks.SandboxUnlockID Barnacle;
		public static MultiplayerUnlocks.SandboxUnlockID SandGrub;
		public static MultiplayerUnlocks.SandboxUnlockID BigSandGrub;
		public static MultiplayerUnlocks.SandboxUnlockID BigMoth;
		public static MultiplayerUnlocks.SandboxUnlockID SmallMoth;
		public static MultiplayerUnlocks.SandboxUnlockID BoxWorm;
		public static MultiplayerUnlocks.SandboxUnlockID FireSprite;
		public static MultiplayerUnlocks.SandboxUnlockID Rattler;
		public static MultiplayerUnlocks.SandboxUnlockID SkyWhale;
		public static MultiplayerUnlocks.SandboxUnlockID ScavengerTemplar;
		public static MultiplayerUnlocks.SandboxUnlockID ScavengerDisciple;
		public static MultiplayerUnlocks.SandboxUnlockID Loach;
		public static MultiplayerUnlocks.SandboxUnlockID RotLoach;
		public static MultiplayerUnlocks.SandboxUnlockID BlizzardLizard;
		public static MultiplayerUnlocks.SandboxUnlockID BasiliskLizard; 
		public static MultiplayerUnlocks.SandboxUnlockID IndigoLizard;
		public static MultiplayerUnlocks.SandboxUnlockID Rat;
		public static MultiplayerUnlocks.SandboxUnlockID Frog;
        public static MultiplayerUnlocks.SandboxUnlockID Tardigrade;
        public static MultiplayerUnlocks.SandboxUnlockID Stowaway;
        public static List<MultiplayerUnlocks.SandboxUnlockID> WatcherUnlocks;

        public static void Start()
        {
			if (!WConfig.cfgSandbox.Value)
			{
				return;
			}
            FireSpriteLarva = new MultiplayerUnlocks.SandboxUnlockID("FireSpriteLarva", true);
            Boomerang = new MultiplayerUnlocks.SandboxUnlockID("Boomerang", true);
             
            DrillCrab = new MultiplayerUnlocks.SandboxUnlockID("DrillCrab", true);
            Barnacle = new MultiplayerUnlocks.SandboxUnlockID("Barnacle", true);
            SandGrub = new MultiplayerUnlocks.SandboxUnlockID("SandGrub", true);
            BigSandGrub = new MultiplayerUnlocks.SandboxUnlockID("BigSandGrub", true);
            BigMoth = new MultiplayerUnlocks.SandboxUnlockID("BigMoth", true);
            SmallMoth = new MultiplayerUnlocks.SandboxUnlockID("SmallMoth", true);
            BoxWorm = new MultiplayerUnlocks.SandboxUnlockID("BoxWorm", true);
            FireSprite = new MultiplayerUnlocks.SandboxUnlockID("FireSprite", true);
            Rattler = new MultiplayerUnlocks.SandboxUnlockID("Rattler", true);
            SkyWhale = new MultiplayerUnlocks.SandboxUnlockID("SkyWhale", true);
            ScavengerTemplar = new MultiplayerUnlocks.SandboxUnlockID("ScavengerTemplar", true);
            ScavengerDisciple = new MultiplayerUnlocks.SandboxUnlockID("ScavengerDisciple", true);
            Loach = new MultiplayerUnlocks.SandboxUnlockID("Loach", true);
            RotLoach = new MultiplayerUnlocks.SandboxUnlockID("RotLoach", true);
            BlizzardLizard = new MultiplayerUnlocks.SandboxUnlockID("BlizzardLizard", true);
            BasiliskLizard = new MultiplayerUnlocks.SandboxUnlockID("BasiliskLizard", true);
            IndigoLizard = new MultiplayerUnlocks.SandboxUnlockID("IndigoLizard", true);
            Rat = new MultiplayerUnlocks.SandboxUnlockID("Rat", true);
            Frog = new MultiplayerUnlocks.SandboxUnlockID("Frog", true);
            Tardigrade = new MultiplayerUnlocks.SandboxUnlockID("Tardigrade", true);


            MultiplayerUnlocks.CreatureUnlockList.Add(DrillCrab);	
			MultiplayerUnlocks.CreatureUnlockList.Add(Barnacle);	
			MultiplayerUnlocks.CreatureUnlockList.Add(SandGrub);  
			MultiplayerUnlocks.CreatureUnlockList.Add(BigSandGrub);  
			MultiplayerUnlocks.CreatureUnlockList.Add(BigMoth);   
			MultiplayerUnlocks.CreatureUnlockList.Add(SmallMoth);  
			MultiplayerUnlocks.CreatureUnlockList.Add(BoxWorm);
			MultiplayerUnlocks.CreatureUnlockList.Add(FireSprite);  
			MultiplayerUnlocks.CreatureUnlockList.Add(Rattler);  
			MultiplayerUnlocks.CreatureUnlockList.Add(SkyWhale);  
			MultiplayerUnlocks.CreatureUnlockList.Add(ScavengerTemplar);  
			MultiplayerUnlocks.CreatureUnlockList.Add(ScavengerDisciple);  
			MultiplayerUnlocks.CreatureUnlockList.Add(Loach);  
			MultiplayerUnlocks.CreatureUnlockList.Add(RotLoach);   
			MultiplayerUnlocks.CreatureUnlockList.Add(BlizzardLizard);
			MultiplayerUnlocks.CreatureUnlockList.Add(BasiliskLizard);  
			MultiplayerUnlocks.CreatureUnlockList.Add(IndigoLizard);   
			MultiplayerUnlocks.CreatureUnlockList.Add(Rat);  
			MultiplayerUnlocks.CreatureUnlockList.Add(Frog);
            MultiplayerUnlocks.CreatureUnlockList.Add(Tardigrade);

            MultiplayerUnlocks.ItemUnlockList.Add(FireSpriteLarva);
            MultiplayerUnlocks.ItemUnlockList.Add(Boomerang);

            On.MultiplayerUnlocks.SandboxItemUnlocked += MultiplayerUnlocks_SandboxItemUnlocked;

            if (WConfig.cfgDebugInfo.Value)
            {
                //MultiplayerUnlocks.CreatureUnlockList.Add(MoreSlugcatsEnums.SandboxUnlockID.StowawayBug);
            }

            //IL.SandboxGameSession.SpawnEntity += StowawayFix;
            //IL.ArenaBehaviors.SandboxEditor.AddIcon_IconSymbolData_Vector2_EntityID_bool_bool += StowawayFix;
                   
            IL.SandboxGameSession.SpawnEntity += FixBigSandGrubBlackScreen;
            On.Watcher.SandGrubNetwork.SpawnGrub += Arena_FixWormSize;
            On.SandboxGameSession.SpawnCreatures += SandboxGameSession_SpawnCreatures;
        }

        private static void SandboxGameSession_SpawnCreatures(On.SandboxGameSession.orig_SpawnCreatures orig, SandboxGameSession self)
        {
            GrubList = new List<bool>();
            orig(self);
        }

        private static void FixBigSandGrubBlackScreen(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
              x => x.MatchLdsfld("Watcher.WatcherEnums/CreatureTemplateType", "SandGrub")))
            {
                c.EmitDelegate<Func<CreatureTemplate.Type, CreatureTemplate.Type>>((self) =>
                {
                    if (self == WatcherEnums.CreatureTemplateType.BigSandGrub)
                    {
                        GrubList.Add(true);
                        return Watcher.WatcherEnums.CreatureTemplateType.SandGrub;
                    }
                    else if (self == WatcherEnums.CreatureTemplateType.SandGrub)
                    {
                        GrubList.Add(false);
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log("SandboxGameSession_SpawnEntity Hook Failed");
            }
        }


        private static void Arena_FixWormSize(On.Watcher.SandGrubNetwork.orig_SpawnGrub orig, SandGrubNetwork self, bool big)
        {
            if (self.room.game.session is ArenaGameSession)
            {
                if (GrubList != null && GrubList.Count > 0)
                {
                    big = GrubList[0];
                    GrubList.RemoveAt(0);
                }
            }
            orig(self, big);
        }
        public static List<bool> GrubList;



 
        private static void StowawayFix(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
              x => x.MatchLdsfld("DLCSharedEnums/CreatureTemplateType", "StowawayBug")))
            {
                c.EmitDelegate<Func<CreatureTemplate.Type, CreatureTemplate.Type>>((self) =>
                {
                   
                    return null;
                });
            }
            else
            {
                Debug.Log("SandboxGameSession_SpawnEntity Hook Failed");
            }
        }

        private static bool MultiplayerUnlocks_SandboxItemUnlocked(On.MultiplayerUnlocks.orig_SandboxItemUnlocked orig, MultiplayerUnlocks self, MultiplayerUnlocks.SandboxUnlockID unlockID)
        {
            if (WConfig.cfgDebugInfo.Value)
            {
                return true;
            }
           
            if (WatcherUnlocks == null)
            {
                WatcherUnlocks = new List<MultiplayerUnlocks.SandboxUnlockID>
                {
                     FireSpriteLarva,
                     Boomerang,
                     DrillCrab,
                     Barnacle,
                     SandGrub,
                     BigSandGrub,
                     BigMoth,
                     SmallMoth,
                     BoxWorm,
                     FireSprite,
                     Rattler,
                     SkyWhale,
                     ScavengerTemplar,
                     ScavengerDisciple,
                     Loach,
                     RotLoach,
                     BlizzardLizard,
                     BasiliskLizard,
                     IndigoLizard,
                     Rat,
                     Frog,
                     Tardigrade,
                };
            }
            if (WatcherUnlocks.Contains(unlockID))
            {
                return true;
            }
            return orig(self,unlockID);
        }

 
    }
}