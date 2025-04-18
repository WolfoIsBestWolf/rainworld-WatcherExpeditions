using Expedition;
using Menu.Remix;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace WatcherExpeditions
{
    //Infest X regions, Neuron challenge replacement
    public class RotSpreading_Challenge : Challenge
    {
        public int target;
        public int rotted;


        public override void UpdateDescription()
        {
            this.description = ChallengeTools.IGT.Translate("Infect <score_target> regions with sentient rot [<current_score>/<score_target>]").Replace("<score_target>", ValueConverter.ConvertToString<int>(this.target)).Replace("<current_score>", ValueConverter.ConvertToString<int>(this.rotted));
            base.UpdateDescription();

            Debug.Log("RotSpreading_Challenge UpdateDescription");
            if (this.game != null && this.game.rainWorld.progression.currentSaveState != null)
            {
                Debug.Log("Count : " + game.rainWorld.progression.currentSaveState.miscWorldSaveData.regionsInfectedBySentientRotSpread.Count);
                foreach (string a in this.game.rainWorld.progression.currentSaveState.miscWorldSaveData.regionsInfectedBySentientRotSpread)
                {
                    Debug.Log("regionsInfectedBySentientRotSpread : " + a);
                }
            }
        }

        public override bool CanBeHidden()
        {
            return true;
        }


        public override Challenge Generate()
        {
            if (ExpeditionData.challengeDifficulty == 0.5f)
            {
                return new RotSpreading_Challenge
                {
                    target = 5,
                };
            }
            return new RotSpreading_Challenge
            {
                target = (int)Mathf.Lerp(3f, 8f, ExpeditionData.challengeDifficulty),
            };
        }

        //Running these every frame seems inefficient
        public override void Update()
        {
            base.Update();
            if (this.game != null && this.game.rainWorld.progression.currentSaveState != null)
            {
                //Somehow if this isn't here it completes based on previous cycle if you died.
                if (this.revealCheckDelay >= 100)
                {
                    if (this.game.rainWorld.progression.currentSaveState.miscWorldSaveData.regionsInfectedBySentientRotSpread.Count != this.rotted)
                    {
                        game.rainWorld.progression.currentSaveState.miscWorldSaveData.regionsInfectedBySentientRotSpread.Remove("");
                        this.rotted = this.game.rainWorld.progression.currentSaveState.miscWorldSaveData.regionsInfectedBySentientRotSpread.Count;
                        this.UpdateDescription();
                    }
                    if (!this.completed && this.rotted >= this.target)
                    {
                        this.CompleteChallenge();
                    }
                }
            }

        }


        public override string ChallengeName()
        {
            return ChallengeTools.IGT.Translate("Rot Spreading");
        }


        public override bool ValidForThisSlugcat(SlugcatStats.Name slugcat)
        {
            return slugcat == Watcher.WatcherEnums.SlugcatStatsName.Watcher && WConfig.cfgRotChallenge.Value && WConfig.cfgWatcher_KarmaFlower.Value;
        }

        public override int Points()
        {
            if (target < 4)
            {
                return (int)(target * 22f * (this.hidden ? 2f : 1f));
            }
            return (int)(target * 33f * (this.hidden ? 2f : 1f));
        }

        public override void Reset()
        {
            this.rotted = 0;
            base.Reset();
            Debug.Log("RotSpreading_Challenge Reset");
            if (this.game != null && this.game.rainWorld.progression.currentSaveState != null)
            {
                Debug.Log("Count : " + game.rainWorld.progression.currentSaveState.miscWorldSaveData.regionsInfectedBySentientRotSpread.Count);
                foreach (string a in this.game.rainWorld.progression.currentSaveState.miscWorldSaveData.regionsInfectedBySentientRotSpread)
                {
                    Debug.Log("regionsInfectedBySentientRotSpread : " + a);
                }
            }
        }
        public override bool Duplicable(Challenge challenge)
        {
            return !(challenge is RotSpreading_Challenge);
        }
        public override string ToString()
        {
            return string.Concat(new string[]
            {
                "RotSpreading_Challenge",
                "~",
                ValueConverter.ConvertToString<int>(this.rotted),
                "><",
                ValueConverter.ConvertToString<int>(this.target),
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
                this.rotted = int.Parse(array[0], NumberStyles.Any, CultureInfo.InvariantCulture);
                this.target = int.Parse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture);
                this.completed = (array[2] == "1");
                this.hidden = (array[3] == "1");
                this.revealed = (array[4] == "1");
                this.UpdateDescription();
            }
            catch (Exception ex)
            {
                ExpLog.Log("ERROR: RotSpreading_Challenge FromString() encountered an error: " + ex.Message);
            }
        }

    }
}