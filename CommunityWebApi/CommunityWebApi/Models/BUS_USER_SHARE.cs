using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class BUS_USER_SHARE
    {
        /// <summary>
        /// 
        /// </summary>
        public BUS_USER_SHARE()
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

        private System.String _USER_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String USER_ID { get { return this._USER_ID; } set { this._USER_ID = value; } }

        private System.String _TO_SHARED_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String TO_SHARED_ID { get { return this._TO_SHARED_ID; } set { this._TO_SHARED_ID = value; } }

        private System.String _CONTENT_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String CONTENT_ID { get { return this._CONTENT_ID; } set { this._CONTENT_ID = value; } }

        private System.Int32 _SHARE_TYPE;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 SHARE_TYPE { get { return this._SHARE_TYPE; } set { this._SHARE_TYPE = value; } }
    }
}
