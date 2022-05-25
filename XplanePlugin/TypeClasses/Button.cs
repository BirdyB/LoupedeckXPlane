using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Loupedeck.XplanePlugin.TypeClasses
{
    public class Button
    {
        public enum btnmode
        {
            normal,
            display
        }

        public enum status
        {
            green,
            yellow,
            red,
            unknown
        }

        public Button()
        {
        }
        public string id { get; set; }
        public btnmode mode { get; set; } = btnmode.normal;
        public int prio { get; set; } = 99;
        public XPlaneConnector.XPlaneCommand command { get; set; }
        public string caption { get; set; }
        public BitmapColor bgcolor { get; set; } = BitmapColor.White;
        public BitmapColor textcolor { get; set; } = BitmapColor.Black;
        public string image { get; set; } = "";
        public Action<XPlaneConnector.XPlaneConnector, Button> RunCommand = (connector, btn) => {
            connector.SendCommand(btn.command);
        };
        public Func<PluginImageSize, Button, BitmapImage> GetImage = (e, Button) => {
            switch (Button.mode)
            {
                case btnmode.normal:
                    return SupportClasses.ButtonImages.standardImage(e, Button.caption, Button.image);
                    break;
                case btnmode.display:
                    string output = Button.caption +"\r\n"+ Button.getDisplayValue();
                    return SupportClasses.ButtonImages.standardImage(e, output, Button.image);
                    break;
                default:
                    return SupportClasses.ButtonImages.standardImage(e, Button.caption, Button.image);
                    break;

            }


        };

        public int loop { get; set; } = 0;
        public bool sticky { get; set; } = false;
        public bool hideFromOverview { get; set; } = false;
        public int bincode=0;
        public float value { get; set; }
        public string unit { get; set; } = "";
        public string format { get; set; } = "";
        public float divider { get; set; } = 1;
        public int round { get; set; } = -1;
        public string getDisplayValue()
        {
            string output ="";
            float outvalue = this.value / this.divider;
            Debug.WriteLine($"{outvalue} \t {this.value} \t {this.divider} \t {outvalue.ToString(this.format)}");
            if (this.round != -1)
            {
                outvalue = ((float)Math.Round(outvalue, this.round));
            }
            output = outvalue.ToString(this.format) + " " + this.unit;
            return output;
        }


        // Properties for Display-Mode
        public float green_lo { get; set; }
        public float green_hi { get; set; }
        public float yellow_lo { get; set; }
        public float yellow_hi { get; set; }
        public float red_lo { get; set; }
        public float red_hi { get; set; }
        public float dis_min_value { get; set; }
        public float dis_max_value { get; set; }

        public XPlaneConnector.DataRefElement dataRef { get; set; }

        public status GetStatus()
        {
            if (value >= green_lo && value <= green_hi)
            {
                return status.green;
            }

            if (value < green_lo && value >= yellow_lo)
            {
                return status.yellow;
            }
            if (value < yellow_lo)
            {
                return status.red;
            }
            if (value > green_hi && value <= yellow_hi)
            {
                return status.yellow;
            }
            if (value > yellow_hi)
            {
                return status.red;
            }
            return status.unknown;

        }
    }
}
