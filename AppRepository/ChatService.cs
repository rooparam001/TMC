using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using System;
using System.Collections.Generic;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public static class ChatService
    {
        public static bool SaveChat(ChatServiceMessageListModel model)
        {
            var _db = new TMCDBContext();
            var respObj = false;
            try
            {
                if (model != null)
                {
                    if (model.SenderID > 0 && !string.IsNullOrEmpty(model.ChatMessage))
                    {
                        if (model.GroupID == 0)
                        {
                            model.GroupID = SaveGroup(model).ID;
                        }

                        respObj = _db.fn_SaveChat(new TBL_CHATMESSAGEMASTER()
                        {
                            CHATMASTERID = model.GroupID,
                            CHATMESSAGE = model.ChatMessage.Trim(),
                            DATECREATED = DateTime.Now,
                            SENDERID = model.SenderID
                        });
                    }
                }
            }
            catch (Exception ex) { respObj = false; }
            return respObj;
        }

        public static TBL_CHATGROUPMASTER SaveGroup(ChatServiceMessageListModel model)
        {
            var _db = new TMCDBContext();
            var respObj = new TBL_CHATGROUPMASTER();
            try
            {
                respObj = new TBL_CHATGROUPMASTER()
                {
                    GROUPNAME = model.SenderID.ToString() + "-" + model.UserID.ToString(),
                    HOSTID = model.SenderID,
                    PARTYID = model.UserID,
                    DATECREATED = DateTime.Now
                };
                respObj = _db.fn_SaveChatGroup(respObj);
            }
            catch (Exception ex) { respObj = new TBL_CHATGROUPMASTER(); }
            return respObj;
        }

        public static List<ChatServiceContactModel> GetAll(int sourceID)
        {
            var _db = new TMCDBContext();
            var respObj = new List<ChatServiceContactModel>();
            try
            {
                respObj = _db.fn_GetAllChatContactsByUserID(sourceID);
            }
            catch { }
            return respObj;
        }

        public static List<ChatServiceMessageListModel> GetGroupChat(int GroupID, int UserID)
        {
            var _db = new TMCDBContext();
            var respObj = new List<ChatServiceMessageListModel>();
            try
            {
                respObj = _db.fn_GetAllChatByGroupID(GroupID, UserID);
            }
            catch { }
            return respObj;
        }
    }
}
