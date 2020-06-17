using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class BUS_CAREERPATH_COMMENT
    {
        /// <summary>
        /// 
        /// </summary>
        public BUS_CAREERPATH_COMMENT()
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

        private System.Int32 _PATH_CLASS;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 PATH_CLASS { get { return this._PATH_CLASS; } set { this._PATH_CLASS = value; } }
    }
}
