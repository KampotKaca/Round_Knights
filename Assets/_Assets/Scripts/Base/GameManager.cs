using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] string saveIdentifier = "Level";
        [field: SerializeField] public Transform EnvironmentParent { get; private set; }
        [field: SerializeField] public Transform TeamsParent { get; private set; }

        void Awake()
        {
            SaveManager.SearchParents.Clear();
            SaveManager.SearchParents.Add(EnvironmentParent);
            SaveManager.Additionals.Clear();
            
            SaveManager.LoadAll(saveIdentifier);
        }

        [Button]
        void Save()
        {
            SaveManager.SearchParents.Clear();
            SaveManager.SearchParents.Add(EnvironmentParent);
            SaveManager.Additionals.Clear();
            
            SaveManager.SaveAll(saveIdentifier);
        }
    }
}