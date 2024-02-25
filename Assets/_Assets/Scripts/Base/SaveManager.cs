using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

#if !UNITY_EDITOR
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace RoundKnights
{
    public static class SaveManager
    {
        [Serializable]
        public struct SaveFile
        {
            public string Id;
            public string Cont;
        }
        
        [Serializable]
        public struct SaveData
        {
            public SaveFile[] Files;
        }
        
        const string END_LINE = ".rkt";

        public static readonly HashSet<Transform> SearchParents = new();
        public static readonly HashSet<ISavedObject> Additionals = new();

        [MenuItem("Tools/DeleteSaves")]
        static void DeleteAllSaves()
        {
            var files = Directory.GetFiles(Application.persistentDataPath);
            foreach (string file in files) File.Delete(file);
        }
        
        [MenuItem("Tools/OpenSavePath")]
        static void OpenSavePath()
        {
            if (!Directory.Exists(Application.persistentDataPath))
                Directory.CreateDirectory(Application.persistentDataPath);

            Process.Start(Application.persistentDataPath);
        }
        
        static ISavedObject[] GetResults()
        {
            List<ISavedObject> results = new();

            foreach (var parent in SearchParents)
            {
                var res = parent.GetComponentsInChildren<ISavedObject>(true);
                foreach (var saveObject in res) if(saveObject.Queue >= 0) results.Add(saveObject);
            }
            
            results.AddRange(Additionals);
            results.Sort((x, y) => x.Queue - y.Queue);
            
            return results.ToArray();
        }
        
        public static void LoadAll(string identifier)
        {
            var saveObjects = GetResults();

            string path = GetPath(identifier);
            if (File.Exists(path))
            {
                FileStream file = new(path, FileMode.Open);
                
#if UNITY_EDITOR

                SaveData saveData;
                using (StreamReader stream = new(file))
                {
                    string json = stream.ReadToEnd();
                    saveData = JsonUtility.FromJson<SaveData>(json);
                }
#else
                BinaryFormatter formatter = new();
                SaveData saveData = (SaveData)formatter.Deserialize(file);           
#endif
                
                file.Close();
                
                Dictionary<string, string> dic = new();
                for (int i = 0; i < saveData.Files.Length; i++)
                    dic.Add(saveData.Files[i].Id, saveData.Files[i].Cont);
                
                foreach (var savedObject in saveObjects)
                {
                    if(savedObject.Queue < 0) continue;

                    if (dic.TryGetValue(savedObject.SaveFileIdentifier, out var save))
                        savedObject.LoadFromSaveFile(JsonUtility.FromJson(save, savedObject.FileType));
                    else savedObject.LoadDefault();
                }
            }
            else
            {
                foreach (var savedObject in saveObjects)
                {
                    if(savedObject.Queue < 0) continue;
                    savedObject.LoadDefault();
                }
            }
        }

        public static void SaveAll(string identifier)
        {
            var saveObjects = GetResults();

            Dictionary<string, string> data = new();

            foreach (var so in saveObjects)
            {
#if UNITY_EDITOR
                if (data.ContainsKey(so.SaveFileIdentifier))
                {
                    Debug.LogError($"Trying to insert key twice {so.SaveFileIdentifier}");
                    continue;
                }
#endif
                var save = so.GetSaveFile();
                if(save == null) continue;
                string json = JsonUtility.ToJson(save);
                data.Add(so.SaveFileIdentifier, json);
            }

            SaveData saveData = new()
            {
                Files = new SaveFile[data.Count]
            };

            int id = 0;
            foreach (var pair in data)
            {
                saveData.Files[id] = new SaveFile
                {
                    Id = pair.Key,
                    Cont = pair.Value
                };
                id++;
            }

            string path = GetPath(identifier);
            
            FileStream file = new(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            
#if UNITY_EDITOR
            using(StreamWriter stream = new(file))
            {
                var json = JsonUtility.ToJson(saveData);
                stream.WriteAsync(json);
            };
#else
            BinaryFormatter formatter = new();
            formatter.Serialize(file, saveData);
#endif
            
            file.Close();
            
            Debug.Log(path);
        }
        
        static string GetPath(string identifier) 
            => $"{Application.persistentDataPath}/{identifier}{END_LINE}";
    }
    
    public enum SaveFileQueue { Skip    = -1,
                                PreLoad = 0,    Load    = 1000, PostLoad = 2000, 
                                PreRun  = 3000, Run     = 4000, PostRun  = 5000,
                                PreLate = 6000, Late    = 7000, PostLate = 8000 }
    
    public interface ISavedObject
    {
        public Type FileType { get; }
        public int Queue { get; }
        public string SaveFileIdentifier { get; }
        public void LoadDefault();
        public void LoadFromSaveFile(object saveFile);
        public object GetSaveFile();
    }
}