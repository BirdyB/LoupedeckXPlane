using System;

using System.Diagnostics;
using System.Threading.Tasks;

using XPlaneConnector;
using XPlaneConnector.DataRefs;

namespace Loupedeck.XplanePlugin.TypeClasses
{
    public class SubscriptionValue
    {
        public SubscriptionValue()
        {
        }

        public XPlaneConnector.DataRefElement dataRef { get; set; }
        public float value { get; set; }
        public string displayName { get; set; }
        public bool forceUpdate { get; set; }


        public int getIndex()
        {
            string STR = this.dataRef.DataRef;
            string FinalString = "-1";
            try
            {
                int Pos1 = STR.IndexOf("[") + 1;
                int Pos2 = STR.IndexOf("]");
                FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            }
            catch ( Exception e)
            {
                FinalString = "-1";
            }
            return Int32.Parse(FinalString);
        }
    }
}
