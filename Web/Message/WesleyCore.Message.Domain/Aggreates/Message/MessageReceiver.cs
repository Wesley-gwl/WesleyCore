using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Message.Domain.Aggreates.Message
{
    /// <summary>
    /// 消息接收人表
    /// </summary>
    [Table("MessageReceiver", Schema = "Message")]
    public class MessageReceiver : Entity<long>, IAggregateRoot, ISoftDelete
    {
        /// <summary>
        /// 保护类
        /// </summary>
        protected MessageReceiver()
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="receiverID"></param>
        /// <param name="receiverName"></param>

        public MessageReceiver(Guid receiverID, string receiverName)
        {
            ReceiverID = receiverID;
            ReceiverName = receiverName;
        }

        /// <summary>
        /// 接收人id
        /// </summary>
        public Guid ReceiverID { get; private set; }

        /// <summary>
        /// 接收人姓名
        /// </summary>
        [StringLength(20)]
        public string ReceiverName { get; private set; }

        /// <summary>
        /// 是否已推送
        /// </summary>
        public bool IsPushed { get; private set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; private set; }

        /// <summary>
        /// 读取时间
        /// </summary>
        public DateTime? ReadTime { get; private set; }

        /// <summary>
        /// 删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}