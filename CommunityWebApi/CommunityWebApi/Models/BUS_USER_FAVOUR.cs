using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class BUS_USER_FAVOUR
    {
        /// <summary>
        /// 
        /// </summary>
        public BUS_USER_FAVOUR()
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

        private System.String _TYPE_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String TYPE_ID { get { return this._TYPE_ID; } set { this._TYPE_ID = value; } }

        private System.String _USER_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String USER_ID { get { return this._USER_ID; } set { this._USER_ID = value; } }

        private System.Int32 _TYPE;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 TYPE { get { return this._TYPE; } set { this._TYPE = value; } }
    }
}
