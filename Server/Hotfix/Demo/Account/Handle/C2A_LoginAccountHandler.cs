using System;
using System.Text.RegularExpressions;

namespace ET
{
    [FriendClass(typeof (Account))]
    public class C2A_LoginAccountHandler: AMRpcHandler<C2A_LoginAccount, A2C_LgoinAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LgoinAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误，当前Scene为：{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.PassWord))
            {
                response.Error = ErrorCode.ERR_LoginInfoError;
                reply();
                session.Dispose();
                return;
            }

            if (!Regex.IsMatch(request.AccountName.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_LoginInfoError;
                reply();
                session.Dispose();
                return;
            }

            if (!Regex.IsMatch(request.PassWord.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_LoginInfoError;
                reply();
                session.Dispose();
                return;
            }

            var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                    .Query<Account>(d => d.accountName.Equals(request.AccountName.Trim()));
            Account account = null;

            if (accountInfoList.Count > 0)
            {
                account = accountInfoList[0];
                session.AddChild(account);
                if (account.accountType == (int)AccountType.BlackList)
                {
                    response.Error = ErrorCode.ERR_AccountBlackList;
                    reply();
                    session.Dispose();
                    return;
                }

                if (!account.passWord.Equals(request.PassWord))
                {
                    request.Error = ErrorCode.ERR_PassWordError;
                    reply();
                    session.Dispose();
                    return;
                }
            }
            else
            {
                account = session.AddChild<Account>();
                account.accountName = request.AccountName.Trim();
                account.passWord = request.PassWord;
                account.createTime = TimeHelper.ServerNow();
                account.accountType = (int)AccountType.General;
                await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<Account>(account);
            }

            string token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString();
            session.DomainScene().GetComponent<TokenComponent>().Remove(account.Id);
            session.DomainScene().GetComponent<TokenComponent>().Add(account.Id, token);

            response.AccountId = account.Id;
            response.Token = token;

            reply();
        }
    }
}