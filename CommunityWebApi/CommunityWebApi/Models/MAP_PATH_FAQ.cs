using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class MAP_PATH_FAQ
    {
        /// <summary>
        /// 
        /// </summary>
        public MAP_PATH_FAQ()
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

        private System.String _FIRST_PATH_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String FIRST_PATH_ID { get { return this._FIRST_PATH_ID; } set { this._FIRST_PATH_ID = value; } }

        private System.String _FAQ_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.String FAQ_ID { get { return this._FAQ_ID; } set { this._FAQ_ID = value; } }
    }
}
