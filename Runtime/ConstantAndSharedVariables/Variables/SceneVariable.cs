namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SceneVariable", menuName = "FAITH/SharedVariable/SceneVariable")]
    public class SceneVariable : ScriptableObject
    {

#if UNITY_EDITOR
        public string scenePath;
#endif

        public string sceneName;
    }
}

