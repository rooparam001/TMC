using EntitesInterfaces.AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public static class ChatService
    {
        public static bool Save(ChatServiceViewModel model)
        {
            var _db = new TMCDBContext();
            var respObj = false;
            try
            {
                if (model != null)
                {
                    if (model.SenderID > 0 && model.ReceiverID > 0 && !string.IsNullOrEmpty(model.ChatMsg))
                    {
                        if (_db.fn_CheckChatGroupExists(model))
                        {

                        }
                    }
                }
            }
            catch (Exception ex) { }
            return respObj;
        }
    }
}
