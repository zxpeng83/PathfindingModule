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
    public enum FakeObjType
    {
        Barrier,
        Target,
    }

    /// <summary>
    /// 地图坐标系x和z轴的
    /// </summary>
    public class Coordinate
    {
        public static int minX = 1;
        public static int maxX = 21;
        public static int minZ = 1;
        public static int maxZ = 21;
    }
}