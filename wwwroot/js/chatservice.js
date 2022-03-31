var chatService = {
    groupID: 0,
    userPic: '',
    groupHostObj: '',
    contactArr: [],
    LastMsgID: 0,
    fnloadContactData: function () {
        $.ajax({
            url: '/Account/GetAllContactList',
            dataType: "json",
            data: {},
            method: 'GET',
            success: function (data) {
                var playdataTable = $('.users');
                playdataTable.empty();
                var isFirst = true;

                chatService.contactArr = [];
                chatService.contactArr = data.data;

                $(data.data).each(function (index, relationModelObj) {

                    if (!relationModelObj.isSelfAccount) {
                        if (isFirst) {
                            isFirst = false;
                            chatService.groupID = relationModelObj.groupID;
                            chatService.userPic = relationModelObj.contactPic;
                        }
                        var ppic = (relationModelObj.contactPic ? '/Blogs/ProfileData/' + relationModelObj.contactPic : 'https://www.bootdey.com/img/Content/avatar/avatar3.png');
                        playdataTable.append('<li class="person" data-chat="person1"><div class="user"><img src="' + ppic + '" alt="' + relationModelObj.groupID + '">' +
                            '</div><p class="name-time"><span class="name">' + relationModelObj.contactName + '</span><br /><span class="time">' + relationModelObj.lastDateTime + '</span></p></li>');
                    }
                    else
                        chatService.groupHostObj = relationModelObj;
                });
                chatService.fnloadChatData();
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnloadChatData: function () {
        const messagesDiv = document.getElementsByClassName('chatContainerScroll');
        $.ajax({
            url: '/Account/GetAllChat',
            dataType: "json",
            data: { 'groupID': chatService.groupID, 'LastMsgID': chatService.LastMsgID },
            method: 'GET',
            success: function (data) {
                if (data.data.length > 0) {
                    var playdataTable = $('.chatContainerScroll');
                    if (chatService.LastMsgID == 0)
                        playdataTable.empty();

                    var groupPartyObj = chatService.fnfinditemFromContactList(chatService.contactArr, 'groupID', chatService.groupID);
                    $('.name').html(groupPartyObj[0].contactName);
                    $(data.data).each(function (index, relationModelObj) {

                        if (relationModelObj.isSenderSelfAccount) {
                            var ppic = (chatService.groupHostObj.contactPic ? '/Blogs/ProfileData/' + chatService.groupHostObj.contactPic : 'https://www.bootdey.com/img/Content/avatar/avatar3.png');
                            playdataTable.append('<li class="chat-right"><div class="chat-hour">' + relationModelObj.dateCreated + '</div><div class="chat-text">' + relationModelObj.chatMessage + '</div>' +
                                '<div class="chat-avatar"><img src="' + ppic + '" alt="Retail Admin">' +
                                '<div class="chat-name">' + chatService.groupHostObj.contactName + '</div></div></li>');
                        }
                        else {
                            var ppic = (groupPartyObj[0].contactPic ? '/Blogs/ProfileData/' + groupPartyObj[0].contactPic : 'https://www.bootdey.com/img/Content/avatar/avatar3.png');
                            playdataTable.append('<li class="chat-left"><div class="chat-avatar"><img src="' + ppic + '">' +
                                '<div class="chat-name">' + groupPartyObj[0].contactName + '</div></div><div class="chat-text">' + relationModelObj.chatMessage + '</div>' +
                                ' <div class="chat-hour">' + relationModelObj.dateCreated + '</div></li>');
                        }

                        chatService.LastMsgID = relationModelObj.msgID;
                    });

                    messagesDiv[0].scrollTop = messagesDiv[0].scrollHeight;
                }
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnfinditemFromContactList: function (obj, key, val) {

        var objects = [];
        for (var i in obj) {
            if (!obj.hasOwnProperty(i)) continue;
            if (typeof obj[i] == 'object') {
                objects = objects.concat(chatService.fnfinditemFromContactList(obj[i], key, val));
            } else if (i == key && obj[key] == val && obj['isSelfAccount'] == false) {
                objects.push(obj);
            }
        }
        return objects;
    },
    fnloadnewgroup: function (userid) {
        $.ajax({
            url: '/Account/LoadNewGroup',
            dataType: "json",
            data: { 'userID': userid },
            method: 'GET',
            success: function (data) {
                if (data.data.groupname != null) {
                    window.location.href = '/Account/Message/';
                }
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
};