namespace Loupedeck.XplanePlugin
{
    using System;
    using System.Diagnostics;

    public class XplaneApplication : ClientApplication
    {
        public XplaneApplication()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            {
                Debug.WriteLine(eventArgs.Exception.ToString());
            };
        }
 

        protected override String GetProcessName() => "X-Plane";

        protected override String GetBundleName() => "";

}
}