using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSort
{
    class HSBColor : IComparable
    {
        public float Hue { get; set; }
        public float Saturation { get; set; }
        public float Brightness { get; set; }

        public int CompareTo(object obj)
        {
            var color = (HSBColor)obj;

            if (Hue > color.Hue)
            {
                return 1;
            }
            else if (Hue < color.Hue)
            {
                return -1;
            }
            else if (Brightness > color.Brightness)
            {
                return 1;
            }
            else if (Brightness < color.Brightness)
            {
                return -1;
            }
            else if (Saturation > color.Saturation)
            {
                return 1;
            }
            else if (Saturation < color.Saturation)
            {
                return -1;
            }
            else
            {
                return 0;
            }

        }

        public static int CompareTo(HSBColor color, HSBColor color2)
        {

            if (color2.Hue > color.Hue)
            {
                return 1;
            }
            else if (color2.Hue < color.Hue)
            {
                return -1;
            }

            else if (color2.Saturation > color.Saturation)
            {
                return 1;
            }
            else if (color2.Saturation < color.Saturation)
            {
                return -1;
            }
            else if (color2.Brightness > color.Brightness)
            {
                return 1;
            }
            else if (color2.Brightness < color.Brightness)
            {
                return -1;
            }
            else
            {
                return 0;
            }

        }


        public static bool operator <(HSBColor c1, HSBColor c2)
        {

            return CompareTo(c1, c2) < 0;

        }

        public static bool operator >(HSBColor c1, HSBColor c2)
        {

            return CompareTo(c1, c2) > 0;

        }

        public static bool operator <=(HSBColor c1, HSBColor c2)
        {
            return CompareTo(c1, c2) <= 0;
        }

        public static bool operator >=(HSBColor c1, HSBColor c2)
        {
            return CompareTo(c1, c2) <= 0;
        }

    }
}
