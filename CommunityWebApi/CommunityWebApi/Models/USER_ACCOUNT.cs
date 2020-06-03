using SqlSugar;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class USER_ACCOUNT
    {
        /// <summary>
        /// 
        /// </summary>
        public USER_ACCOUNT()
        {
        }

        private System.String _id;
        /// <summary>
        /// 
        /// </summary>
        public System.String id { get { return this._id; } set { this._id = value; } }

        private System.DateTime _datetime_created;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime datetime_created { get { return this._datetime_created; } set { this._datetime_created = value; } }

        private System.DateTime? _datetime_modified;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? datetime_modified { get { return this._datetime_modified; } set { this._datetime_modified = value; } }

        private System.String _state;
        /// <summary>
        /// 
        /// </summary>
        public System.String state { get { return this._state; } set { this._state = value; } }

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
    }
}
