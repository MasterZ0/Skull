using Sirenix.OdinInspector;
using UnityEngine;

namespace AdventureGame.Shared
{
    /// <summary>
    /// Temporary
    /// </summary>
    public abstract class Singleton<T> : SerializedMonoBehaviour where T : MonoBehaviour 
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There already have a instances of {typeof(T).Name}, Current: {Instance.gameObject} and new {gameObject}");
                return;
            }

            Instance = this as T;
        }
    }
}