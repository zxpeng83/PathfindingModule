namespace GameConfig
{
    /// <summary>
    /// 游戏模式
    /// </summary>
    public enum GameMode
    {
        AStar,
    }

    /// <summary>
    /// 放置的预瞄物体类型
    /// </summary>
    public enum GraphObjType
    {
        Barrier,
        Target,
        Fake,
    }

    /// <summary>
    /// 地图坐标系x和z轴的范围
    /// </summary>
    public class RangeXYZ
    {
        public static int minX = 1;
        public static int maxX = 21;
        public static int minZ = 1;
        public static int maxZ = 21;
    }
}