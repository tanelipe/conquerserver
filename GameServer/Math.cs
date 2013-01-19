using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class ConquerMath
    {
        public static int CalculateDistance(Location Location1, Location Location2)
        {
            return CalculateDistance(Location1.X, Location1.Y, Location2.X, Location2.Y);
        }
        public static double PointDirection(double x1, double y1, double x2, double y2)
        {
            double direction = 0;

            double AddX = x2 - x1;
            double AddY = y2 - y1;
            double r = (double)Math.Atan2(AddY, AddX);

            if (r < 0) r += (double)Math.PI * 2;

            direction = 360 - (r * 180 / (double)Math.PI);
            return direction;
        }
        public static int CalculateDistance(ushort X1, ushort Y1, ushort X2, ushort Y2)
        {
            return (int)Math.Sqrt(Math.Pow(X1 - X2, 2) + Math.Pow(Y1 - Y2, 2));
        }
    }
}
