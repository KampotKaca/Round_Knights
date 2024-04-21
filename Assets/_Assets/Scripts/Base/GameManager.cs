using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] string saveIdentifier = "Level";

        public const string AFTER_LOAD_MESSAGE = "OnAfterLoadMessage";
        
        protected override void OnAwakeEvent()
        {
            InputReader.Instance.ResetEvents();
            InputReader.Instance.Init();
            
            Environment.Instance.Load();
            
            populateSaves();
            SaveManager.LoadAll(saveIdentifier);
            
            AddOns.SendGlobalMessage(AFTER_LOAD_MESSAGE);
        }

        [Button]
        void Save()
        {
            populateSaves();
            SaveManager.SaveAll(saveIdentifier);
        }

        static void populateSaves()
        {
            SaveManager.SearchParents.Clear();
            SaveManager.SearchParents.Add(Environment.Instance.transform);
            SaveManager.Additionals.Clear();
            SaveManager.Additionals.Add(TribesManager.Instance);
            SaveManager.Additionals.Add(CameraManager.Instance);
        }
    }
}