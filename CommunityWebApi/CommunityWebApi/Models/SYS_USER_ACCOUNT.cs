using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class SYS_USER_ACCOUNT
    {
        /// <summary>
        /// 
        /// </summary>
        public SYS_USER_ACCOUNT()
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

        private System.String _ACCOUNT_NUMBER;
        /// <summary>
        /// 
        /// </summary>
        public System.String ACCOUNT_NUMBER { get { return this._ACCOUNT_NUMBER; } set { this._ACCOUNT_NUMBER = value; } }

        private System.String _PASSWORD;
        /// <summary>
        /// 
        /// </summary>
        public System.String PASSWORD { get { return this._PASSWORD; } set { this._PASSWORD = value; } }

        private System.Int32 _TIMESTAMP_INT;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 TIMESTAMP_INT { get { return this._TIMESTAMP_INT; } set { this._TIMESTAMP_INT = value; } }
    }
}
