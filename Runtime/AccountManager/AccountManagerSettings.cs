namespace com.faith.core
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AccountManagerSettings", menuName = "FAITH/AccountManager/AccountManagerSettings", order = 1)]
    public class AccountManagerSettings : ScriptableObject
    {
        #region Custom Variables

        [Serializable]
        public class CurrecnyInfo
        {
#if UNITY_EDITOR
            public bool showOnEditor;
#endif

            public string currencyName = "DEFAULT";
            public Sprite currencyIcon;
            public double currencydefaultAmount = 0;

            [Range(0.1f, 2f)]
            public float currencyAnimationDuration = 0.1f;
            public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
        }

        #endregion

        #region Public Variables

        public List<CurrecnyInfo> listOfCurrencyInfos;

        #endregion

    }
}


