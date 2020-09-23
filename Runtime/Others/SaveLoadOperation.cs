namespace com.faith.core
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;

    public class SaveLoadOperation   : MonoBehaviour
    {

        public static void SaveData<T>(T data, Action OnDataSaved = null, string fileName = "saveFile", string extension = "data") {

            string path = "";

            if (Application.isEditor)
            {
                path = Application.dataPath + "/_BinaryFormatedData";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path += "/" + fileName + "." + extension;
            }
            else
                path = Application.persistentDataPath + "/" + fileName + "." + extension;

            CoreDebugger.Debug.Log("SavedFile : " + path);

            FileStream fileStream = new FileStream(path, FileMode.Create);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, data);

            fileStream.Close();

            OnDataSaved?.Invoke();
        }

        public static void LoadData<T>(Action OnDataLoadFailed, Action<T> OnDataLoadSucceed = null, string fileName = "saveFile", string extension = "data") {

            T data;

            string path = "";

            if (Application.isEditor)
                path = Application.dataPath + "/_BinaryFormatedData/" + fileName + "." + extension;
            else
                path = Application.persistentDataPath + "/" + fileName + "." + extension;

            if (File.Exists(path))
            {
                FileStream fileStream = new FileStream(path, FileMode.Open);

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                data = (T) Convert.ChangeType(binaryFormatter.Deserialize(fileStream), typeof(T));

                fileStream.Close();

                OnDataLoadSucceed?.Invoke(data);
            }
            else {

                OnDataLoadFailed.Invoke();
            }

        }
    }
}

