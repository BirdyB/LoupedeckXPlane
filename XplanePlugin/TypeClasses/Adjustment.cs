using System;
namespace Loupedeck.XplanePlugin.TypeClasses
{
    using XPlaneConnector;
    using XPlaneConnector.DataRefs;

    public class Adjustment
    {
        public Adjustment()
        {
        }

        public string id { get; set; }
        public float freq { get; set; }
        public int prio { get; set; } = 99;
        public DataRefElement element { get; set; }
        public XPlaneCommand command { get; set; }
        public string caption { get; set; }
        public string unit { get; set; }
        public string format { get; set; }
        public float divider { get; set; } = 1;
        public float setdivider { get; set; } = 1;
        public float minvalue { get; set; } = -1;
        public float maxvalue { get; set; } = -1;
        public bool showPicture { get; set; } = true;
        public bool showText { get; set; } = true;
        public BitmapColor bgcolor { get; set; } = BitmapColor.White;
        public BitmapColor textcolor { get; set; } = BitmapColor.Black;
        public BitmapImage image { get; set; }
        public Func<Adjustment, String> getValue = (adj) =>
        {
            float disfreq;
            String output;
            disfreq = adj.freq / adj.divider;
            if (adj.round != -1)
            {
                disfreq = ((float)Math.Round(disfreq, adj.round));
            }
            output = disfreq.ToString(adj.format) + "\r\n" + adj.unit;
            if (adj.showText)
            {
                return output;
            }
            else
            {
                return "";
            }
        };
        public Action<XPlaneConnector, Int32, float, Adjustment> apply = (connector, diff, multiplier, adj) =>
        {
            adj.freq += (float)(diff * multiplier / adj.setdivider);
            if (!(adj.minvalue == -1))
            {
                if (adj.freq <= adj.minvalue)
                {
                    adj.freq = adj.minvalue;
                }
            }
            if (!(adj.maxvalue == -1))
            {
                if (adj.freq >= adj.maxvalue)
                {
                    adj.freq = adj.maxvalue;
                }
            }
            if (adj.round != -1)
            {
                adj.freq = (float)Math.Round(adj.freq, adj.round);
            }
            connector.SetDataRefValue(adj.element, adj.freq);

            connector.Subscribe(adj.element, -1, (e, v) =>
            {
                adj.freq = v;

            });

        };
        public Func<PluginImageSize, Adjustment, BitmapImage> GetImage = (e, adj) =>
        {

            if (adj.showPicture)
            {
                float disfreq;
                String output;
                disfreq = adj.freq / adj.divider;
                if (adj.round != -1)
                {
                    disfreq = ((float)Math.Round(disfreq, adj.round));
                }
                output = disfreq.ToString(adj.format) + "\r\n" + adj.unit;
                string text = $"{adj.caption}\r\n{output}";
                return SupportClasses.ButtonImages.standardAdjustmentImage(e, text,""); //Implement ImageAction, Need to adjust everything to string.
            }
            else
            {
                return null;
            }

        };
        public int round { get; set; } = -1;


    }
}
