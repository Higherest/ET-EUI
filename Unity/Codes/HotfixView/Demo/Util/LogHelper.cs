namespace ET
{
    public static class LogHelper
    {
        public static void ErrorCodeLog(int code)
        {
            if (code == ErrorCode.ERR_PassWordError)
            {
                Log.Error("密码错误");
            }
            else if (code == ErrorCode.ERR_RequestRepeatedly)
            {
                Log.Error("重复请求");
            }
            else if (code == ErrorCode.ERR_LoginInfoError)
            {
                Log.Error("登录信息错误");
            }
            else if (code == ErrorCode.ERR_NetWorkError)
            {
                Log.Error("网络错误");
            }
        }
    }
}