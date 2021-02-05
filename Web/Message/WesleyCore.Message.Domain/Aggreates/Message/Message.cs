using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using WesleyCore.Domin.Abstractions;
using WesleyCore.Message.Domain.Aggreates.Message;
using WesleyCore.Message.Domain.Enums.Message;
using WesleyUntity;

namespace WesleyCore.Message.Domain
{
    /// <summary>
    /// 消息 -不添加租户id
    /// </summary>
    [Table("Message", Schema = "Message")]
    public class Message : Entity<Guid>, IAggregateRoot, ISoftDelete
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected Message()
        {
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <param name="senderID"></param>
        /// <param name="senderName"></param>
        /// <param name="userList"></param>
        public Message(string title, string content, MessageTypeEnum type, Guid? senderID, string senderName, Dictionary<Guid, string> userList)
        {
            Id = ComFunc.NewCombGuid();
            Title = title;
            Content = content;
            Type = type;
            SenderID = senderID;
            SenderName = senderName;
            MessageReceivers = userList.Select(p => new MessageReceiver(p.Key, p.Value)).ToList();
        }

        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(50)]
        public string Title { get; private set; }

        /// <summary>
        /// 正文
        /// </summary>
        [StringLength(400)]
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
        [StringLength(20)]
        public string SenderName { get; private set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// 消息接收人
        /// </summary>
        public List<MessageReceiver> MessageReceivers { get; set; }

        /// <summary>
        /// 软删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}