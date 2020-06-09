using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class SYS_FAIL_LOG
    {
        /// <summary>
        /// 
        /// </summary>
        public SYS_FAIL_LOG()
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

        private System.String _CONTROLLER_NAME;
        /// <summary>
        /// 
        /// </summary>
        public System.String CONTROLLER_NAME { get { return this._CONTROLLER_NAME; } set { this._CONTROLLER_NAME = value; } }

        private System.String _FUN_NAME;
        /// <summary>
        /// 
        /// </summary>
        public System.String FUN_NAME { get { return this._FUN_NAME; } set { this._FUN_NAME = value; } }

        private System.String _URL;
        /// <summary>
        /// 
        /// </summary>
        public System.String URL { get { return this._URL; } set { this._URL = value; } }

        private System.String _INTERFACE_DESC;
        /// <summary>
        /// 
        /// </summary>
        public System.String INTERFACE_DESC { get { return this._INTERFACE_DESC; } set { this._INTERFACE_DESC = value; } }

        private System.String _RECEIVE_DATA;
        /// <summary>
        /// 
        /// </summary>
        public System.String RECEIVE_DATA { get { return this._RECEIVE_DATA; } set { this._RECEIVE_DATA = value; } }

        private System.String _FAIL_MSG;
        /// <summary>
        /// 
        /// </summary>
        public System.String FAIL_MSG { get { return this._FAIL_MSG; } set { this._FAIL_MSG = value; } }

        private System.String _REQUEST_TYPE;
        /// <summary>
        /// 
        /// </summary>
        public System.String REQUEST_TYPE { get { return this._REQUEST_TYPE; } set { this._REQUEST_TYPE = value; } }

        private System.Int32 _TIMESTAMP_INT;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 TIMESTAMP_INT { get { return this._TIMESTAMP_INT; } set { this._TIMESTAMP_INT = value; } }
    }
}
