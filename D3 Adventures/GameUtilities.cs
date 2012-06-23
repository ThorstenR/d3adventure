using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using D3_Adventures.Structures;
using Utilities.WinControl;

namespace D3_Adventures
{
    public static class GameUtilities
    {
        // thanks to http://www.blizzhackers.cc/viewtopic.php?p=4584495#p4584495 and Opkllhibus
        // UNTESTED!
        public static PointF FromD3toScreenCoords(Vec3 vec3)
        {
            Console.WriteLine("FromD3toScreenCoords HAS NOT YET BEEN TESTED!");
            RECT rect;
            WC.GetClientRect(Globals.winHandle, out rect);
            int resolutionX = rect.Width;
            int resolutionY = rect.Height;

            double aspectChange = (resolutionX/resolutionY)/(800/600); // 800/600 = default aspect ratio
            Vec3 currentLoc = Data.getCurrentPos();

            double xd = vec3.x - currentLoc.x;
            double yd = vec3.y - currentLoc.y;
            double zd = vec3.z - currentLoc.z;

            double w = -0.515 * xd + -0.514 * yd + -0.686 * zd + 97.985;

            double X = (-1.682 * xd + 1.683 * yd + 0 * zd + 7.045e-3) / w;
            double Y = (-1.54 * xd + -1.539 * yd + 2.307 * zd + 6.161) / w;
            double Z = (-0.515 * xd + -0.514 * yd + -0.686 * zd + 97.002) / w;

            X /= aspectChange;

            while (Math.Abs(X) >= 1 || Math.Abs(Y) >= 1 || Z <= 0)
            {
                xd /= 2;
                yd /= 2;
                zd /= 2;

                w = -0.515 * xd + -0.514 * yd + -0.686 * zd + 97.985;
                X = (-1.682 * xd + 1.683 * yd + 0 * zd + 7.045e-3) / w;
                Y = (-1.54 * xd + -1.539 * yd + 2.307 * zd + 6.161) / w;
                Z = (-0.515 * xd + -0.514 * yd + -0.686 * zd + 97.002) / w;

                X /= aspectChange;
            }

            float rX = (float)((X + 1) / 2 * resolutionX);
            float rY = (float)((1 - Y) / 2 * resolutionY);

            return new PointF(rX, rY);
        }
    }
}