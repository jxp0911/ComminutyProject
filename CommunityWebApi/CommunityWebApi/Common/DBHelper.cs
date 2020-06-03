using SqlSugar;
/// <summary>
/// SqlSugar
/// </summary>
public class DBContext
{
    private DBContext()
    {
    }
    public static SqlSugarClient GetInstance
    {
        get
        {
            var connStr = "database='communitydb';Data Source = '120.27.4.196'; User Id = 'root'; pwd='root123';charset='utf8';pooling=true";
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connStr,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                IsShardSameThread = true,
                InitKeyType = InitKeyType.Attribute
            });
            db.Ado.CommandTimeOut = 30000;//设置超时时间
            return db;
        }
    }
}