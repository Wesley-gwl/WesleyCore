using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Message.Domain.Enums.Message;

namespace WesleyCore.Message.Application.Commands.Message
{
    /// <summary>
    /// 新增消息命令
    /// </summary>
    public class CreateMessageCommand : IRequest<bool>
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
        public CreateMessageCommand(string title, string content, MessageTypeEnum type, Guid? senderID, string senderName, Dictionary<Guid, string> userList)
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