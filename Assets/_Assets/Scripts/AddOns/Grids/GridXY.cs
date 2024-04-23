using UnityEngine;

namespace RoundKnights
{
    public struct GridXYCell
    {
        
    }

    public class GridXY : Grid2D<GridXYCell>
    {
        protected override void OnLoad()
        {
            
        }

        protected override GridXYCell CreateCell(int x, int y) => new();
        protected override Vector3 ToVec3(Vector2 v2) => new(v2.x, v2.y, 0);
        protected override Vector2 ToVec2(Vector3 v3) => new(v3.x, v3.y);
    }
}