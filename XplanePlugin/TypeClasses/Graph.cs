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

        public float value_min = 0;
        public float value_max = 80;

        //current value
        public float value;

        //bitmap builder
        public BitmapBuilder builder = new BitmapBuilder(60, 40);



        public Graph(float value_min, float value_max, float red_lo, float yellow_lo,float green_lo, float green_hi, float yellow_hi, float red_hi)
        {
            this.value_min = value_min;
            this.value_max = value_max;
            this.red_lo = red_lo;
            this.yellow_lo = yellow_lo;
            this.green_lo = green_lo;
            this.green_hi = green_hi;
            this.yellow_hi = yellow_hi;
            this.red_hi = red_hi;
        }

        public void init()
        {
            builder.Clear(bgColor);
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
            Debug.WriteLine($" Value {val} bei max Winkel {angle_max - angle_min} und max. Wert {value_max} bei degPerVal{degPerVal} ergibt {deg}");
            Debug.WriteLine($"Koordinaten für Linie x0={x0}, y0={y0}, x1={x1}, y1={y1}");
            this.builder.DrawLine(x0, y0, x1, y1, lineColor, strokeLine);

        }

        
        public void drawArcs() {
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
            builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
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
            builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
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
            builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
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
            builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
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
            builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
            curAngle += angle;
        }


        public int getCircleY(float winkel, int r)
        {
            int y;
            double temp;
            winkel = (float)DegreesToRadians(winkel);
            temp = r * Math.Sin(winkel);
            y = (int)temp;
            Debug.WriteLine($"r={r}, winkel={winkel}, Sin(winkel)={Math.Sin(winkel)}, temp={temp}, y={y}");

            return y;

        }

        public int getCircleX(float winkel, int r)
        {
            int x;
            double temp;
            winkel = (float)DegreesToRadians(winkel);
            temp = r * Math.Cos(winkel);
            x = (int)temp;

            Debug.WriteLine($"r={r}, winkel={winkel}, Cos(winkel)={Math.Cos(winkel)}, temp={temp}, x={x}");

            return x;

        }

        public void demo()
        {
            int rline = this.rArc + 2;

            int offset = 0;

            /*
            builder.DrawLine(x0, 0, x0, 60, new BitmapColor(255, 0, 0), 1);

            builder.DrawLine(0, y0, 60, y0, new BitmapColor(255, 0, 0), 1);
            */

            offset = 0;
            this.builder.DrawLine(x0, y0, this.getCircleX(offset, rline) + x0, (this.getCircleY(offset, rline)) + y0, lineColor, 1);

            offset = 10;
            this.builder.DrawLine(x0, y0, this.getCircleX(offset, rline) + x0, (this.getCircleY(offset, rline)) + y0, lineColor, 1);

            offset = 20;
            this.builder.DrawLine(x0, y0, this.getCircleX(offset, rline) + x0, (this.getCircleY(offset, rline)) + y0, lineColor, 1);

            offset = 45;
            this.builder.DrawLine(x0, y0, this.getCircleX(offset, rline) + x0, (this.getCircleY(offset, rline)) + y0, lineColor, 1);

            offset = 90;
            this.builder.DrawLine(x0, y0, this.getCircleX(offset, rline) + x0, (this.getCircleY(offset, rline)) + y0, lineColor, 1);

            offset = 135;
            this.builder.DrawLine(x0, y0, this.getCircleX(offset, rline) + x0, (this.getCircleY(offset, rline)) + y0, lineColor, 1);

            offset = 180;
            this.builder.DrawLine(x0, y0, this.getCircleX(offset, rline) + x0, (this.getCircleY(offset, rline)) + y0, lineColor, 1);

            offset = 225;
            this.builder.DrawLine(x0, y0, this.getCircleX(offset, rline) + x0, (this.getCircleY(offset, rline)) + y0, lineColor, 1);

            offset = 270;
            this.builder.DrawLine(x0, y0, this.getCircleX(offset, rline) + x0, (this.getCircleY(offset, rline)) + y0, lineColor, 1);

            offset = 315;
            this.builder.DrawLine(x0, y0, this.getCircleX(offset, rline) + x0, (this.getCircleY(offset, rline)) + y0, lineColor, 1);

            //this.builder.DrawArc(x0, y0, this.rArc, 270, 90, arcGreenColor, 1);
        }

        public BitmapImage getGraph(float val = 40)
        {
            this.init();
            //this.demo();
            if (val < this.red_lo)
            { val = this.red_lo; }
            if (val > red_hi)
            { val = this.red_hi; }
            this.drawArcs();
            this.drawValue(val);
            return this.builder.ToImage();

        }

        public double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
