using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entitys
{
    /// <summary>
    /// 
    /// </summary>
    public class BUS_PLAN_DETAIL
    {
        /// <summary>
        /// 
        /// </summary>
        public BUS_PLAN_DETAIL()
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

        private System.String _HEADER_ID;
        /// <summary>
        /// 父ID
        /// </summary>
        public System.String HEADER_ID { get { return this._HEADER_ID; } set { this._HEADER_ID = value; } }

        private System.Int32 _SEQ;
        /// <summary>
        /// 计划执行顺序
        /// </summary>
        public System.Int32 SEQ { get { return this._SEQ; } set { this._SEQ = value; } }

        private System.String _CONTENT;
        /// <summary>
        /// 计划内容
        /// </summary>
        public System.String CONTENT { get { return this._CONTENT; } set { this._CONTENT = value; } }

        private System.Int32 _STATUS;
        /// <summary>
        /// 0:作废;1:计划完成;2:计划执行中;3:计划待执行
        /// </summary>
        public System.Int32 STATUS { get { return this._STATUS; } set { this._STATUS = value; } }

        private System.Int32? _COMPLETE_TIME;
        /// <summary>
        /// 计划完成时间
        /// </summary>
        public System.Int32? COMPLETE_TIME { get { return this._COMPLETE_TIME; } set { this._COMPLETE_TIME = value; } }

        private System.Int32 _VISIBLE_TYPE;
        /// <summary>
        /// 1:未分享，尽自己可见；2:已分享，全员可见
        /// </summary>
        public System.Int32 VISIBLE_TYPE { get { return this._VISIBLE_TYPE; } set { this._VISIBLE_TYPE = value; } }
    }
}
