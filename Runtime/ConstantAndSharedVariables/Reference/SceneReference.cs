namespace com.faith.core
{

    [System.Serializable]
    public class SceneReference
    {

#if UNITY_EDITOR
        public string scenePath;
#endif
        public string sceneName;

        public bool UseConstant = true;
        public SceneVariable Variable;

        public SceneReference() { }

        public SceneReference(string value) {

            UseConstant = true;
            sceneName = value;
        }

        public string SceneName
        {
            get { return UseConstant ? sceneName : Variable.sceneName; }
        }

        public static implicit operator string(SceneReference reference) {

            return reference.SceneName;
        }
    }
}

