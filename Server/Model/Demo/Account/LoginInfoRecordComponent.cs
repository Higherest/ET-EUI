using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof (Scene))]
    public class LoginInfoRecordComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<long, int> AccountLoginInfoDict = new Dictionary<long, int>();
    }
}