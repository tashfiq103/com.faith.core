namespace com.faith.core
{

    using System.Collections.Generic;
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

    }
}

