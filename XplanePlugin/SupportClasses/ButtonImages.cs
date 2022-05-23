using System;
using System.Diagnostics;

namespace Loupedeck.XplanePlugin.SupportClasses
{
    public static class ButtonImages
    {
        //Color Definitions
        public static BitmapColor Red { get { return new BitmapColor(255, 0, 0); } }
        public static BitmapColor Green { get { return new BitmapColor(0, 255, 0); } }
        public static BitmapColor Blue { get { return new BitmapColor(0, 0, 255); } }
        public static BitmapColor LightBlue { get { return new BitmapColor(115, 253, 255); } }
        public static BitmapColor Yellow { get { return new BitmapColor(115, 253, 255); } }
        public static BitmapColor Black { get { return new BitmapColor(0, 0, 0); } }
        public static BitmapColor White { get { return new BitmapColor(255, 255, 255); } }

        //Settings

        public static BitmapColor globalBgColor { get; set; } = ButtonImages.Black;
        public static BitmapColor globalIconColor { get; set; } = ButtonImages.White;
        public static BitmapColor globalTextColor { get; set; } = ButtonImages.Black;


        //Image Definitions with (size, text, image)
        public static BitmapImage buttonImageText(PluginImageSize imageSize, string text, string image, BitmapColor iconColor, BitmapColor textColor) {

            return _standardImage(imageSize, iconColor, textColor, text, image, 25, 10, 30, 10, 50, 60, 10);
        }

        public static BitmapImage activeImage(PluginImageSize imageSize, string text, string image)
        {

            return _standardImage(imageSize, ButtonImages.Green, ButtonImages.White, text, image);
        }

        public static BitmapImage activeImage(PluginImageSize imageSize, string text, string image, int picx, int picy, int picsize, int textx, int texty, int textWidth, int textHight)
        {

            return _standardImage(imageSize, ButtonImages.Green, ButtonImages.White, text, image, picx, picy, picsize, textx, texty, textWidth, textHight);
        }

        public static BitmapImage customImage(PluginImageSize imageSize, string text, string image, BitmapColor iconColor, BitmapColor textColor)
        {

            return _standardImage(imageSize, iconColor, textColor, text, image);
        }

        public static BitmapImage customImage(PluginImageSize imageSize, string text, string image, BitmapColor iconColor, BitmapColor textColor, int picx, int picy, int picsize, int textx, int texty, int textWidth, int textHight)
        {

            return _standardImage(imageSize, iconColor, textColor, text, image, picx, picy, picsize, textx, texty, textWidth, textHight);
        }

        public static BitmapImage standardImage(PluginImageSize imageSize, string text, string image)
        {

            return _standardImage(imageSize, ButtonImages.globalIconColor, ButtonImages.globalTextColor, text, image);
        }

        public static BitmapImage standardImage(PluginImageSize imageSize, string text, string image, int picx, int picy, int picsize, int textx, int texty, int textWidth, int textHight)
        {

            return _standardImage(imageSize, ButtonImages.globalIconColor, ButtonImages.globalTextColor, text, image, picx, picy, picsize, textx, texty, textWidth, textHight);
        }


        public static BitmapImage _standardImage(PluginImageSize imageSize, BitmapColor iconColor, BitmapColor textColor, string text = "", string image = "")
        {
            var builder = new BitmapBuilder(imageSize);
            ;
            builder.Clear(ButtonImages.globalBgColor);
            ButtonImages.RoundEdge(ref builder, 5, 5, 70, 70, 20, iconColor);
            if (image != "")
            {
                BitmapImage imageBitmap = EmbeddedResources.ReadImage(EmbeddedResources.FindFile(image));
                imageBitmap.Resize(60, 60);
                builder.DrawImage(imageBitmap, 10, 10);
            }

            builder.DrawText(text, 10, 10, 60, 60, textColor);

            return builder.ToImage();
        }
        public static BitmapImage _standardImageGraph(PluginImageSize imageSize, BitmapColor iconColor, BitmapColor textColor, string text, BitmapImage image)
        {
            var builder = new BitmapBuilder(imageSize);
            ;
            builder.Clear(ButtonImages.globalBgColor);
            ButtonImages.RoundEdge(ref builder, 5, 5, 70, 70, 20, iconColor);

            builder.DrawImage(image, 10, 0);
            builder.DrawText(text, 10, 40, 60, 20, textColor);


            return builder.ToImage();
        }

