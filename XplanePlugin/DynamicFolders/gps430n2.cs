using System;
    using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class gps430n2 : TemplateFolder
    {
        public gps430n2()
        {
            this.DisplayName = "GPS430 - 2";
            this.GroupName = "GPS";
            //this.Navigation = PluginDynamicFolderNavigation.ButtonArea;
            this.Navigation = PluginDynamicFolderNavigation.None;
        }

        private string image { get; } = "pin.png";

        public override BitmapImage GetButtonImage(PluginImageSize imageSize)
        {
            return SupportClasses.ButtonImages.standardImage(imageSize, this.DisplayName, this.image, 25, 10, 30, 10, 50, 60, 10);
        }

        private bool _fine = true;


        protected override void FillSubscriptions() => base.FillSubscriptions();
        protected override void FillAdjustments()
        {
            Loupedeck.XplanePlugin.TypeClasses.Adjustment temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            temp.id = "Zoom";
            temp.prio = 1;
            temp.caption = "Zoom";
            temp.command = Commands.GPSG430n2Popup;
            temp.showPicture = false;
            temp.getValue = (adj) =>
            {
                return "";
            };
            temp.apply = (conn, diff, multiplier, adj) =>
            {
                if (diff > 0)
                {
                    for (int i = 0; i < (diff * multiplier); i++)
                    {
                        conn.SendCommand(Commands.GPSG430n2ZoomIn);
                    }
                }
                else
                {
                    diff = diff * -1;
                    for (int i = 0; i < (diff * multiplier); i++)
                    {
                        conn.SendCommand(Commands.GPSG430n2ZoomOut);
                    }

                }
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();

            temp.id = "Nav2KnobL";
            temp.prio = 9;
            temp.caption = "Nav2\r\nPUSH\r\nC/V";
            temp.command = Commands.GPSG430n2NavComTog;
            temp.showPicture = true;
            temp.getValue = (adj) =>
            {
                return "";
            };
            temp.apply = (conn, diff, multiplier, adj) =>
            {
                if (this._fine)
                {

                    if (diff > 0)
                    {
                        for (int i = 0; i < (diff * multiplier); i++)
                        {
                            conn.SendCommand(Commands.GPSG430n2FineUp);
                        }
                    }
                    else
                    {
                        diff = diff * -1;
                        for (int i = 0; i < (diff * multiplier); i++)
                        {
                            conn.SendCommand(Commands.GPSG430n2FineDown);
                        }

                    }
                }
                else
                {
                    if (diff > 0)
                    {
                        for (int i = 0; i < (diff * multiplier); i++)
                        {
                            conn.SendCommand(Commands.GPSG430n2CoarseUp);
                        }
                    }
                    else
                    {
                        diff = diff * -1;
                        for (int i = 0; i < (diff * multiplier); i++)
                        {
                            conn.SendCommand(Commands.GPSG430n2CoarseDown);
                        }

                    }
                }
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();

            temp.id = "Nav2KnobR";
            temp.caption = "Nav2\r\nPUSH\r\nCRSR";
            temp.prio = 12;
            temp.command = Commands.GPSG430n2Cursor;
            temp.showPicture = true;
            temp.getValue = (adj) =>
            {
                return "";
            };
            temp.apply = (conn, diff, multiplier, adj) =>
            {
                if (diff > 0)
                {
                    for (int i = 0; i < (diff * multiplier); i++)
                    {
                        if (this._fine)
                        {
                            conn.SendCommand(Commands.GPSG430n2PageUp);
                        }
                        else
                        {
                            conn.SendCommand(Commands.GPSG430n2ChapterUp);
                        }
                    }
                }
                else
                {
                    diff = diff * -1;
                    for (int i = 0; i < (diff * multiplier); i++)
                    {
                        if (this._fine)
                        {
                            conn.SendCommand(Commands.GPSG430n2PageDn);
                        }
                        else
                        {
                            conn.SendCommand(Commands.GPSG430n2ChapterDn);
                        }
                    }

                }
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();

            temp.id = "Nav2ComVol";
            temp.prio = 8;
            temp.caption = "Nav2\r\nCom Vol.";
            temp.command = Commands.GPSG430n2Cvol;
            temp.showPicture = true;
            temp.getValue = (adj) =>
            {
                return "";
            };
            temp.apply = (conn, diff, multiplier, adj) =>
            {
                if (diff > 0)
                {
                    for (int i = 0; i < (diff * multiplier); i++)
                    {
                        conn.SendCommand(Commands.GPSG430n2CvolUp);
                    }
                }
                else
                {
                    diff = diff * -1;
                    for (int i = 0; i < (diff * multiplier); i++)
                    {
                        conn.SendCommand(Commands.GPSG430n2CvolDn);
                    }

                }
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();

            temp.id = "Nav2NavVol";
            temp.prio = 11;
            temp.caption = "Nav2\r\nNav Vol.";
            temp.command = Commands.GPSG430n2Vvol;
            temp.showPicture = true;
            temp.getValue = (adj) =>
            {
                return "";
            };
            temp.apply = (conn, diff, multiplier, adj) =>
            {
                if (diff > 0)
                {
                    for (int i = 0; i < (diff * multiplier); i++)
                    {
                        conn.SendCommand(Commands.GPSG430n2VvolUp);
                    }
                }
                else
                {
                    diff = diff * -1;
                    for (int i = 0; i < (diff * multiplier); i++)
                    {
                        conn.SendCommand(Commands.GPSG430n2VvolDn);
                    }

                }
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();

            temp = getEmptyAdjustment("Nav2Spacer");
            temp.prio = 10;
            temp.showPicture = true;
            temp.bgcolor = BitmapColor.Black;
            temp.textcolor = BitmapColor.Black;
            temp.getValue = (adj) =>
            {
                return "";
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
        }
        protected override void FillButtons()
        {
            Loupedeck.XplanePlugin.TypeClasses.Button temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
            temp.id = "Fine";
            temp.caption = "Fine";
            temp.prio = 1;
            temp.sticky = true;
            temp.hideFromOverview = true;
            temp.RunCommand = (conn, comm) =>
            {
                this._fine = !this._fine;
                this.CommandImageChanged(temp.id);
            };
            temp.GetImage = (size, but) => {
                var builder = new BitmapBuilder(size);
                var clrFine = new BitmapColor(142, 250, 0);
                var clrCoarse = new BitmapColor(255, 147, 0);

                if (this._fine)
                {
                    builder.Clear(clrFine);
                    builder.DrawText("Fine / \r\nPage", BitmapColor.Black);
                }
                else
                {
                    builder.Clear(clrCoarse);
                    builder.DrawText("Coarse / \r\nChapter", BitmapColor.Black);
                }
                return builder.ToImage();
            };
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
            BitmapColor ColorBgNav2 = new BitmapColor(115, 253, 255);

            temp.id = "Nav2CDI";
            temp.caption = "Nav2\r\nCDI";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2Cdi;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2OBS";
            temp.caption = "Nav2\r\nOBS";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2Obs;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2MSG";
            temp.caption = "Nav2\r\nMSG";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2Msg;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2FPL";
            temp.caption = "Nav2\r\nFPL";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2Fpl;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2VNAV";
            temp.caption = "Nav2\r\nVNAV";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2Vnav;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2PROC";
            temp.caption = "Nav2\r\nPROC";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2Proc;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2CLR";
            temp.caption = "Nav2\r\nCLR";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2Clr;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2ENT";
            temp.caption = "Nav2\r\nENT";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2Ent;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2COMSwitch";
            temp.caption = "Nav2\r\nCom Act/Stby";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2ComFf;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2NAVSwitch";
            temp.caption = "Nav2\r\nNav Act/Stby";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2NavFf;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2MenuBtn";
            temp.caption = "Nav2\r\nMenu";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2Menu;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2Direct";
            temp.caption = "Nav2\r\nDirect";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2Direct;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Nav2Popup";
            temp.caption = "Nav2\r\nPopup";
            temp.bgcolor = ColorBgNav2;
            temp.command = Commands.GPSG430n2Popup;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            base.FillButtons();
        }
    }
    }
