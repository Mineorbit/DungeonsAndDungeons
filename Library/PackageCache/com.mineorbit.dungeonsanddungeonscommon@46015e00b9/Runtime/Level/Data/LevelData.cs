using System;
using System.Collections.Generic;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    [Serializable]
    public class LevelData
    {
        public Dictionary<Tuple<int, int>, int> regions = new Dictionary<Tuple<int, int>, int>();
    }
}