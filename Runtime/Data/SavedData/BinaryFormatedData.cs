﻿namespace com.faith.core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    [Serializable]
    public class BinaryDataWrapper
    {
        [SerializeField] public Dictionary<string, bool>    boolValues;
        [SerializeField] public Dictionary<string, int>     intValues;
        [SerializeField] public Dictionary<string, float>   floatValues;
        [SerializeField] public Dictionary<string, double>  doubleValues;
        [SerializeField] public Dictionary<string, string>  stringValues;

        public BinaryDataWrapper() {

            boolValues  = new Dictionary<string, bool>();
            intValues   = new Dictionary<string, int>();
            floatValues = new Dictionary<string, float>();
            doubleValues= new Dictionary<string, double>();
            stringValues= new Dictionary<string, string>();
        }
    }

    public static class BinaryFormatedData
    {
        #region Private Variables

        private static string _fileName = "binaryDataWrapper";
        private static string _fileExtension = "bd";

        private static bool _isInitialDataLoaded = false;
        private static bool _isDataLoadingProcessOnGoing = false;

        private static BinaryDataWrapper _binaryDataWrapper;

        private static Queue<BinaryData<bool>> _listOfBooleanBinaryDataToBeRetrived;
        private static Queue<BinaryData<int>> _listOfIntegerBinaryDataToBeRetrived;
        private static Queue<BinaryData<float>> _listOfFloatBinaryDataToBeRetrived;
        private static Queue<BinaryData<double>> _listOfDoubleBinaryDataToBeRetrived;
        private static Queue<BinaryData<string>> _listOfStringBinaryDataToBeRetrived;

        #endregion

        #region Configuretion

        private static void OnDataLoadFailed() {

            CoreDebugger.Debug.LogError("Failed to retrive the binaryData");
        }

        private static void OnDataLoadSucceed(BinaryDataWrapper binaryDataWrapper) {

            _binaryDataWrapper = binaryDataWrapper;

            _isInitialDataLoaded = true;

            while (_listOfBooleanBinaryDataToBeRetrived.Count > 0)
            {
                BinaryData<bool> binaryData = _listOfBooleanBinaryDataToBeRetrived.Dequeue();

                bool _value = false;
                _binaryDataWrapper.boolValues.TryGetValue(binaryData.GetKey(), out _value);
                binaryData.SetData(_value);

            }

            while (_listOfIntegerBinaryDataToBeRetrived.Count > 0)
            {
                BinaryData<int> binaryData = _listOfIntegerBinaryDataToBeRetrived.Dequeue();

                int _value = 0;
                _binaryDataWrapper.intValues.TryGetValue(binaryData.GetKey(), out _value);
                binaryData.SetData(_value);

            }

            while (_listOfFloatBinaryDataToBeRetrived.Count > 0)
            {
                BinaryData<float> binaryData = _listOfFloatBinaryDataToBeRetrived.Dequeue();

                float _value = 0;
                _binaryDataWrapper.floatValues.TryGetValue(binaryData.GetKey(), out _value);
                binaryData.SetData(_value);

            }

            while (_listOfDoubleBinaryDataToBeRetrived.Count > 0)
            {
                BinaryData<double> binaryData = _listOfDoubleBinaryDataToBeRetrived.Dequeue();

                double _value = 0;
                _binaryDataWrapper.doubleValues.TryGetValue(binaryData.GetKey(), out _value);
                binaryData.SetData(_value);

            }

            while (_listOfStringBinaryDataToBeRetrived.Count > 0)
            {
                BinaryData<string> binaryData = _listOfStringBinaryDataToBeRetrived.Dequeue();

                string _value = "";
                _binaryDataWrapper.stringValues.TryGetValue(binaryData.GetKey(), out _value);
                binaryData.SetData(_value);

            }
        }

        private static void OnDataSavedSucced() {

            CoreDebugger.Debug.Log("Data saved successfully");
        }

        private static void AssignedToDesignatedQueueForRetrivingData<T>(BinaryData<T> binaryData) {

            if (typeof(T) == typeof(bool))
            {
                _listOfBooleanBinaryDataToBeRetrived.Enqueue((BinaryData<bool>)Convert.ChangeType(binaryData, typeof(BinaryData<bool>)));
            }
            else if (typeof(T) == typeof(int))
            {
                _listOfIntegerBinaryDataToBeRetrived.Enqueue((BinaryData<int>)Convert.ChangeType(binaryData, typeof(BinaryData<int>)));
            }
            else if (typeof(T) == typeof(float))
            {
                _listOfFloatBinaryDataToBeRetrived.Enqueue((BinaryData<float>)Convert.ChangeType(binaryData, typeof(BinaryData<float>)));
            }
            else if (typeof(T) == typeof(double))
            {
                _listOfDoubleBinaryDataToBeRetrived.Enqueue((BinaryData<double>)Convert.ChangeType(binaryData, typeof(BinaryData<double>)));
            }
            else if (typeof(T) == typeof(string))
            {
                _listOfStringBinaryDataToBeRetrived.Enqueue((BinaryData<string>)Convert.ChangeType(binaryData, typeof(BinaryData<string>)));

            }
        }

        private static void PassRetrivedData<T>(BinaryData<T> binaryData) {

            if (typeof(T) == typeof(bool))
            {
                bool value = false;

                if (!_binaryDataWrapper.boolValues.TryGetValue(binaryData.GetKey(), out value))
                    _binaryDataWrapper.boolValues.Add(binaryData.GetKey(), value);
                
                binaryData.SetData((T)Convert.ChangeType(value, typeof(T)));
            }
            else if (typeof(T) == typeof(int))
            {
                int value = 0;

                if(!_binaryDataWrapper.intValues.TryGetValue(binaryData.GetKey(), out value))
                    _binaryDataWrapper.intValues.Add(binaryData.GetKey(), value);

                binaryData.SetData((T)Convert.ChangeType(value, typeof(T)));
            }
            else if (typeof(T) == typeof(float))
            {
                float value = 0;

                if(!_binaryDataWrapper.floatValues.TryGetValue(binaryData.GetKey(), out value))
                    _binaryDataWrapper.floatValues.Add(binaryData.GetKey(), value);

                binaryData.SetData((T)Convert.ChangeType(value, typeof(T)));
            }
            else if (typeof(T) == typeof(double))
            {
                double value = 0;

                if(!_binaryDataWrapper.doubleValues.TryGetValue(binaryData.GetKey(), out value))
                    _binaryDataWrapper.doubleValues.Add(binaryData.GetKey(), value);

                binaryData.SetData((T)Convert.ChangeType(value, typeof(T)));
            }
            else if (typeof(T) == typeof(string))
            {
                string value = "";

                if(!_binaryDataWrapper.stringValues.TryGetValue(binaryData.GetKey(), out value))
                    _binaryDataWrapper.stringValues.Add(binaryData.GetKey(), value);

                binaryData.SetData((T)Convert.ChangeType(value, typeof(T)));
            }
        }

        #endregion

        #region Public Callback

        public static void GetInitialData<T>(BinaryData<T> binaryData) {

            if (!_isInitialDataLoaded)
            {

                if (!_isDataLoadingProcessOnGoing)
                {

                    _isDataLoadingProcessOnGoing = true;
                    SaveLoadOperation.LoadData<BinaryDataWrapper>(OnDataLoadFailed, OnDataLoadSucceed, _fileName, _fileExtension);
                }

                AssignedToDesignatedQueueForRetrivingData(binaryData);
            }
            else {

                PassRetrivedData(binaryData);
            }
        }

        public static void GetData<T>(BinaryData<T> binaryData){

            PassRetrivedData(binaryData);
        }

        public static void SetData<T>(BinaryData<T> binaryData) {


        }

        public static void TakeDataSnapshot() {

            SaveLoadOperation.SaveData(_binaryDataWrapper, OnDataSavedSucced, _fileName, _fileExtension);
        }

        #endregion
    }
}


