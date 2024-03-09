using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] string saveIdentifier = "Level";
        [field: SerializeField] InputReader inputReader;
        
        void Awake()
        {
            inputReader.ResetEvents();
            PopulateSaves();
            SaveManager.LoadAll(saveIdentifier);
            Environment.Instance.Load();
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
            SaveManager.SearchParents.Add(Environment.Instance.transform);
            SaveManager.Additionals.Clear();
            SaveManager.Additionals.Add(TribesManager.Instance);
            SaveManager.Additionals.Add(CameraManager.Instance);
        }
    }
}