using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class BUS_COMMENT_REPLY
    {
        /// <summary>
        /// 
        /// </summary>
        public BUS_COMMENT_REPLY()
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

        private System.String _STATE;
        /// <summary>
        /// 
        /// </summary>
        public System.String STATE { get { return this._STATE; } set { this._STATE = value; } }

        private System.Int32 _TIMESTAMP_INT;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 TIMESTAMP_INT { get { return this._TIMESTAMP_INT; } set { this._TIMESTAMP_INT = value; } }

        private System.String _COMMENT_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String COMMENT_ID { get { return this._COMMENT_ID; } set { this._COMMENT_ID = value; } }

        private System.String _REPLY_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String REPLY_ID { get { return this._REPLY_ID; } set { this._REPLY_ID = value; } }

        private System.String _REPLY_TYPE;
        /// <summary>
        /// 
        /// </summary>
        public System.String REPLY_TYPE { get { return this._REPLY_TYPE; } set { this._REPLY_TYPE = value; } }

        private System.String _CONTENT;
        /// <summary>
        /// 
        /// </summary>
        public System.String CONTENT { get { return this._CONTENT; } set { this._CONTENT = value; } }

        private System.String _FROM_UID;
        /// <summary>
        /// 
        /// </summary>
        public System.String FROM_UID { get { return this._FROM_UID; } set { this._FROM_UID = value; } }

        private System.String _TO_UID;
        /// <summary>
        /// 
        /// </summary>
        public System.String TO_UID { get { return this._TO_UID; } set { this._TO_UID = value; } }
    }
}