        public static BitmapImage _standardImage(PluginImageSize imageSize, BitmapColor iconColor, BitmapColor textColor, string text, string image, int picx, int picy, int picsize, int textx, int texty, int textWidth, int textHight)
        {
            var builder = new BitmapBuilder(imageSize);
            ;
            builder.Clear(ButtonImages.globalBgColor);
            ButtonImages.RoundEdge(ref builder, 5, 5, 70, 70, 20, iconColor);
            if (image != "")
            {
                BitmapImage imageBitmap = EmbeddedResources.ReadImage(EmbeddedResources.FindFile(image));
                imageBitmap.Resize(picsize, picsize);
                builder.DrawImage(imageBitmap, picx, picy);
            }

            builder.DrawText(text, textx, texty, textWidth, textHight, textColor);

            return builder.ToImage();
        }

        public static BitmapImage _standardImageImg(PluginImageSize imageSize, BitmapColor iconColor, BitmapColor textColor, string text, BitmapImage image, int picx, int picy, int picsize, int textx, int texty, int textWidth, int textHight)
        {
            var builder = new BitmapBuilder(imageSize);
            ;
            builder.Clear(ButtonImages.globalBgColor);
            ButtonImages.RoundEdge(ref builder, 5, 5, 70, 70, 20, iconColor);
            builder.DrawImage(image, picx, picy);
            builder.DrawText(text, textx, texty, textWidth, textHight, textColor);

            return builder.ToImage();
        }

        //Adjustment Images

        public static BitmapImage standardAdjustmentImage(PluginImageSize imageSize, string text, string image)
        {
            return ButtonImages._standardAdjustmentImage(imageSize, text, image);
        }

        public static BitmapImage _standardAdjustmentImage(PluginImageSize imageSize, string text, string image)
        {
            var builder = new BitmapBuilder(imageSize);
            builder.Clear(BitmapColor.Black);
            builder.DrawText(text, BitmapColor.White,12,10,2);
            return builder.ToImage();
        }


        //Drawing Helper Functions
        private static void RoundEdge(ref BitmapBuilder builder, int x, int y, int width, int height, int radius, BitmapColor color)
        {
            if ((width + x) > builder.Width)
            {
                width = builder.Width - x;
            }
            if ((height + y) > builder.Height)
            {
                height = builder.Height - y;
            }
            if (width > height)
            {
                if (radius > (width / 2))
                {
                    radius = width / 2;
                }
            }
            else
            {
                if (radius > (height / 2))
                {
                    radius = height / 2;
                }
            }

            ButtonImages.drawFilledRectangle(ref builder, (radius + x), y, width - (radius * 2), height, color);
            ButtonImages.drawFilledRectangle(ref builder, x, (radius + y), width, height - (radius * 2), color);
            ButtonImages.drawFilledCircle(ref builder, (radius + x), (radius + y), radius, color);
            ButtonImages.drawFilledCircle(ref builder, (width + x - radius), (radius + y), radius, color);
            ButtonImages.drawFilledCircle(ref builder, (radius + x), (height + y - radius), radius, color);
            ButtonImages.drawFilledCircle(ref builder, (width + x - radius), (height + y - radius), radius, color);
        }

        private static void drawFilledCircle(ref BitmapBuilder builder, int a, int b, int c, BitmapColor color)
        {
            for (float i = c; i > 0; i = i - (float)0.2)
            {
                builder.DrawCircle(a, b, i, color);
            }
        }

        private static void drawFilledRectangle(ref BitmapBuilder builder, int x, int y, int width, int height, BitmapColor color)
        {
            if (width > height)
            {
                while (x <= (width + x))
                {
                    builder.DrawRectangle(x, y, width, height, color);
                    x++;
                    y++;
                    width = width - 2;
                    height = height - 2;
                }
            }
            else
            {
                while (y <= (height + y))
                {
                    builder.DrawRectangle(x, y, width, height, color);
                    x++;
                    y++;
                    width = width - 2;
                    height = height - 2;
                }
            }
        }

    }
}
