using System;

using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class gps430 : TemplateFolder
    {
        private bool _fine = true;

        public gps430()
        {
            this.DisplayName = "GPS430";
            this.GroupName = "GPS";
            //this.Navigation = PluginDynamicFolderNavigation.ButtonArea;
            this.Navigation = PluginDynamicFolderNavigation.None;
        }

        private string image { get; } = "pin.png";

        public override BitmapImage GetButtonImage(PluginImageSize imageSize)
        {
            return SupportClasses.ButtonImages.standardImage(imageSize, this.DisplayName, this.image, 25, 10, 30, 10, 50, 60, 10);
        }

        protected override void FillSubscriptions() => base.FillSubscriptions();
        protected override void FillAdjustments() {
            Loupedeck.XplanePlugin.TypeClasses.Adjustment temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            temp.id = "Zoom";
            temp.prio = 1;
            temp.caption = "Zoom";
            temp.showPicture = false;
            temp.getValue = (adj) =>
            {
                return "";
            };
            temp.apply = (conn, diff, multiplier, adj) =>
            {
                if (diff > 0)
                {
                    for (int i = 0; i<(diff*multiplier);i++ )
                    {
                        conn.SendCommand(Commands.GPSG430n1ZoomIn);
                    }
                } else
                {
                    diff = diff * -1;
                    for (int i = 0; i < (diff * multiplier); i++)
                    {
                        conn.SendCommand(Commands.GPSG430n1ZoomOut);
                    }

                }
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();

            temp.id = "KnobL";
            temp.prio = 3;
            temp.caption = "PUSH\r\nC/V";
            temp.command = Commands.GPSG430n1NavComTog;
            temp.showPicture = true;
            temp.getValue = (adj) =>
            {
                return "";
            };
            temp.apply = (conn, diff, multiplier, adj) =>
            {
               if(this._fine){
                    
                    if (diff > 0)
                    {
                        for (int i = 0; i < (diff * multiplier); i++)
                        {
                            conn.SendCommand(Commands.GPSG430n1FineUp);
                        }
                    }
                    else
                    {
                        diff = diff * -1;
                        for (int i = 0; i < (diff * multiplier); i++)
                        {
                            conn.SendCommand(Commands.GPSG430n1FineDown);
                        }

                    }
                } else
                {
                    if (diff > 0)
                    {
                        for (int i = 0; i < (diff * multiplier); i++)
                        {
                            conn.SendCommand(Commands.GPSG430n1CoarseUp);
                        }
                    }
                    else
                    {
                        diff = diff * -1;
                        for (int i = 0; i < (diff * multiplier); i++)
                        {
                            conn.SendCommand(Commands.GPSG430n1CoarseDown);
                        }

                    }
                }
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();

            temp.id = "KnobR";
            temp.caption = "PUSH\r\nCRSR";
            temp.prio = 6;
            temp.command = Commands.GPSG430n1Cursor;
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
                        { conn.SendCommand(Commands.GPSG430n1PageUp);
                        }
                        else
                        {
                            conn.SendCommand(Commands.GPSG430n1ChapterUp);
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
                            conn.SendCommand(Commands.GPSG430n1PageDn);
                        }
                        else
                        {
                            conn.SendCommand(Commands.GPSG430n1ChapterDn);
                        }
                    }

                }
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();

            temp.id = "ComVol";
            temp.prio = 2;
            temp.caption = "Com Vol.";
            temp.command = Commands.GPSG430n1Cvol;
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
                        conn.SendCommand(Commands.GPSG430n1CvolUp);
                    }
                }
                else
                {
                    diff = diff * -1;
                    for (int i = 0; i < (diff * multiplier); i++)
                    {
                        conn.SendCommand(Commands.GPSG430n1CvolDn);
                    }

                }
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();

            temp.id = "NavVol";
            temp.prio = 5;
            temp.caption = "Nav Vol.";
            temp.command = Commands.GPSG430n1Vvol;
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
                        conn.SendCommand(Commands.GPSG430n1VvolUp);
                    }
                }
                else
                {
                    diff = diff * -1;
                    for (int i = 0; i < (diff * multiplier); i++)
                    {
                        conn.SendCommand(Commands.GPSG430n1VvolDn);
                    }

                }
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();

            temp = getEmptyAdjustment("Spacer");
            temp.prio = 4;
            temp.showPicture = true;
            temp.bgcolor = BitmapColor.Black;
            temp.textcolor = BitmapColor.Black;
            temp.getValue = (adj) =>
            {
                return "";
            };
            this._adjustments.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();

           temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
            temp.id = "Nav2Zoom";
            temp.prio = 7;
            temp.caption = "Nav2\r\nZoom";
            temp.showPicture = true;
            temp.bgcolor = BitmapColor.Black;
            temp.textcolor = BitmapColor.White;

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
        protected override void FillButtons() {
            Loupedeck.XplanePlugin.TypeClasses.Button temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
            BitmapColor ColorBgNav2 = new BitmapColor(115, 253, 255);
            temp.id = "CDI";
            temp.caption = "CDI";
            temp.prio = 2;
            temp.command = Commands.GPSG430n1Cdi;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "OBS";
            temp.caption = "OBS";
            temp.command = Commands.GPSG430n1Obs;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "MSG";
            temp.caption = "MSG";
            temp.command = Commands.GPSG430n1Msg;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "FPL";
            temp.caption = "FPL";
            temp.command = Commands.GPSG430n1Fpl;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "VNAV";
            temp.caption = "VNAV";
            temp.command = Commands.GPSG430n1Vnav;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "PROC";
            temp.caption = "PROC";
            temp.command = Commands.GPSG430n1Proc;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "CLR";
            temp.caption = "CLR";
            temp.command = Commands.GPSG430n1Clr;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "ENT";
            temp.caption = "ENT";
            temp.command = Commands.GPSG430n1Ent;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "COMSwitch";
            temp.caption = "Com Act/Stby";
            temp.command = Commands.GPSG430n1ComFf;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "NAVSwitch";
            temp.caption = "Nav Act/Stby";
            temp.command = Commands.GPSG430n1NavFf;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "MenuBtn";
            temp.caption = "Menu";
            temp.command = Commands.GPSG430n1Menu;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Direct";
            temp.caption = "Direct";
            temp.command = Commands.GPSG430n1Direct;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Popup";
            temp.caption = "Popup";
            temp.command = Commands.GPSG430n1Popup;
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

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
                var clrFine = new BitmapColor(142,250,0);
                var clrCoarse = new BitmapColor(255,147,0);

                if (this._fine)
                {
                    builder.Clear(clrFine);
                    builder.DrawText("Fine / \r\nPage",BitmapColor.Black);
                } else
                {
                    builder.Clear(clrCoarse);
                    builder.DrawText("Coarse / \r\nChapter", BitmapColor.Black);
                }
                return builder.ToImage();
            };
            this._buttons.Add(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

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
