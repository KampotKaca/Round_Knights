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
            PopulateSaves();
            SaveManager.LoadAll(saveIdentifier);
        }

        [Button]
        void Save()
        {
            PopulateSaves();
            SaveManager.SaveAll(saveIdentifier);
        }

        void PopulateSaves()
        {
            SaveManager.SearchParents.Clear();
            SaveManager.SearchParents.Add(EnvironmentParent);
            SaveManager.Additionals.Clear();
            SaveManager.Additionals.Add(TribesManager.Instance);
            SaveManager.Additionals.Add(CameraManager.Instance);
        }
    }
}