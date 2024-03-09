using UnityEngine;

namespace RoundKnights
{
    public class Environment : Singleton<Environment>
    {
        [SerializeField] Vector2 defaultSize = Vector2.one;
        [SerializeField] Vector2 additionalSize = Vector2.one;
        
        public void Load()
        {
            CameraManager.Instance.AddEdgeBounds(new()
            {
                Center = transform.position.TOV2(),
                Size = defaultSize
            });
            
            CameraManager.Instance.AddEdgeBounds(new()
            {
                Center = transform.position.TOV2() + new Vector2(5, 5),
                Size = additionalSize
            });
        }
    }
}