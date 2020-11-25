namespace com.faith.core
{
    using UnityEngine;
    using System.Collections.Generic;


    [CreateAssetMenu(fileName = "SceneContainer", menuName = ScriptableObjectAssetMenu.MENU_SCENE_CONTAINER, order = ScriptableObjectAssetMenu.ORDER_SCENE_CONTAINER)]
    public class SceneContainerAsset : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private bool   isFollowingScenseListIncludedInBuild = false;
#endif
        [SerializeField] private        List<SceneReference> listOfScene;
    }
}

