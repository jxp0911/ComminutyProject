using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class BUS_PLAN_HEADER
    {
        /// <summary>
        /// 
        /// </summary>
        public BUS_PLAN_HEADER()
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
        /// 发布人
        /// </summary>
        public System.String USER_ID { get { return this._USER_ID; } set { this._USER_ID = value; } }

        private System.Int32 _STATUS;
        /// <summary>
        /// 0:报废;1:已完成;2:使用中;
        /// </summary>
        public System.Int32 STATUS { get { return this._STATUS; } set { this._STATUS = value; } }

        private System.Int32 _FAVOUR_COUNT;
        /// <summary>
        /// 点赞数
        /// </summary>
        public System.Int32 FAVOUR_COUNT { get { return this._FAVOUR_COUNT; } set { this._FAVOUR_COUNT = value; } }

        private System.Int32 _SOURCE_TYPE;
        /// <summary>
        /// 计划来源(1:原创;2:借鉴)
        /// </summary>
        public System.Int32 SOURCE_TYPE { get { return this._SOURCE_TYPE; } set { this._SOURCE_TYPE = value; } }

        private System.String _FIRST_PATH_ID;
        /// <summary>
        /// 职业规划ID
        /// </summary>
        public System.String FIRST_PATH_ID { get { return this._FIRST_PATH_ID; } set { this._FIRST_PATH_ID = value; } }

        private System.Int32 _IS_SHARED;
        /// <summary>
        /// 是否被分享
        /// </summary>
        public System.Int32 IS_SHARED { get { return this._IS_SHARED; } set { this._IS_SHARED = value; } }

        private System.String _SOURCE_USER_ID;
        /// <summary>
        /// 原创用户ID
        /// </summary>
        public System.String SOURCE_USER_ID { get { return this._SOURCE_USER_ID; } set { this._SOURCE_USER_ID = value; } }

        private System.Int32 _SHARED_COUNT;
        /// <summary>
        /// 被应用的次数
        /// </summary>
        public System.Int32 SHARED_COUNT { get { return this._SHARED_COUNT; } set { this._SHARED_COUNT = value; } }

        private System.Int32 _SHARE_VERSION;
        /// <summary>
        /// 已分享的版本号
        /// </summary>
        public System.Int32 SHARE_VERSION { get { return this._SHARE_VERSION; } set { this._SHARE_VERSION = value; } }
    }
}
