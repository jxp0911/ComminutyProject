using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class SYS_TABS_INFO
    {
        /// <summary>
        /// 
        /// </summary>
        public SYS_TABS_INFO()
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

        private System.String _TAB_CODE;
        /// <summary>
        /// 
        /// </summary>
        public System.String TAB_CODE { get { return this._TAB_CODE; } set { this._TAB_CODE = value; } }

        private System.String _TAB_NAME;
        /// <summary>
        /// 
        /// </summary>
        public System.String TAB_NAME { get { return this._TAB_NAME; } set { this._TAB_NAME = value; } }
        private System.Int32 _SEQ;
        /// <summary>
        /// 显示顺序
        /// </summary>
        public System.Int32 SEQ { get { return this._SEQ; } set { this._SEQ = value; } }
    }
}
