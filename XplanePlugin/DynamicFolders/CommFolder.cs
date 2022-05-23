using System;
using System.Collections.Generic;
using System.Linq;


using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class CommFolder : TemplateFolder
    {
        public CommFolder()
        {
            this.DisplayName = "Com";
            this.GroupName = "X-Plane";
            this.Navigation = PluginDynamicFolderNavigation.ButtonArea;
        }

        private string image { get; } = "headphones.png";

        public override BitmapImage GetButtonImage(PluginImageSize imageSize)
        {
            return SupportClasses.ButtonImages.standardImage(imageSize, this.DisplayName, this.image, 25, 10, 30, 10, 50, 60, 10);
        }

        public override Boolean Load()
        {
            this.FillAdjustments();
            this.FillButtons();

            this.connector.Start();
            foreach (KeyValuePair<string, Loupedeck.XplanePlugin.TypeClasses.Adjustment> pair in this._adjustments)
            {
                this.SubscribeFreq(pair.Key, pair.Value.element);
            }

            return base.Load();
        }



        private void SubscribeFreq(String name, DataRefElement element)
        {
            this.connector.Subscribe(element, 5, (e, v) =>
            {
                this._adjustments[name].freq = v;
                this.AdjustmentValueChanged(name);
            });
        }

        protected override void FillAdjustments()
        {
            Loupedeck.XplanePlugin.TypeClasses.Adjustment temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            if (!this._adjustments.ContainsKey("Com1"))
            {
                temp.id = "Com1";
                temp.showPicture = false;
                temp.element = DataRefs.CockpitRadiosCom1StdbyFreqHz;
                temp.command = Commands.RadiosCom1StandyFlip;
                temp.format = "n2";
                temp.unit = "Mhz";
                temp.divider = 100;
                this._adjustments.Add(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            if (!this._adjustments.ContainsKey("Com2"))
            {
                temp.id = "Com2";
                temp.showPicture = false;
                temp.element = DataRefs.CockpitRadiosCom2StdbyFreqHz;
                temp.command = Commands.RadiosCom2StandyFlip;
                temp.format = "n2";
                temp.unit = "Mhz";
                temp.divider = 100;
                this._adjustments.Add(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            if (!this._adjustments.ContainsKey("Nav1"))
            {
                temp.id = "Nav1";
                temp.showPicture = false;
                temp.element = DataRefs.CockpitRadiosNav1StdbyFreqHz;
                temp.command = Commands.RadiosNav1StandyFlip;
                temp.format = "n2";
                temp.unit = "Mhz";
                temp.divider = 100;
                this._adjustments.Add(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            if (!this._adjustments.ContainsKey("Nav2"))
            {
                temp.id = "Nav2";
                temp.showPicture = false;
                temp.element = DataRefs.CockpitRadiosNav2StdbyFreqHz;
                temp.command = Commands.RadiosNav2StandyFlip;
                temp.format = "n2";
                temp.unit = "Mhz";
                temp.divider = 100;
                this._adjustments.Add(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            if (!this._adjustments.ContainsKey("ADF1"))
            {
                temp.id = "ADF1";
                temp.showPicture = false;
                temp.element = DataRefs.CockpitRadiosAdf1StdbyFreqHz;
                temp.command = Commands.RadiosAdf1StandyFlip;
                temp.format = "n2";
                temp.unit = "Mhz";
                temp.divider = 100;
                this._adjustments.Add(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            if (!this._adjustments.ContainsKey("ADF2"))
            {
                temp.id = "ADF2";
                temp.showPicture = false;
                temp.element = DataRefs.CockpitRadiosAdf2StdbyFreqHz;
                temp.command = Commands.RadiosAdf2StandyFlip;
                temp.format = "n2";
                temp.unit = "Mhz";
                temp.divider = 100;
                this._adjustments.Add(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }
            if (!this._adjustments.ContainsKey("Transponder"))
            {
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
                temp.id = "Transponder";
                temp.prio = 1;
                temp.minvalue = 0;
                temp.maxvalue = 9999;
                temp.showPicture = false;
                temp.element = DataRefs.CockpitRadiosTransponderCode;
                temp.command = Commands.TransponderTransponderOn;
                temp.format = "n0";
                temp.unit = "";
                temp.divider = 1;
                this._adjustments.Add(temp.id, temp);
                temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            }

            base.FillAdjustments();
        }

        protected override void FillButtons()
        {
            Loupedeck.XplanePlugin.TypeClasses.Button temp = new TypeClasses.Button();
            if (!this._buttons.ContainsKey("NAVopen"))
            {
                temp.id = "NAVopen";
                temp.sticky = true;
                temp.hideFromOverview = true;
                temp.command = Commands.GPSG430n1Popup;
                temp.caption = "GPS\r\nPopup";
                temp.bgcolor = BitmapColor.White;
                temp.textcolor = BitmapColor.Black;

                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();
            }

            if (!this._buttons.ContainsKey("NAV2open"))
            {
                temp.id = "NAV2open";
                temp.command = Commands.GPSG430n2Popup;
                temp.caption = "GPS2\r\nPopup";
                temp.bgcolor = BitmapColor.White;
                temp.textcolor = BitmapColor.Black;

                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();
            }

            if (!this._buttons.ContainsKey("TransmitCom1"))
            {
                temp.id = "TransmitCom1";
                temp.command = Commands.AudioPanelTransmitAudioCom1;
                temp.caption = "Com1\r\nTransmit";
                temp.bgcolor = BitmapColor.Black;
                temp.textcolor = BitmapColor.White;
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();
            }
            if (!this._buttons.ContainsKey("TransponderOn"))
            {
                temp.id = "TransponderOn";
                temp.caption = "Transponder\r\nOn";
                temp.command = Commands.TransponderTransponderOn;
                temp.bgcolor = BitmapColor.Black;
                temp.textcolor = BitmapColor.White;
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();
            }
            if (!this._buttons.ContainsKey("TransponderOff"))
            {
                temp.id = "TransponderOff";
                temp.caption = "Transponder\r\nOff";
                temp.command = Commands.TransponderTransponderOff;
                temp.bgcolor = BitmapColor.Black;
                temp.textcolor = BitmapColor.White;
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();
            }
            if (!this._buttons.ContainsKey("TransponderStby"))
            {
                temp.id = "TransponderStby";
                temp.caption = "Transponder\r\nStby";
                temp.command = Commands.TransponderTransponderStandby;
                temp.bgcolor = BitmapColor.Black;
                temp.textcolor = BitmapColor.White;
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();
            }
            if (!this._buttons.ContainsKey("TransponderIdent"))
            {
                temp.id = "TransponderIdent";
                temp.caption = "Transponder\r\nIdent";
                temp.command = Commands.TransponderTransponderIdent;
                temp.bgcolor = BitmapColor.Black;
                temp.textcolor = BitmapColor.White;
                this._buttons.Add(temp.id, temp);
                temp = new TypeClasses.Button();
            }
            base.FillButtons();

        }
        }
    }
