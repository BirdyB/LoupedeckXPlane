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

            float deg = val * degPerVal;
            int x1 = this.getCircleX(deg, this.rLine)+x0;
            int y1 = y0 - (this.getCircleY(deg, this.rLine));
            Debug.WriteLine($" Value {val} bei max Winkel {angle_max - angle_min} und max. Wert {value_max} ergibt {deg}");
            Debug.WriteLine($"Koordinaten für Linie x0={x0}, y0={y0}, x1={x1}, y1={y1}");
            this.builder.DrawLine(x0, y0,x1 ,y1, lineColor, strokeLine);

        }

        public void drawArcs() {
            int arc_total = angle_max - (angle_min);

            float vmax = red_lo + yellow_lo + green_lo + green_hi + yellow_hi + red_hi;

            float sumact = 0;

            float arc_low = angle_min;
            float arcdeg =(float)(((red_lo - sumact) / vmax) * arc_total);
            if (arcdeg < 0) { arcdeg = 0; }
            BitmapColor clr = arcBlueColor;
            builder.DrawArc(this.x0, this.y0, this.rArc, arc_low, arcdeg, clr, this.strokeArc);
            sumact += red_lo;

            Debug.WriteLine($"arc_low: {arc_low}, arcdec: {arcdeg}");

            arc_low = arc_low + arcdeg;
            arcdeg = (float)(((yellow_lo-sumact) / vmax) * arc_total);
            if (arcdeg < 0)
            { arcdeg = 0; }
            clr = arcRedColor;
            builder.DrawArc(this.x0, this.y0, this.rArc, arc_low, arcdeg, clr, this.strokeArc);
            sumact += yellow_lo;

            Debug.WriteLine($"arc_low: {arc_low}, arcdec: {arcdeg}");

            arc_low = arc_low + arcdeg;
            arcdeg = (float)(((green_lo-sumact) / vmax) * arc_total);
            if (arcdeg < 0)
            { arcdeg = 0; }
            clr = arcYellowColor;
            builder.DrawArc(this.x0, this.y0, this.rArc, arc_low, arcdeg, clr, this.strokeArc);
            sumact += green_lo;

            Debug.WriteLine($"arc_low: {arc_low}, arcdec: {arcdeg}");

            arc_low = arc_low + arcdeg;
            arcdeg = (float)(((green_hi-sumact) / vmax) * arc_total);
            if (arcdeg < 0)
            { arcdeg = 0; }
            clr = arcGreenColor;
            builder.DrawArc(this.x0, this.y0, this.rArc, arc_low, arcdeg, clr, this.strokeArc);
            sumact += green_hi;

            Debug.WriteLine($"arc_low: {arc_low}, arcdec: {arcdeg}");

            arc_low = arc_low + arcdeg;
            arcdeg = (float)(((yellow_hi-sumact) / vmax) * arc_total);
            if (arcdeg < 0)
            { arcdeg = 0; }
            clr = arcYellowColor;
            builder.DrawArc(this.x0, this.y0, this.rArc, arc_low, arcdeg, clr, this.strokeArc);
            sumact += yellow_hi;

            Debug.WriteLine($"arc_low: {arc_low}, arcdec: {arcdeg}");

            arc_low = arc_low + arcdeg;
            arcdeg = (float)(((red_hi-sumact) / vmax) * arc_total);
            if (arcdeg < 0)
            { arcdeg = 0; }
            clr = arcRedColor;
            builder.DrawArc(this.x0, this.y0, this.rArc, arc_low, arcdeg, clr, this.strokeArc);
            sumact += red_hi;

            Debug.WriteLine($"arc_low: {arc_low}, arcdec: {arcdeg}");

            arc_low = arc_low + arcdeg;
            arcdeg = (float)(((vmax) / vmax) * arc_total)-arcdeg;
            if (arcdeg < 0)
            { arcdeg = 0; }
            clr = arcBlueColor;
            builder.DrawArc(this.x0, this.y0, this.rArc, arc_low, arcdeg, clr, this.strokeArc);

            Debug.WriteLine($"arc_low: {arc_low}, arcdec: {arcdeg}");
        }


        public void drawArcs2()
        {
            int arc_offset = 180;
            int arc_total = angle_max - (angle_min+arc_offset);
            float vRange = value_max - value_min;
            float degProVal = arc_total / vRange;
            float curAngle = arc_offset;
            float curVal = 0;
            float curMaxVal = 0;

            Debug.WriteLine($"arc_total: {arc_total}, vRange:{vRange}, degProVal:{degProVal}");


            float angle = red_lo * degProVal;
            Debug.WriteLine($"curAngle: {curAngle}, curVal: {curVal}, angle: {angle}");
            BitmapColor clr = this.arcBlueColor;
           builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);

            curVal += red_lo;
            if (red_lo>curMaxVal)
            { curMaxVal = red_lo; }
            curAngle += angle;

            if (yellow_lo > red_lo)
            {
                angle = (yellow_lo - curMaxVal) * degProVal;
                if (angle < 0)
                { angle = 0; }
            }
            else
            {
                angle = 0;
            }
            Debug.WriteLine($"curAngle: {curAngle}, curVal: {curVal}, angle: {angle}");
            clr = arcRedColor;

            builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);

            curVal += yellow_lo;
            if (yellow_lo > curMaxVal)
            { curMaxVal = yellow_lo; }
            curAngle += angle;

            if (green_lo > yellow_lo)
            {
                angle = (green_lo - curMaxVal) * degProVal;
                if (angle < 0)
                { angle = 0; }
            }
            else
            {
                angle = 0;
            }
            Debug.WriteLine($"curAngle: {curAngle}, curVal: {curVal}, angle: {angle}");
            clr = arcYellowColor;
            builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
            curVal += green_lo;
            if (green_lo > curMaxVal)
            { curMaxVal = green_lo; }
            curAngle += angle;

            if (green_hi > green_lo)
            {
                angle = (green_hi - curMaxVal) * degProVal;
                if (angle < 0)
                { angle = 0; }
            }
            else
            {
                angle = 0;
            }
            Debug.WriteLine($"curAngle: {curAngle}, curVal: {curVal}, angle: {angle}");
            clr = arcGreenColor;
            builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
            curVal += green_hi;
            if (green_hi > curMaxVal)
            { curMaxVal = green_hi; }
            curAngle += angle;

            if (yellow_hi > green_hi)
            {
                angle = (yellow_hi - curMaxVal) * degProVal;
                if (angle < 0)
                { angle = 0; }
            }
            else
            {
                angle = 0;
            }
            Debug.WriteLine($"curAngle: {curAngle}, curVal: {curVal}, angle: {angle}");
            clr = arcYellowColor;

            builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
            curVal += yellow_hi;
            if (yellow_hi > curMaxVal)
            { curMaxVal = yellow_hi; }
            curAngle += angle;

            if (red_hi > yellow_hi)
            {
                angle = (red_hi - curMaxVal) * degProVal;
                if (angle < 0)
                { angle = 0; }
            }
            else
            {
                angle = 0;
            }
            Debug.WriteLine($"curAngle: {curAngle}, curVal: {curVal}, angle: {angle}");
            clr = arcRedColor;

            builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
            curVal += red_hi;
            if (red_hi > curMaxVal)
            { curMaxVal = red_hi; }
            curAngle += angle;

            if (value_max > red_hi)
            {
                angle = (value_max - curMaxVal) * degProVal;
                if (angle < 0)
                { angle = 0; }
            }
            else
            {
                angle = 0;
            }
            Debug.WriteLine($"curAngle: {curAngle}, curVal: {curVal}, angle: {angle}");
            clr = arcBlueColor;
            builder.DrawArc(this.x0, this.y0, this.rArc, curAngle, angle, clr, this.strokeArc);
        }



        public void drawArcs3() {
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
            if (val < this.value_min)
            { val = this.value_min; }
            if (val > value_max)
            { val = this.value_max; }
            this.drawArcs3();
            this.drawValue(val);
            return this.builder.ToImage();

        }

        public double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
