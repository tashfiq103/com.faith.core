using System.Collections;
using UnityEngine;
using com.faith.core;
public class CentralDelayTest : MonoBehaviour
{
    #region Public Variables

    public RangeReference numberOfInstances;
    public RangeReference initialDelay;
    public RangeReference delay;

    #endregion

    #region Mono Behaviour

    private IEnumerator Start()
    {
        int instances = (int) numberOfInstances;
        for (int i = 0; i < instances; i++) {

            yield return new WaitForSeconds(initialDelay);
            CentralDelay.Instance.SetDelay(delay);
        }
    }

    #endregion
}
