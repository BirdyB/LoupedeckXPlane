using System;
using System.Diagnostics;
using XPlaneConnector;

using XPlaneConnector.DataRefs;
namespace Loupedeck.XplanePlugin.TypeClasses
{
    public class Graph
    {
        //Set colors, etc.
        public BitmapColor bgColor = BitmapColor.Transparent;
        public BitmapColor lineColor = new BitmapColor(0, 0, 0);
        public BitmapColor arcGreenColor = new BitmapColor(0, 255, 0);
        public BitmapColor arcYellowColor = new BitmapColor(255, 255, 0);
        public BitmapColor arcRedColor = new BitmapColor(255, 0, 0);
        public BitmapColor arcBlueColor = new BitmapColor(0, 0, 255);

        public int strokeArc = 8;
        public int strokeLine = 2;

        public int rArc = 25;
        public int rLine = 28;

        public int width = 60;
        public int height = 40;

        public int x0 =30;
        public int y0 =40;


        //set angles
        public int angle_min = 180;
        public int angle_max = 360;

        //set boundaries
        public float red_lo = 10;
        public float yellow_lo = 20;
        public float green_lo = 30;
        public float green_hi = 40;
        public float yellow_hi = 50;
        public float red_hi = 60;

        //current value
        public float value;

        //bitmap builder
        public BitmapBuilder builder = new BitmapBuilder(60, 40);
        public BitmapBuilder scaleBuilder = new BitmapBuilder(60, 40);

        private BitmapImage scale;



        public Graph(float red_lo, float yellow_lo,float green_lo, float green_hi, float yellow_hi, float red_hi)
        {
            this.red_lo = red_lo;
            this.yellow_lo = yellow_lo;
            this.green_lo = green_lo;
            this.green_hi = green_hi;
            this.yellow_hi = yellow_hi;
            this.red_hi = red_hi;

        }

        public Graph() {
            this.red_lo = 0;
            this.yellow_lo = 0;
            this.green_lo = 0;
            this.green_hi = 0;
            this.yellow_hi = 0;
            this.red_hi = 0;
        }

        public void init()
        {
            builder.Clear(bgColor);
            this.scale = drawArcs();
        }


        public void drawValue(float val) {
            float valRng = red_hi - red_lo;
            float degMin = 180;
            float degMax = 360;
            float degRng = degMax - degMin;
            float degPerVal = degRng / valRng;

            float deg = (((val - red_lo) * degPerVal) * -1) + 180;
            int x1 = this.getCircleX(deg, this.rLine) + x0;
            int y1 = y0 - (this.getCircleY(deg, this.rLine));
            this.builder.DrawLine(x0, y0, x1, y1, lineColor, strokeLine);

        }

        
        public BitmapImage drawArcs() {
            BitmapBuilder tempbuilder = new BitmapBuilder(60, 40);
        float valRng = red_hi - red_lo;
            float degMin = 180;
            float degMax = 360;
            float degRng = degMax - degMin;

            float curAngle = degMin;
            float angle;
            float curMax = red_lo;
            if (yellow_lo > curMax)
            {
                angle = ((yellow_lo - red_lo) / valRng) * degRng;
                curMax = yellow_lo;
            }
            else
            {
                angle = 0;
            }
            BitmapColor clr = arcRedColor;
            tempbuilder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
            curAngle += angle;

            if (green_lo > curMax)
            {
                angle = ((green_lo - curMax) / valRng) * degRng;
                curMax = green_lo;
            }
            else
            {
                angle = 0;
            }
            clr = arcYellowColor;
            tempbuilder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
            curAngle += angle;

            if (green_hi > curMax)
            {
                angle = ((green_hi - curMax) / valRng) * degRng;
                curMax = green_hi;
            }
            else
            {
                angle = 0;
            }
            clr = arcGreenColor;
            tempbuilder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
            curAngle += angle;

            if (yellow_hi > curMax)
            {
                angle = ((yellow_hi - curMax) / valRng) * degRng;
                curMax = yellow_hi;
            }
            else
            {
                angle = 0;
            }
            clr = arcYellowColor;
            tempbuilder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
            curAngle += angle;

            if (red_hi > curMax)
            {
                angle = ((red_hi - curMax) / valRng) * degRng;
                curMax = red_hi;
            }
            else
            {
                angle = 0;
            }
            clr = arcRedColor;
            tempbuilder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
            curAngle += angle;

            return tempbuilder.ToImage();
        }


        public int getCircleY(float winkel, int r)
        {
            int y;
            double temp;
            winkel = (float)DegreesToRadians(winkel);
            temp = r * Math.Sin(winkel);
            y = (int)temp;


            return y;

        }

        public int getCircleX(float winkel, int r)
        {
            int x;
            double temp;
            winkel = (float)DegreesToRadians(winkel);
            temp = r * Math.Cos(winkel);
            x = (int)temp;

            return x;

        }

        public BitmapImage getGraph(float val = 40)
        {
            //this.init();
            if (val < this.red_lo)
            { val = this.red_lo; }
            if (val > red_hi)
            { val = this.red_hi; }
            builder.Clear(bgColor);
            builder.DrawImage(scale);
            this.drawValue(val);
            BitmapImage output = this.builder.ToImage();

            return output;

        }

        public double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
