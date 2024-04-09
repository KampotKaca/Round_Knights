using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] string saveIdentifier = "Level";
        [field: SerializeField] InputReader inputReader;

        protected override void OnAwakeEvent()
        {
            inputReader.Init();
            populateSaves();
            SaveManager.LoadAll(saveIdentifier);
            Environment.Instance.Load();
        }

        [Button]
        void Save()
        {
            populateSaves();
            SaveManager.SaveAll(saveIdentifier);
        }

        void populateSaves()
        {
            SaveManager.SearchParents.Clear();
            SaveManager.SearchParents.Add(Environment.Instance.transform);
            SaveManager.Additionals.Clear();
            SaveManager.Additionals.Add(TribesManager.Instance);
            SaveManager.Additionals.Add(CameraManager.Instance);
        }
    }
}