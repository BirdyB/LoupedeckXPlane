using System;

using XPlaneConnector;
using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.DynamicFolders
{
    public class gps430n1 : TemplateFolder
    {
        private bool _fine = true;

        public gps430n1()
        {
            this.DisplayName = "GPS430 - 1";
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
            temp.command = Commands.GPSG430n1Popup;
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
            this._adjustments.TryAdd(temp.id, temp);
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
            this._adjustments.TryAdd(temp.id, temp);
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
            this._adjustments.TryAdd(temp.id, temp);
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
            this._adjustments.TryAdd(temp.id, temp);
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
            this._adjustments.TryAdd(temp.id, temp);
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
            this._adjustments.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Adjustment();
        }
        protected override void FillButtons() {
            Loupedeck.XplanePlugin.TypeClasses.Button temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
            BitmapColor ColorBgNav2 = new BitmapColor(115, 253, 255);
            temp.id = "CDI";
            temp.caption = "CDI";
            temp.prio = 2;
            temp.command = Commands.GPSG430n1Cdi;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "OBS";
            temp.caption = "OBS";
            temp.command = Commands.GPSG430n1Obs;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "MSG";
            temp.caption = "MSG";
            temp.command = Commands.GPSG430n1Msg;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "FPL";
            temp.caption = "FPL";
            temp.command = Commands.GPSG430n1Fpl;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "VNAV";
            temp.caption = "VNAV";
            temp.command = Commands.GPSG430n1Vnav;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "PROC";
            temp.caption = "PROC";
            temp.command = Commands.GPSG430n1Proc;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "CLR";
            temp.caption = "CLR";
            temp.command = Commands.GPSG430n1Clr;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "ENT";
            temp.caption = "ENT";
            temp.command = Commands.GPSG430n1Ent;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "COMSwitch";
            temp.caption = "Com Act/Stby";
            temp.command = Commands.GPSG430n1ComFf;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "NAVSwitch";
            temp.caption = "Nav Act/Stby";
            temp.command = Commands.GPSG430n1NavFf;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "MenuBtn";
            temp.caption = "Menu";
            temp.command = Commands.GPSG430n1Menu;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Direct";
            temp.caption = "Direct";
            temp.command = Commands.GPSG430n1Direct;
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();

            temp.id = "Popup";
            temp.caption = "Popup";
            temp.command = Commands.GPSG430n1Popup;
            this._buttons.TryAdd(temp.id, temp);
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
            this._buttons.TryAdd(temp.id, temp);
            temp = new Loupedeck.XplanePlugin.TypeClasses.Button();
            base.FillButtons();
        } 
    }
}
