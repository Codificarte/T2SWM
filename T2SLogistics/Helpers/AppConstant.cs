using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Helpers
{
    public class AppConstant
    {
        private static List<Color> _randomColorList = new List<Color>()
        {
            Colors.Orange,
            Colors.Blue,
            Colors.Red,
            Colors.Yellow,
            Colors.Green,
            Colors.BlanchedAlmond,
            Colors.Beige,
            Colors.Purple,
            Colors.Magenta,
            Colors.Cyan,
            Colors.DarkBlue,
            Colors.DarkGreen,
            Colors.DarkRed,
            Colors.Chocolate,
            Colors.Turquoise,
            Colors.WhiteSmoke,
            Colors.YellowGreen,
            Colors.Violet,
            Colors.Tomato,
            Colors.Teal,
            Colors.Wheat,
            Colors.Thistle,
            Colors.Tan,
            Colors.Coral
        };

        public static Color GetRandomColor()
        {
            return _randomColorList[new Random().Next(_randomColorList.Count)];
        }


    }
}
