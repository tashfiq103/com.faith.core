namespace com.faith.core
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    [CreateAssetMenu(
        fileName = "TransformAnimatorAsset",
        menuName = ScriptableObjectAssetMenu.MENU_VISUAL_TRANSFORM_ANIMATOR,
        order = ScriptableObjectAssetMenu.ORDER_VISUAL_TRANSFORM_ANIMATOR)]
    public class TransformAnimatorAsset : ScriptableObject
    {

        #region Custom Varaibles

        internal class ControllerForDynamicAnimation
        {

            #region Private Variables

            private CancellationTokenSource _cancellationTokenSource;

            #endregion

            #region Configuretion

            private bool IsAnyOfTransformIsDestroyed()
            {

                foreach (Transform transformReference in GetAnimatedTransform)
                {

                    if (transformReference == null)
                    {
                        _cancellationTokenSource.Cancel();
                        return true;
                    }
                }

                return false;
            }

            private async void DoAnimation(
                TransformAnimatorAsset animationParameterReference,
                UnityAction<ControllerForDynamicAnimation, UnityAction> OnRemovingAnimation,
                UnityAction OnAnimationEnd)
            {

                int cycleLength = 17;
                int numberOfTransform = GetAnimatedTransform.Count;
                float durationOfAnimationInMS = animationParameterReference.durationOfAnimation * 1000;
                float remainingTime = durationOfAnimationInMS;
                float progression = 0;

                Vector3[] initialPosition = new Vector3[numberOfTransform];
                Vector3[] animatedPosition = new Vector3[numberOfTransform];

                Vector3[] initialEulerAngle = new Vector3[numberOfTransform];
                Vector3[] animatedEulerAngle = new Vector3[numberOfTransform];

                Vector3[] initialLocalScale = new Vector3[numberOfTransform];

                for (int i = 0; i < numberOfTransform; i++)
                {

                    initialPosition[i] = GetAnimatedTransform[i].localPosition;
                    animatedPosition[i] = initialPosition[i] + animationParameterReference.animatedLocalPosition;

                    initialEulerAngle[i] = GetAnimatedTransform[i].localEulerAngles;
                    animatedEulerAngle[i] = initialEulerAngle[i] + animationParameterReference.animatedLocalEulerAngle;

                    switch (animationParameterReference.initialScalePoint)
                    {

                        case CoreEnums.InitialScalePoint.Current:
                            initialLocalScale[i] = GetAnimatedTransform[i].localScale;
                            break;

                        case CoreEnums.InitialScalePoint.New:
                            initialLocalScale[i] = animationParameterReference.initialLocalScale;
                            break;
                    }


                }

                while (remainingTime > 0)
                {

                    if (IsAnyOfTransformIsDestroyed()) break;

                    progression = 1f - (remainingTime / durationOfAnimationInMS);

                    //Animation :   Position
                    if (animationParameterReference.isPositionAnimationEnabled)
                    {
                        switch (animationParameterReference.axisTypeForPositionAnimation)
                        {

                            case CoreEnums.AxisType.OneForAll:

                                for (int i = 0; i < numberOfTransform; i++)
                                {

                                    Vector3 modifiedPosition = Vector3.Lerp(
                                        animationParameterReference.transitionTypeForPosition == CoreEnums.TransitionType.TransitionIn ? animatedPosition[i] : initialPosition[i],
                                        animationParameterReference.transitionTypeForPosition == CoreEnums.TransitionType.TransitionIn ? initialPosition[i] : animatedPosition[i],
                                        animationParameterReference.curveForPositionAnimation.Evaluate(progression)
                                    );

                                    GetAnimatedTransform[i].localPosition = modifiedPosition;
                                }

                                break;

                            case CoreEnums.AxisType.Seperate:

                                for (int i = 0; i < numberOfTransform; i++)
                                {
                                    Vector3 modifiedPosition = new Vector3(
                                        animationParameterReference.transitionTypeForPosition == CoreEnums.TransitionType.TransitionIn ? animatedPosition[i].x : initialPosition[i].x * animationParameterReference.curveForPositionAnimationOnX.Evaluate(progression),
                                        animationParameterReference.transitionTypeForPosition == CoreEnums.TransitionType.TransitionIn ? animatedPosition[i].y : initialPosition[i].y * animationParameterReference.curveForPositionAnimationOnY.Evaluate(progression),
                                        animationParameterReference.transitionTypeForPosition == CoreEnums.TransitionType.TransitionIn ? animatedPosition[i].z : initialPosition[i].z * animationParameterReference.curveForPositionAnimationOnZ.Evaluate(progression)
                                    );

                                    GetAnimatedTransform[i].localPosition = modifiedPosition;
                                }

                                break;
                        }

                    }

                    //Animation :   LocalEuler
                    if (animationParameterReference.isRotationAnimationEnabled)
                    {
                        switch (animationParameterReference.axisTypeForRotationAnimation)
                        {

                            case CoreEnums.AxisType.OneForAll:

                                for (int i = 0; i < numberOfTransform; i++)
                                {

                                    Vector3 modifiedEulerAngle = Vector3.Lerp(
                                            initialEulerAngle[i],
                                            animatedEulerAngle[i],
                                            animationParameterReference.curveForLocalEulerAngleAnimation.Evaluate(progression)
                                        );
                                    GetAnimatedTransform[i].localEulerAngles = modifiedEulerAngle;
                                }

                                break;

                            case CoreEnums.AxisType.Seperate:

                                for (int i = 0; i < numberOfTransform; i++)
                                {

                                    Vector3 modifiedEulerAngle = new Vector3(
                                            animatedEulerAngle[i].x * animationParameterReference.curveForLocalEulerAngleAnimationOnX.Evaluate(progression),
                                            animatedEulerAngle[i].y * animationParameterReference.curveForLocalEulerAngleAnimationOnY.Evaluate(progression),
                                            animatedEulerAngle[i].z * animationParameterReference.curveForLocalEulerAngleAnimationOnZ.Evaluate(progression)
                                        );
                                    GetAnimatedTransform[i].localEulerAngles = modifiedEulerAngle;
                                }

                                break;
                        }

                    }

                    //Animation :   LocalScale
                    if (animationParameterReference.isScalingAnimationEnabled)
                    {
                        switch (animationParameterReference.axisTypeForScalingAnimation)
                        {

                            case CoreEnums.AxisType.OneForAll:

                                for (int i = 0; i < numberOfTransform; i++)
                                {

                                    Vector3 modifiedLocalScale = Vector3.Lerp(
                                            initialLocalScale[i],
                                            animationParameterReference.animatedLocalScale,
                                            animationParameterReference.curveForLocalScaleAnimation.Evaluate(progression)
                                        );
                                    GetAnimatedTransform[i].localScale = modifiedLocalScale;
                                }

                                break;

                            case CoreEnums.AxisType.Seperate:

                                for (int i = 0; i < numberOfTransform; i++)
                                {

                                    Vector3 modifiedEulerAngle = new Vector3(
                                            Mathf.Lerp(initialLocalScale[i].x, animationParameterReference.animatedLocalScale.x, animationParameterReference.curveForLocalScaleAnimationOnX.Evaluate(progression)),
                                            Mathf.Lerp(initialLocalScale[i].y, animationParameterReference.animatedLocalScale.y, animationParameterReference.curveForLocalScaleAnimationOnX.Evaluate(progression)),
                                            Mathf.Lerp(initialLocalScale[i].z, animationParameterReference.animatedLocalScale.z, animationParameterReference.curveForLocalScaleAnimationOnX.Evaluate(progression))
                                        );
                                    GetAnimatedTransform[i].localScale = modifiedEulerAngle;
                                }

                                break;
                        }

                    }

                    await Task.Delay(cycleLength);
                    remainingTime -= cycleLength;
                }

                //Snap
                if (!_cancellationTokenSource.IsCancellationRequested)
                {

                    for (int i = 0; i < numberOfTransform; i++)
                    {

                        if (animationParameterReference.isPositionAnimationEnabled)
                            GetAnimatedTransform[i].position = initialPosition[i];

                        if (animationParameterReference.isRotationAnimationEnabled)
                        {

                            switch (animationParameterReference.axisTypeForRotationAnimation)
                            {

                                case CoreEnums.AxisType.OneForAll:

                                    GetAnimatedTransform[i].localEulerAngles = Vector3.Lerp(
                                                initialEulerAngle[i],
                                                animatedEulerAngle[i],
                                                animationParameterReference.curveForLocalEulerAngleAnimation.Evaluate(1)
                                            );
                                    break;
                                case CoreEnums.AxisType.Seperate:

                                    GetAnimatedTransform[i].localEulerAngles = new Vector3(
                                                animatedEulerAngle[i].x * animationParameterReference.curveForLocalEulerAngleAnimationOnX.Evaluate(1),
                                                animatedEulerAngle[i].y * animationParameterReference.curveForLocalEulerAngleAnimationOnY.Evaluate(1),
                                                animatedEulerAngle[i].z * animationParameterReference.curveForLocalEulerAngleAnimationOnZ.Evaluate(1)
                                            );

                                    break;
                            }
                        }

                        if (animationParameterReference.isScalingAnimationEnabled)
                        {

                            switch (animationParameterReference.axisTypeForScalingAnimation)
                            {

                                case CoreEnums.AxisType.OneForAll:

                                    GetAnimatedTransform[i].localScale = Vector3.Lerp(
                                                initialLocalScale[i],
                                                animationParameterReference.animatedLocalScale,
                                                animationParameterReference.curveForLocalScaleAnimation.Evaluate(1)
                                            );
                                    break;
                                case CoreEnums.AxisType.Seperate:

                                    GetAnimatedTransform[i].localScale = new Vector3(
                                                Mathf.Lerp(initialLocalScale[i].x, animationParameterReference.animatedLocalScale.x, animationParameterReference.curveForLocalScaleAnimationOnX.Evaluate(1)),
                                                Mathf.Lerp(initialLocalScale[i].y, animationParameterReference.animatedLocalScale.y, animationParameterReference.curveForLocalScaleAnimationOnX.Evaluate(1)),
                                                Mathf.Lerp(initialLocalScale[i].z, animationParameterReference.animatedLocalScale.z, animationParameterReference.curveForLocalScaleAnimationOnX.Evaluate(1))
                                            );

                                    break;
                            }
                        }


                    }
                    OnRemovingAnimation?.Invoke(this, OnAnimationEnd);
                }
                else
                {

                    _cancellationTokenSource.Cancel();
                }

            }

            #endregion

            #region Public Callback

            public List<Transform> GetAnimatedTransform { get; private set; }

            public ControllerForDynamicAnimation(
                TransformAnimatorAsset animationParameterReference,
                List<Transform> listOfTransform,
                UnityAction<ControllerForDynamicAnimation, UnityAction> OnRemovingAnimation,
                ref UnityAction OnAnimationEnd)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                GetAnimatedTransform = new List<Transform>(listOfTransform);
                DoAnimation(animationParameterReference, OnRemovingAnimation, OnAnimationEnd);
            }

            #endregion

        }

        #endregion


        #region Public Variables

