using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Domin.Abstractions;
using WesleyCore.Message.Domain.Enums.Message;

namespace WesleyCore.User.Domain.Events.User
{
    /// <summary>
    /// 发送用户消息
    /// </summary>
    public class SendUserMessageDomainEvent : IDomainEvent
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <param name="senderID"></param>
        /// <param name="senderName"></param>
        /// <param name="userList"></param>
        public SendUserMessageDomainEvent(string title, string content, MessageTypeEnum type, Guid? senderID, string senderName, Dictionary<Guid, string> userList)
        {
            Title = title;
            Content = content;
            Type = type;
            SenderID = senderID;
            SenderName = senderName;
            UserList = userList;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// 类型
        /// </summary>
        public MessageTypeEnum Type { get; private set; }

        /// <summary>
        /// 发送人ID
        /// </summary>
        public Guid? SenderID { get; private set; }

        /// <summary>
        /// 发送人姓名
        /// </summary>
        public string SenderName { get; private set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public Dictionary<Guid, string> UserList { get; private set; }
    }
}