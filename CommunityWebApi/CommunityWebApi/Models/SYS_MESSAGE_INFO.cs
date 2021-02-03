using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class SYS_MESSAGE_INFO
    {
        /// <summary>
        /// 
        /// </summary>
        public SYS_MESSAGE_INFO()
        {
        }

        private System.String _ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String ID { get { return this._ID; } set { this._ID = value; } }

        private System.DateTime _DATETIME_CREATED;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime DATETIME_CREATED { get { return this._DATETIME_CREATED; } set { this._DATETIME_CREATED = value; } }

        private System.DateTime? _DATETIME_MODIFIED;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? DATETIME_MODIFIED { get { return this._DATETIME_MODIFIED; } set { this._DATETIME_MODIFIED = value; } }

        private System.Int32 _TIMESTAMP_INT;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 TIMESTAMP_INT { get { return this._TIMESTAMP_INT; } set { this._TIMESTAMP_INT = value; } }

        private System.String _STATE;
        /// <summary>
        /// 
        /// </summary>
        public System.String STATE { get { return this._STATE; } set { this._STATE = value; } }

        private System.String _CONTENT;
        /// <summary>
        /// 通知内容
        /// </summary>
        public System.String CONTENT { get { return this._CONTENT; } set { this._CONTENT = value; } }

        private System.String _FROM_UID;
        /// <summary>
        /// 发起通知的人
        /// </summary>
        public System.String FROM_UID { get { return this._FROM_UID; } set { this._FROM_UID = value; } }

        private System.String _TO_UID;
        /// <summary>
        /// 被通知人
        /// </summary>
        public System.String TO_UID { get { return this._TO_UID; } set { this._TO_UID = value; } }

        private System.Int32 _STATUS;
        /// <summary>
        /// 状态(0：未查看；1：已查看)
        /// </summary>
        public System.Int32 STATUS { get { return this._STATUS; } set { this._STATUS = value; } }

        private System.Int32 _TYPE;
        /// <summary>
        /// 通知类型(1：评论)
        /// </summary>
        public System.Int32 TYPE { get { return this._TYPE; } set { this._TYPE = value; } }
    }
}
