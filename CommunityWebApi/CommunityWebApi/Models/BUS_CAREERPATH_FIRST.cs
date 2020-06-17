using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class BUS_CAREERPATH_FIRST
    {
        /// <summary>
        /// 
        /// </summary>
        public BUS_CAREERPATH_FIRST()
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

        private System.String _TITLE;
        /// <summary>
        /// 
        /// </summary>
        public System.String TITLE { get { return this._TITLE; } set { this._TITLE = value; } }

        private System.String _USER_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String USER_ID { get { return this._USER_ID; } set { this._USER_ID = value; } }

        private System.Int32 _TIMESTAMP_INT;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 TIMESTAMP_INT { get { return this._TIMESTAMP_INT; } set { this._TIMESTAMP_INT = value; } }

        private System.Int32 _STATUS;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 STATUS { get { return this._STATUS; } set { this._STATUS = value; } }

        private System.Int32 _FAVOUR_COUNT;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 FAVOUR_COUNT { get { return this._FAVOUR_COUNT; } set { this._FAVOUR_COUNT = value; } }
    }
}
