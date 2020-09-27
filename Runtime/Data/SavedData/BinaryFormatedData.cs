namespace com.faith.core
{
    using System;
    using System.Collections.Specialized;
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;


    [Serializable]
    public class BinaryDataWrapper
    {
        [SerializeField] public OrderedDictionary keyedValues;

        public BinaryDataWrapper() {

            keyedValues = new OrderedDictionary();
        }
    }

    public static class BinaryFormatedData
    {
        #region Public Variables

        public static BinaryDataWrapper rawBinaryData;

        #endregion

        #region Private Variables

        private static string _fileName = "binaryDataWrapper";
        private static string _fileExtension = "bd";
        private static List<string> _listOfBinaryDataKeys;

        private static bool _isInitialDataLoaded = false;
        private static bool _isDataLoadingProcessOnGoing = false;

        private static int _numberOfKeysInBinaryDatabase;

        private static Queue<BinaryData<bool>> _queueOfBooleanBinaryDataToBeRetrived = new Queue<BinaryData<bool>>();
        private static Queue<BinaryData<int>> _queueOfIntegerBinaryDataToBeRetrived = new Queue<BinaryData<int>>();
        private static Queue<BinaryData<float>> _queueOfFloatBinaryDataToBeRetrived = new Queue<BinaryData<float>>();
        private static Queue<BinaryData<double>> _queueOfDoubleBinaryDataToBeRetrived = new Queue<BinaryData<double>>();
        private static Queue<BinaryData<string>> _queueOfStringBinaryDataToBeRetrived = new Queue<BinaryData<string>>();

        #endregion

        #region Configuretion

        private static void OnDataSavedSucced()
        {
            CoreDebugger.Debug.Log("Data saved successfully");
            
        }

        private static void OnDataLoadFailed() {

            CoreDebugger.Debug.LogError("Failed to retrive the binaryData");
            SaveLoadOperation.SaveData(
                new BinaryDataWrapper(),
                delegate {
                    SaveLoadOperation.LoadData<BinaryDataWrapper>(OnDataLoadFailed, OnDataLoadSucceed, _fileName, _fileExtension);
                },
                _fileName,
                _fileExtension);
        }

        private static void OnDataLoadSucceed(BinaryDataWrapper binaryDataWrapper) {

            rawBinaryData = binaryDataWrapper;

            _isDataLoadingProcessOnGoing = false;
            _isInitialDataLoaded = true;

            _numberOfKeysInBinaryDatabase = rawBinaryData.keyedValues.Count;

            ICollection keyCollections      = rawBinaryData.keyedValues.Keys;
            string[] keysArray             = new string[_numberOfKeysInBinaryDatabase];
            keyCollections.CopyTo(keysArray, 0);

            _listOfBinaryDataKeys                           = new List<string>(keysArray);
            

            //Fetch :   BooleanData
            OnPassingTheLoadedBinaryDataToDifferentQueueV2(ref _numberOfKeysInBinaryDatabase, ref _queueOfBooleanBinaryDataToBeRetrived);
            //Fetch :   IntegerData
            OnPassingTheLoadedBinaryDataToDifferentQueueV2(ref _numberOfKeysInBinaryDatabase, ref _queueOfIntegerBinaryDataToBeRetrived);
            //Fetch :   FloatData
            OnPassingTheLoadedBinaryDataToDifferentQueueV2(ref _numberOfKeysInBinaryDatabase, ref _queueOfFloatBinaryDataToBeRetrived);
            //Fetch :   DoubleData
            OnPassingTheLoadedBinaryDataToDifferentQueueV2(ref _numberOfKeysInBinaryDatabase, ref _queueOfDoubleBinaryDataToBeRetrived);
            //Fetch :   StringData
            OnPassingTheLoadedBinaryDataToDifferentQueueV2(ref _numberOfKeysInBinaryDatabase, ref _queueOfStringBinaryDataToBeRetrived);
        }

        private static void OnPassingTheLoadedBinaryDataToDifferentQueueV2<T>(ref int numberOfDataBinaryDatabase, ref Queue<BinaryData<T>> queueOfBinaryDataToRecieveInitialValues) {

            int index = numberOfDataBinaryDatabase;
            while (queueOfBinaryDataToRecieveInitialValues.Count > 0) {

                bool hasFoundInBinarayDatabase = false;

                BinaryData<T> binaryData = queueOfBinaryDataToRecieveInitialValues.Dequeue();
                T value = binaryData.GetInitializedValue();

                for (int i = 0; i < numberOfDataBinaryDatabase; i++)
                {

                    if (StringOperation.IsSameString(_listOfBinaryDataKeys[i], binaryData.GetKey()))
                    {
                        index = i;
                        value = (T) rawBinaryData.keyedValues[i];
                        hasFoundInBinarayDatabase = true;
                        break;
                    }
                }

                if (!hasFoundInBinarayDatabase)
                {
                    rawBinaryData.keyedValues.Add(binaryData.GetKey(), value);
                    binaryData.SetIndexOfBinaryDataWrapper(index++);
                }
                else
                    binaryData.SetIndexOfBinaryDataWrapper(index);


                binaryData.SetData(value);
            }
        }

        private static void AssignedToDesignatedQueueForRetrivingData<T>(BinaryData<T> binaryData) {
            
            if (typeof(T) == typeof(bool))
            {
                _queueOfBooleanBinaryDataToBeRetrived.Enqueue((BinaryData<bool>)Convert.ChangeType(binaryData, typeof(BinaryData<bool>)));
            }
            else if (typeof(T) == typeof(int))
            {
                _queueOfIntegerBinaryDataToBeRetrived.Enqueue((BinaryData<int>)Convert.ChangeType(binaryData, typeof(BinaryData<int>)));
            }
            else if (typeof(T) == typeof(float))
            {
                _queueOfFloatBinaryDataToBeRetrived.Enqueue((BinaryData<float>)Convert.ChangeType(binaryData, typeof(BinaryData<float>)));
            }
            else if (typeof(T) == typeof(double))
            {
                _queueOfDoubleBinaryDataToBeRetrived.Enqueue((BinaryData<double>)Convert.ChangeType(binaryData, typeof(BinaryData<double>)));
            }
            else if (typeof(T) == typeof(string))
            {
                _queueOfStringBinaryDataToBeRetrived.Enqueue((BinaryData<string>)Convert.ChangeType(binaryData, typeof(BinaryData<string>)));

            }
        }

        private static void PassRetrivedData<T>(ref BinaryData<T> binaryData) {

            int index   = _numberOfKeysInBinaryDatabase;
            T value     = binaryData.GetInitializedValue();

            bool hasFoundDataInTheList = false;
            for (int i = 0; i < _numberOfKeysInBinaryDatabase; i++)
            {

                if (StringOperation.IsSameString(binaryData.GetKey(), _listOfBinaryDataKeys[i]))
                {
                    value = (T) rawBinaryData.keyedValues[i];
                    index = i;
                    hasFoundDataInTheList = true;
                    break;
                }
            }

            if (!hasFoundDataInTheList)
            {

                rawBinaryData.keyedValues.Add(binaryData.GetKey(), value);
                _listOfBinaryDataKeys.Add(binaryData.GetKey());
            }

            binaryData.SetIndexOfBinaryDataWrapper(index);
            binaryData.SetData(value);
        }

        #endregion

        #region Public Callback

        public static void RegisterInBinaryData<T>(BinaryData<T> binaryData) {

            if (!_isInitialDataLoaded)
            {

                AssignedToDesignatedQueueForRetrivingData(binaryData);

                if (!_isDataLoadingProcessOnGoing)
                {
                    _isDataLoadingProcessOnGoing = true;
                    SaveLoadOperation.LoadData<BinaryDataWrapper>(OnDataLoadFailed, OnDataLoadSucceed, _fileName, _fileExtension);
                }
            }
            else {

                PassRetrivedData(ref binaryData);
            }
        }

        public static T GetData<T>(ref int index){

            return (T) rawBinaryData.keyedValues[index];
        }

        public static void SetData<T>(int index, T value) {

            rawBinaryData.keyedValues[index] = value;
        }

        public static void SaveDataSnapshot() {

            SaveLoadOperation.SaveData(rawBinaryData, OnDataSavedSucced, _fileName, _fileExtension);
        }

        #endregion
    }
}


