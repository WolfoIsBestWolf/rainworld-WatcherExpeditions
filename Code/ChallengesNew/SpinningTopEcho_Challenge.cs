using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using System.Text.RegularExpressions;
using System;
using RWCustom;
using Menu.Remix;

namespace WatcherExpeditions
{
    public class ST_EchoChallenge : Challenge
    {
        // Token: 0x060032F4 RID: 13044 RVA: 0x003AF904 File Offset: 0x003ADB04
        public override void UpdateDescription()
        {
            string text = Region.GetRegionFullName(this.regionName, ExpeditionData.slugcatPlayer);
            this.description = T.Translate("Challenge_ST_Desc").Replace("<region>", ChallengeTools.IGT.Translate(text));
            base.UpdateDescription();
        }

        public override bool CanBeHidden()
        {
            return true;
        }
        public override void Update()
        {
            base.Update(); 
            for (int i = 0; i < this.game.Players.Count; i++)
            {
                //ELog.Log(this.game.Players[i].world.name);
                if (this.game.Players[i].world.name == this.regionName)
                {
                    if (this.game.Players[i] != null && this.game.Players[i].realizedCreature != null && this.game.Players[i].realizedCreature.room != null)
                    {
                        for (int j = 0; j < this.game.Players[i].realizedCreature.room.updateList.Count; j++)
                        {
                            if (this.game.Players[i].realizedCreature.room.updateList[j] is Ghost && this.game.Players[i].world.spinningTopPresences.Count > 0 && (this.game.Players[i].realizedCreature.room.updateList[j] as Ghost).onScreenCounter > 30)
                            {
                                this.CompleteChallenge();
                                ExpLog.Log("ST_EchoChallenge Complete!");
                            }
                        }
                    }
                }
            }
        }


        public override int Points()
        {
            return 60 * (int)(this.hidden ? 2f : 1f);
        }


        public override Challenge Generate()
        {
            List<string> list = new List<string>()
            {
                "WARF",
                "WTDB",
                "WBLA",
                "WRFB",
                "WTDA",
                "WARE",
                "WSKC",
                "WVWA",
                "WPTA",
                "WARC",
                "WARB",
                "WVWB",               
                //"WARD", //Removed 1.5
                //"WSKD", //Removed 1.5
            };
           
            string region = list[UnityEngine.Random.Range(0, list.Count)];
            return new ST_EchoChallenge
            {
                regionName = region
            };
        }
 
        public override bool CombatRequired()
        {
            return false;
        }

 
        public override bool Duplicable(Challenge challenge)
        {
            return !(challenge is ST_EchoChallenge) || !((challenge as ST_EchoChallenge).regionName == regionName);
        }

 
        public override string ChallengeName()
        {
            return T.Translate("Challenge_ST_Name");
            return "Spinning Top";

        }

 
        public override string ToString()
        {
            return string.Concat(new string[]
            {
                "ST_EchoChallenge",
                "~",
                this.regionName,
                "><",
                this.completed ? "1" : "0",
                "><",
                this.hidden ? "1" : "0",
                "><",
                this.revealed ? "1" : "0"
            });
        }

 
        public override void FromString(string args)
        {
            try
            {
                string[] array = Regex.Split(args, "><");
                this.regionName = array[0];
                this.completed = (array[1] == "1");
                this.hidden = (array[2] == "1");
                this.revealed = (array[3] == "1");
                this.UpdateDescription();

                if (regionName == "WARD" || regionName == "WSKD")
                {
                    //Were removed in 1.5.
                    //Backwards compat auto complete
                    this.completed = true;
                }
            }
            catch (Exception ex)
            {
                ExpLog.Log("ERROR: ST_EchoChallenge FromString() encountered an error: " + ex.Message);
            }
        }

 
        public string regionName;

        public override bool ValidForThisSlugcat(SlugcatStats.Name slugcat)
        {
            return slugcat == Watcher.WatcherEnums.SlugcatStatsName.Watcher;
        }
    }
}