#if UNITY_EDITOR

        public bool showAnimationProperty;


#endif

        public bool isPositionAnimationEnabled;
        public bool isRotationAnimationEnabled;
        public bool isScalingAnimationEnabled;

        [Range(0.1f, 5f)]
        public float durationOfAnimation = 1;

        //AnimationParameter    :   Position
        public CoreEnums.AxisType axisTypeForPositionAnimation;

        public CoreEnums.TransitionType transitionTypeForPosition;
        public Vector3 animatedLocalPosition;
        public AnimationCurve curveForPositionAnimation;

        public AnimationCurve curveForPositionAnimationOnX;
        public AnimationCurve curveForPositionAnimationOnY;
        public AnimationCurve curveForPositionAnimationOnZ;

        //AnimationParameter    :   Rotation
        public CoreEnums.AxisType axisTypeForRotationAnimation;

        public Vector3 animatedLocalEulerAngle;
        public AnimationCurve curveForLocalEulerAngleAnimation;

        public AnimationCurve curveForLocalEulerAngleAnimationOnX;
        public AnimationCurve curveForLocalEulerAngleAnimationOnY;
        public AnimationCurve curveForLocalEulerAngleAnimationOnZ;

        //AnimationParameter    :   Scaling
        public CoreEnums.AxisType axisTypeForScalingAnimation;

        public CoreEnums.InitialScalePoint initialScalePoint;
        public Vector3 initialLocalScale;
        public Vector3 animatedLocalScale;
        public AnimationCurve curveForLocalScaleAnimation;

        public AnimationCurve curveForLocalScaleAnimationOnX;
        public AnimationCurve curveForLocalScaleAnimationOnY;
        public AnimationCurve curveForLocalScaleAnimationOnZ;

        #endregion

        #region Private Variables

        private List<ControllerForDynamicAnimation> _listOfDynamicAnimation = new List<ControllerForDynamicAnimation>();

        #endregion

        #region Configuretion

        private bool IsAnyTransformAlreadyAnimating(ref List<Transform> listOfTransforms)
        {

            int numberOfDynamicAnimation = _listOfDynamicAnimation.Count;
            for (int i = 0; i < numberOfDynamicAnimation; i++)
            {

                foreach (Transform transformReference in listOfTransforms)
                {

                    if (_listOfDynamicAnimation[i].GetAnimatedTransform.Contains(transformReference))
                    {

                        return true;
                    }
                }
            }

            return false;
        }

        private void OnRemovingAnimationFromTheList(ControllerForDynamicAnimation dynamicAnimation, UnityAction OnAnimationEnd)
        {

            _listOfDynamicAnimation.Remove(dynamicAnimation);
            OnAnimationEnd?.Invoke();
        }

        #endregion

        #region Public Callback

        public void DoAnimate(List<Transform> listOfTransforms, UnityAction OnAnimationEnd = null)
        {

            if (!IsAnyTransformAlreadyAnimating(ref listOfTransforms))
            {

                _listOfDynamicAnimation.Add(new ControllerForDynamicAnimation(this, listOfTransforms, OnRemovingAnimationFromTheList, ref OnAnimationEnd));
            }
            else
            {

                CoreDebugger.Debug.LogWarning("One of the 'transform' is busy with another animation. Failed To Execute the requested animation");
            }
        }

        #endregion
    }

}
