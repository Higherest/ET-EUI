namespace ET
{
    public enum AccountType
    {
        General = 0, //普通
        BlackList = 1, //黑名单
    }

    public class Account: Entity, IAwake
    {
        public string accountName; //账户名

        public string passWord; //账户密码

        public long createTime; //账号创建时间
        
        public int accountType; //账号类型
    }
}