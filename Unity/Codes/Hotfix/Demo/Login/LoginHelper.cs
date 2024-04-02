using System;

namespace ET
{
    [FriendClass(typeof (AccountInfoComponent))]
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {
            A2C_LgoinAccount a2CLgoinAccount = null;
            Session accountSession = null;

            try
            {
                accountSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                password = MD5Helper.StringMD5(password);
                a2CLgoinAccount = (A2C_LgoinAccount)await accountSession.Call(new C2A_LoginAccount() { AccountName = account, PassWord = password });
            }
            catch (Exception e)
            {
                accountSession?.Dispose();
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2CLgoinAccount.Error != ErrorCode.ERR_Success)
            {
                accountSession?.Dispose();
                return a2CLgoinAccount.Error;
            }

            zoneScene.AddComponent<SessionComponent>().Session = accountSession;
            zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();
            zoneScene.GetComponent<AccountInfoComponent>().Token = a2CLgoinAccount.Token;
            zoneScene.GetComponent<AccountInfoComponent>().AccountId = a2CLgoinAccount.AccountId;

            return ErrorCode.ERR_Success;
        }
    }
}