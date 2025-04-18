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
    internal class ST_ExpeditionStuff
    {
        public static void Start()
        {
            On.Watcher.SpinningTop.StartConversation += DontRepeatConversationsExpedition;

            On.Watcher.SpinningTop.RaiseRippleLevel += DontRaiseRippleLevelNormaly;
            //On.Watcher.SpinningTop.NextMinMaxRippleLevel += SpinningTop_NextMinMaxRippleLevel;
        }


        private Vector2 SpinningTop_NextMinMaxRippleLevel(On.Watcher.SpinningTop.orig_NextMinMaxRippleLevel orig, Room room)
        {
            if (Custom.rainWorld.ExpeditionMode)
            {
                float num = Mathf.Min(5f, (room.game.session as StoryGameSession).saveState.deathPersistentSaveData.maximumRippleLevel + 0.5f);
                float x = Mathf.Max(1f, num - (float)((WConfig.cfgRippleMaxDifference.Value - 1) / 2f));

            }

            return orig(room);
        }


        public static void DontRaiseRippleLevelNormaly(On.Watcher.SpinningTop.orig_RaiseRippleLevel orig, Room room)
        {
            if (Custom.rainWorld.ExpeditionMode)
            {
                if (!WConfig.cfgWatcher_RippleChange.Value)
                {
                    //Still max ripple
                    Vector2 vector = Watcher.SpinningTop.NextMinMaxRippleLevel(room);
                    (room.game.session as StoryGameSession).saveState.deathPersistentSaveData.rippleLevel = vector.y;
                    return;
                }
            }
            orig(room);
        }

        private static void DontRepeatConversationsExpedition(On.Watcher.SpinningTop.orig_StartConversation orig, SpinningTop self)
        {
            orig(self);
            if (Custom.rainWorld.ExpeditionMode)
            {
                if (WConfig.cfgSpinningTopDialogue.Value)
                {
                    Conversation.ID id = Conversation.ID.None;
                    int encounters = self.room.game.GetStorySession.saveState.deathPersistentSaveData.spinningTopEncounters.Count;

                    encounters -= 4; //We add 4
                    if (encounters < 0)
                    {
                        encounters = 0;
                    }
                    switch (encounters)
                    {
                        case 0:
                            //Vanilla 1
                            id = WatcherEnums.ConversationID.Ghost_ST_V1;
                            break;
                        case 1:
                            //Vanilla 2
                            id = WatcherEnums.ConversationID.Ghost_ST_V2;
                            break;
                        case 2:
                            //Vanilla 3 : Ripple 1
                            id = WatcherEnums.ConversationID.Ghost_ST_V3;
                            break;
                        case 3:
                            //1.5 Ripple
                            id = WatcherEnums.ConversationID.Ghost_ST_N1;
                            break;
                        case 4:
                            //2 & 2.5 Ripple have ...
                            //3 Ripple
                            id = WatcherEnums.ConversationID.Ghost_ST_N5;
                            break;
                        case 5:
                            //3.5 Ripple
                            id = WatcherEnums.ConversationID.Ghost_ST_N4;
                            break;
                        case 6:
                            //4 Ripple is ...
                            //4.5 Ripple
                            id = WatcherEnums.ConversationID.Ghost_ST_N2;
                            break;
                        case 7:
                            //5 Ripple
                            //3 min
                            id = WatcherEnums.ConversationID.Ghost_ST_N3;
                            break;
                        case 8:
                            //3.5 min ...
                            //4 min
                            id = WatcherEnums.ConversationID.Ghost_ST_N6;
                            break;
                        case 9:
                            //4.5 min ...
                            //5 min
                            id = WatcherEnums.ConversationID.Ghost_ST_N7;
                            break;
                        case 10:
                            //WARA
                            id = WatcherEnums.ConversationID.Ghost_ST_RIP1;
                            break;
                    }
                    Debug.Log("ST Expedition Convo : " + id.value);
                    if (id != Conversation.ID.None)
                    {
                        self.currentConversation = new SpinningTop.SpinningTopConversation(id, self, self.room.game.cameras[0].hud.dialogBox);
                    }
                }
                else
                {
                    self.currentConversation = new SpinningTop.SpinningTopConversation(Conversation.ID.None, self, self.room.game.cameras[0].hud.dialogBox);
                    self.currentConversation.events.Add(new Conversation.TextEvent(self.currentConversation, 0, "...", 300));
                }
                
            }
        
            
        }
    }
}
