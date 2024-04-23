using UnityEngine;

namespace RoundKnights
{
    public struct GridXZCell
    {
        
    }

    public class GridXZ : Grid2D<GridXZCell>
    {
        protected override void OnLoad()
        {
            
        }
        
        protected override GridXZCell CreateCell(int x, int y) => new();

        protected override Vector3 ToVec3(Vector2 v2) => new(v2.x, 0, v2.y);

        protected override Vector2 ToVec2(Vector3 v3) => new(v3.x, v3.z);
    }
}