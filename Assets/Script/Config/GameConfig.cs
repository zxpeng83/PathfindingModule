namespace GameConfig
{
    /// <summary>
    /// ��Ϸģʽ
    /// </summary>
    public enum GameMode
    {
        AStar,
    }

    /// <summary>
    /// ���õ�Ԥ����������
    /// </summary>
    public enum FakeObjType
    {
        Barrier,
        Target,
    }

    /// <summary>
    /// ��ͼ����ϵx��z���
    /// </summary>
    public class Coordinate
    {
        public static int minX = 1;
        public static int maxX = 21;
        public static int minZ = 1;
        public static int maxZ = 21;
    }
}