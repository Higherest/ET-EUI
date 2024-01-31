namespace ET
{
    public static class DiconnectHelper
    {
        public static async ETTask DisConnect(this Session self)
        {
            if (self == null || self.IsDisposed) return;

            long instanceId = self.InstanceId;
            await TimerComponent.Instance.WaitAsync(1000);

            if (self.InstanceId != instanceId) return;

            self.Dispose();
        }
    }
}