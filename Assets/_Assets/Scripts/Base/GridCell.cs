using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace RoundKnights
{
    public class GridCell
    {
        public bool IsOnNavMesh;
        public List<Entity> Entities;
        public List<Building> Buildings;

        public GridCell(Vector3 center, float cellSize)
        {
            Entities = new();
            Buildings = new();
        }
    }
}