using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.faith.core;

public class BatchedUpdateTest : MonoBehaviour
{
    #region CustomVariables

    public class BatchedUpdateTestClass :  MonoBehaviour, IBatchedUpdateHandler
    {
        public float TimeFrame { get; private set; }


        public void Initialize(float timeFrame, int interval) {

            TimeFrame = timeFrame;
            BatchedUpdate.Instance.RegisterToBatchedUpdate(this, interval);
        }

        public void OnBatchedUpdate()
        {
            TimeFrame -= Time.deltaTime;
            if (TimeFrame <= 0)
                BatchedUpdate.Instance.UnregisterFromBatchedUpdate(this);
        }
    }

    #endregion

    #region Public Variables

    public int numberOfTestClass = 1;
    public RangeReference timeFrames;
    public RangeReference framesVariation;


    #endregion

    #region Private Variables

    private List<BatchedUpdateTestClass> _listOfTestClass;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameObject blueprint = new GameObject();
        for(int i = 0; i < numberOfTestClass; i++)
        {
            GameObject newTestInstance = Instantiate(blueprint, transform);
            newTestInstance.name = string.Format("BatchUpdateTestInstance({0})", i);


            BatchedUpdateTestClass reference = newTestInstance.AddComponent<BatchedUpdateTestClass>();
            reference.Initialize(timeFrames.Value, (int)framesVariation);
        }
    }

}
