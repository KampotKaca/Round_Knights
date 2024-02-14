using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] string saveIdentifier = "Level";
        [field: SerializeField] public Transform EnvironmentParent { get; private set; }

        void Awake()
        {
            SaveManager.SearchParents.Clear();
            SaveManager.SearchParents.Add(EnvironmentParent);
            SaveManager.Additionals.Clear();
            SaveManager.Additionals.Add(TribesManager.Instance);
            
            SaveManager.LoadAll(saveIdentifier);
        }

        [Button]
        void Save()
        {
            SaveManager.SearchParents.Clear();
            SaveManager.SearchParents.Add(EnvironmentParent);
            SaveManager.Additionals.Clear();
            SaveManager.Additionals.Add(TribesManager.Instance);
            
            SaveManager.SaveAll(saveIdentifier);
        }
    }
}