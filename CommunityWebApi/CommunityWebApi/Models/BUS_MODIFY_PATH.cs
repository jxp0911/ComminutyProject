using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class BUS_MODIFY_PATH
    {
        /// <summary>
        /// 
        /// </summary>
        public BUS_MODIFY_PATH()
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

        private System.String _PATH_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String PATH_ID { get { return this._PATH_ID; } set { this._PATH_ID = value; } }

        private System.Int32 _PATH_CLASS;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 PATH_CLASS { get { return this._PATH_CLASS; } set { this._PATH_CLASS = value; } }

        private System.String _CONTENT;
        /// <summary>
        /// 
        /// </summary>
        public System.String CONTENT { get { return this._CONTENT; } set { this._CONTENT = value; } }

        private System.String _USER_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String USER_ID { get { return this._USER_ID; } set { this._USER_ID = value; } }

        private System.Int32 _SUPPORT;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 SUPPORT { get { return this._SUPPORT; } set { this._SUPPORT = value; } }

        private System.Int32 _OPPOSE;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 OPPOSE { get { return this._OPPOSE; } set { this._OPPOSE = value; } }

        private System.String _IS_MERGE;
        /// <summary>
        /// 
        /// </summary>
        public System.String IS_MERGE { get { return this._IS_MERGE; } set { this._IS_MERGE = value; } }
    }
}
