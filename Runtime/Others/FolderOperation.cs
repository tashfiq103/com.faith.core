namespace com.faith.core
{

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    public static class FolderOperation
    {

        public static List<string> GetSubFoldersName(string[] dataPathOnSubFolders)
        {

            List<string> listOfItemName = new List<string>();
            foreach (string t_Path in dataPathOnSubFolders)
            {

                string[] t_SubFolderPaths = AssetDatabase.GetSubFolders(t_Path);
                foreach (string t_SubFolderPath in t_SubFolderPaths)
                {

                    string[] t_SeperatedByComa = t_SubFolderPath.Split('/');
                    listOfItemName.Add(t_SeperatedByComa[t_SeperatedByComa.Length - 1]);
                }
            }
            return listOfItemName;
        }

        public static List<Object> GetFile<T>(string fileName, string[] dataPathForSubFolders, T fileType, bool breakOperationByFirstFind = false) {

            List<Object> result = new List<Object>();
            string[] GUIDs = AssetDatabase.FindAssets(fileName + " t:" + fileType.ToString(), dataPathForSubFolders);
            foreach (string GUID in GUIDs) {

                string path = AssetDatabase.GUIDToAssetPath(GUID);
                Object unityObject = AssetDatabase.LoadAssetAtPath(path, typeof(T));
                if (unityObject != null) {

                    result.Add(unityObject);
                    if (breakOperationByFirstFind) break;
                }
            }
            return result;
        }
    }
}

