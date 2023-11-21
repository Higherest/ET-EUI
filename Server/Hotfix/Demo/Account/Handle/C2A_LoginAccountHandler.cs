using System;

namespace ET
{
    public class C2A_LoginAccountHandler: AMRpcHandler<C2A_LoginAccount, A2C_LgoinAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LgoinAccount response, Action reply)
        {
            
            
            await ETTask.CompletedTask;
        }
    }
}