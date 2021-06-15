using System.Collections;
using UnityEngine;
using com.faith.core;

public class BatchUpdateRepeatedTest : MonoBehaviour, IBatchedUpdateHandler
{
    private int counter;

    public void OnBatchedUpdate()
    {
        
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        WaitForSeconds cycleDelay = new WaitForSeconds(1f);

        yield return cycleDelay;

        while (true) {

            BatchedUpdate.Instance.RegisterToBatchedUpdate(this, 1);
            Debug.Log(string.Format("Registered = {0}", ++counter));
            yield return cycleDelay;
            BatchedUpdate.Instance.UnregisterFromBatchedUpdate(this);
            Debug.Log(string.Format("Unregistered = {0}", counter));
            yield return cycleDelay;
            
        }
    }

    
}
