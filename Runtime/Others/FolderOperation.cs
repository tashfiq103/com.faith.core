namespace com.faith.core
{
    using System;
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

        public static List<T> GetFile<T>(string fileName, string[] dataPathForSubFolders, bool breakOperationByFirstFind = false) {
            
            List<T> result = new List<T>();
            string[] GUIDs = AssetDatabase.FindAssets(fileName + " t:" + typeof(T).ToString(), dataPathForSubFolders);
            foreach (string GUID in GUIDs) {

                string path = AssetDatabase.GUIDToAssetPath(GUID);
                T fetchedObject =  (T) Convert.ChangeType(
                    AssetDatabase.LoadAssetAtPath(path, typeof(T)),
                    typeof(T));
                if (fetchedObject != null) {

                    result.Add(fetchedObject);
                    if (breakOperationByFirstFind) break;
                }
            }
            return result;
        }
    }
}

