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
        [SerializeField] public OrderedDictionary boolValues;
        [SerializeField] public OrderedDictionary intValues;
        [SerializeField] public OrderedDictionary floatValues;
        [SerializeField] public OrderedDictionary doubleValues;
        [SerializeField] public OrderedDictionary stringValues;

        public BinaryDataWrapper() {

            boolValues = new OrderedDictionary();
            intValues = new OrderedDictionary();
            floatValues = new OrderedDictionary();
            doubleValues = new OrderedDictionary();
            stringValues = new OrderedDictionary();
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

        private static bool _isInitialDataLoaded = false;
        private static bool _isDataLoadingProcessOnGoing = false;

        private static bool[]   boolValues;
        private static int[]    intValues;
        private static float[]  floatValues;
        private static double[] doubleValues;
        private static string[] stringValues;

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

            //Fetch :   BooleanData
            int numberOfData                = rawBinaryData.boolValues.Count;
            ICollection keyCollections      = rawBinaryData.boolValues.Keys;
            ICollection valueCollection     = rawBinaryData.boolValues.Values;
            
            string[] keys                   = new string[numberOfData];
            boolValues                      = new bool[numberOfData];
            keyCollections.CopyTo(keys, 0);
            valueCollection.CopyTo(boolValues, 0);

            OnPassingTheLoadedBinaryDataToDifferentQueue(numberOfData, ref keys, ref boolValues, ref _queueOfBooleanBinaryDataToBeRetrived, ref rawBinaryData.boolValues);

            //Fetch :   IntegerData
            numberOfData = rawBinaryData.intValues.Count;
            keyCollections = rawBinaryData.intValues.Keys;
            valueCollection = rawBinaryData.intValues.Values;

            keys = new string[numberOfData];
            intValues = new int[numberOfData];
            keyCollections.CopyTo(keys, 0);
            valueCollection.CopyTo(intValues, 0);

            OnPassingTheLoadedBinaryDataToDifferentQueue(numberOfData, ref keys, ref intValues, ref _queueOfIntegerBinaryDataToBeRetrived, ref rawBinaryData.intValues);

            //Fetch :   FloatData
            numberOfData = rawBinaryData.floatValues.Count;
            keyCollections = rawBinaryData.floatValues.Keys;
            valueCollection = rawBinaryData.floatValues.Values;

            keys = new string[numberOfData];
            floatValues = new float[numberOfData];
            keyCollections.CopyTo(keys, 0);
            valueCollection.CopyTo(floatValues, 0);

            OnPassingTheLoadedBinaryDataToDifferentQueue(numberOfData, ref keys, ref floatValues, ref _queueOfFloatBinaryDataToBeRetrived, ref rawBinaryData.floatValues);

            //Fetch :   DoubleData
            numberOfData = rawBinaryData.doubleValues.Count;
            keyCollections = rawBinaryData.doubleValues.Keys;
            valueCollection = rawBinaryData.doubleValues.Values;

            keys = new string[numberOfData];
            doubleValues = new double[numberOfData];
            keyCollections.CopyTo(keys, 0);
            valueCollection.CopyTo(doubleValues, 0);

            OnPassingTheLoadedBinaryDataToDifferentQueue(numberOfData, ref keys, ref doubleValues, ref _queueOfDoubleBinaryDataToBeRetrived, ref rawBinaryData.doubleValues);

            //Fetch :   StringData
            numberOfData = rawBinaryData.stringValues.Count;
            keyCollections = rawBinaryData.stringValues.Keys;
            valueCollection = rawBinaryData.stringValues.Values;

            keys = new string[numberOfData];
            stringValues = new string[numberOfData];
            keyCollections.CopyTo(keys, 0);
            valueCollection.CopyTo(stringValues, 0);

            OnPassingTheLoadedBinaryDataToDifferentQueue(numberOfData, ref keys, ref stringValues, ref _queueOfStringBinaryDataToBeRetrived, ref rawBinaryData.stringValues);
        }

        private static void OnPassingTheLoadedBinaryDataToDifferentQueue<T>(
            int numberOfDataBinaryDatabase,
            ref string[] keys,
            ref T[] values,
            ref Queue<BinaryData<T>> queueOfBinaryDataToRecieveInitialValues,
            ref OrderedDictionary orderedDictionary) {

            int index = numberOfDataBinaryDatabase;
            while (queueOfBinaryDataToRecieveInitialValues.Count > 0)
            {
                bool hasFoundInBinarayDatabase = false;

                BinaryData<T> binaryData = queueOfBinaryDataToRecieveInitialValues.Dequeue();
                T value     = binaryData.GetInitializedValue();

                for (int i = 0; i < numberOfDataBinaryDatabase; i++)
                {

                    if (StringOperation.IsSameString(keys[i], binaryData.GetKey()))
                    {
                        index = i;
                        value = values[i];
                        hasFoundInBinarayDatabase = true;
                        break;
                    }
                }

                if (!hasFoundInBinarayDatabase) {

                    orderedDictionary.Add(binaryData.GetKey(), value);

                    List<T> listOfValues = new List<T>(values);
                    listOfValues.Add(value);
                    values = listOfValues.ToArray();

                    binaryData.SetIndexOfBinaryDataWrapper(index++);
                }else
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

            if (typeof(T) == typeof(bool))
            {
                SetIndexAndPassInitialData(ref binaryData, ref rawBinaryData.boolValues);

            }
            else if (typeof(T) == typeof(int))
            {
                SetIndexAndPassInitialData(ref binaryData, ref rawBinaryData.intValues);
            }
            else if (typeof(T) == typeof(float))
            {
                SetIndexAndPassInitialData(ref binaryData, ref rawBinaryData.floatValues);
            }
            else if (typeof(T) == typeof(double))
            {
                SetIndexAndPassInitialData(ref binaryData, ref rawBinaryData.doubleValues);
            }
            else if (typeof(T) == typeof(string))
            {
                SetIndexAndPassInitialData(ref binaryData, ref rawBinaryData.stringValues);
            }
        }

        private static void SetIndexAndPassInitialData<T>(ref BinaryData<T> binaryData, ref OrderedDictionary orderedDictionary) {

            int numberOfDataInBinaryDatabase    = orderedDictionary.Count;
            int index                           = numberOfDataInBinaryDatabase;
            T value                             = binaryData.GetInitializedValue();



            string[] keys       = new string[numberOfDataInBinaryDatabase];
            ICollection keyCollections  = orderedDictionary.Keys;
            keyCollections.CopyTo(keys, 0);


            bool hasFoundDataInTheList = false;
            for (int i = 0; i < numberOfDataInBinaryDatabase; i++) {

                if (StringOperation.IsSameString(binaryData.GetKey(), keys[i])) {

                    T[] values = new T[numberOfDataInBinaryDatabase];
                    ICollection valueCollection = orderedDictionary.Values;
                    valueCollection.CopyTo(values, 0);

                    value = values[i];
                    index = i;
                    hasFoundDataInTheList = true;
                    break;
                }
            }

            if (!hasFoundDataInTheList) {

                orderedDictionary.Add(binaryData.GetKey(), value);

                if (typeof(T) == typeof(bool))
                {
                    List<bool> listOfValues = new List<bool>(boolValues);
                    listOfValues.Add((bool) Convert.ChangeType(value, typeof(bool)));
                    boolValues = listOfValues.ToArray();
                }
                else if (typeof(T) == typeof(int))
                {
                    List<int> listOfValues = new List<int>(intValues);
                    listOfValues.Add((int)Convert.ChangeType(value, typeof(int)));
                    intValues = listOfValues.ToArray();
                }
                else if (typeof(T) == typeof(float))
                {
                    List<float> listOfValues = new List<float>(floatValues);
                    listOfValues.Add((float)Convert.ChangeType(value, typeof(float)));
                    floatValues = listOfValues.ToArray();
                }
                else if (typeof(T) == typeof(double))
                {
                    List<double> listOfValues = new List<double>(doubleValues);
                    listOfValues.Add((double)Convert.ChangeType(value, typeof(double)));
                    doubleValues = listOfValues.ToArray();
                }
                else if (typeof(T) == typeof(string))
                {
                    List<string> listOfValues = new List<string>(stringValues);
                    listOfValues.Add((string)Convert.ChangeType(value, typeof(string)));
                    stringValues = listOfValues.ToArray();
                }

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

            if (typeof(T) == typeof(bool))
            {
                return (T) Convert.ChangeType(boolValues[index], typeof(T));
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)Convert.ChangeType(intValues[index], typeof(T));
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)Convert.ChangeType(floatValues[index], typeof(T));
            }
            else if (typeof(T) == typeof(double))
            {
                return (T)Convert.ChangeType(doubleValues[index], typeof(T));
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(stringValues[index], typeof(T));
            }

            return default;
        }

        public static void SetData<T>(int index, T value) {

            if (typeof(T) == typeof(bool))
            {
                boolValues[index]               = (bool) Convert.ChangeType(value, typeof(bool));
                rawBinaryData.boolValues[index] = boolValues[index];
            }
            else if (typeof(T) == typeof(int))
            {
                intValues[index] = (int)Convert.ChangeType(value, typeof(int));
                rawBinaryData.intValues[index] = intValues[index];
            }
            else if (typeof(T) == typeof(float))
            {
                floatValues[index] = (float)Convert.ChangeType(value, typeof(float));
                rawBinaryData.floatValues[index] = floatValues[index];
            }
            else if (typeof(T) == typeof(double))
            {
                doubleValues[index] = (double)Convert.ChangeType(value, typeof(double));
                rawBinaryData.doubleValues[index] = doubleValues[index];
            }
            else if (typeof(T) == typeof(string))
            {
                stringValues[index] = (string)Convert.ChangeType(value, typeof(string));
                rawBinaryData.stringValues[index] = stringValues[index];
            }
        }

        public static void SaveDataSnapshot() {

            SaveLoadOperation.SaveData(rawBinaryData, OnDataSavedSucced, _fileName, _fileExtension);
        }

        #endregion
    }
}


