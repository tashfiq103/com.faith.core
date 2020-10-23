namespace com.faith.core
{
    using UnityEditor;

    [CustomEditor(typeof(TransformAnimatorAsset))]
    public class TransformAnimatorAssetEditor : BaseEditorClass
    {
        #region Private Variables

        private TransformAnimatorAsset _reference;

        private SerializedProperty _sp_durationOfAnimation;

        //AnimationParameter    :   Position

        private SerializedProperty _sp_transitionTypeForPosition;
        private SerializedProperty _sp_animatedLocalPosition;

        //AnimationParameter    :   Rotation

        private SerializedProperty _sp_animatedLocalEulerAngle;

        //AnimationParameter    :   Scaling

        private SerializedProperty _sp_initialScalePoint;
        private SerializedProperty _sp_initialLocalScale;
        private SerializedProperty _sp_animatedLocalScale;


        #endregion

        #region Editor

        public override void OnEnable()
        {
            _reference = (TransformAnimatorAsset)target;

            if (_reference == null)
                return;

            _sp_durationOfAnimation = serializedObject.FindProperty("durationOfAnimation");

            //Position
            _sp_transitionTypeForPosition = serializedObject.FindProperty("transitionTypeForPosition");
            _sp_animatedLocalPosition = serializedObject.FindProperty("animatedLocalPosition");

            //Rotation
            _sp_animatedLocalEulerAngle = serializedObject.FindProperty("animatedLocalEulerAngle");

            //Scaling
            _sp_initialScalePoint = serializedObject.FindProperty("initialScalePoint");
            _sp_initialLocalScale = serializedObject.FindProperty("initialLocalScale");
            _sp_animatedLocalScale = serializedObject.FindProperty("animatedLocalScale");
        }

        public override void OnInspectorGUI()
        {

            ShowScriptReference();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_sp_durationOfAnimation);


            if (_reference.showAnimationProperty)
            {

                DrawHorizontalLine();

                _reference.showAnimationProperty = EditorGUILayout.Foldout(
                    _reference.showAnimationProperty,
                    "Animation Property",
                    true
                );

                EditorGUI.indentLevel += 1;

                PositionGUI();

                RotationGUI();

                ScalingGUI();

                EditorGUI.indentLevel -= 1;
            }
            else
            {

                _reference.showAnimationProperty = EditorGUILayout.Foldout(
                    _reference.showAnimationProperty,
                    "Animation Property",
                    true
                );

            }


            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region CustomGUI

        private void PositionGUI()
        {

            //Position
            _reference.isPositionAnimationEnabled = EditorGUILayout.Foldout(
                        _reference.isPositionAnimationEnabled,
                        (_reference.isPositionAnimationEnabled ? "[Enabled]" : "[Disabled]") + " : Position",
                        true
                    );

            if (_reference.isPositionAnimationEnabled)
            {

                EditorGUILayout.BeginVertical();
                {
                    EditorGUI.indentLevel += 1;

                    EditorGUILayout.PropertyField(_sp_transitionTypeForPosition);
                    EditorGUILayout.PropertyField(_sp_animatedLocalPosition);

                    EditorGUILayout.Space();
                    _reference.axisTypeForPositionAnimation = (CoreEnums.AxisType)EditorGUILayout.EnumPopup(
                            "Curve Mode",
                            _reference.axisTypeForPositionAnimation
                        );


                    {
                        EditorGUI.indentLevel += 1;
                        switch (_reference.axisTypeForPositionAnimation)
                        {

                            case CoreEnums.AxisType.OneForAll:

                                _reference.curveForPositionAnimation = EditorGUILayout.CurveField("Curve :(x,y,z)", _reference.curveForPositionAnimation);

                                break;

                            case CoreEnums.AxisType.Seperate:

                                _reference.curveForPositionAnimationOnX = EditorGUILayout.CurveField("Curve : (x)", _reference.curveForPositionAnimationOnX);
                                _reference.curveForPositionAnimationOnY = EditorGUILayout.CurveField("Curve : (y)", _reference.curveForPositionAnimationOnY);
                                _reference.curveForPositionAnimationOnZ = EditorGUILayout.CurveField("Curve : (z)", _reference.curveForPositionAnimationOnZ);

                                break;
                        }
                        EditorGUI.indentLevel -= 1;
                    }

                    EditorGUI.indentLevel -= 1;
                }
                EditorGUILayout.EndVertical();
                DrawHorizontalLine();
            }

        }

        private void RotationGUI()
        {

            _reference.isRotationAnimationEnabled = EditorGUILayout.Foldout(
                            _reference.isRotationAnimationEnabled,
                            (_reference.isRotationAnimationEnabled ? "[Enabled]" : "[Disabled]") + " : Rotation",
                            true
                        );

            if (_reference.isRotationAnimationEnabled)
            {

                EditorGUILayout.BeginVertical();
                {
                    EditorGUI.indentLevel += 1;

                    EditorGUILayout.PropertyField(_sp_animatedLocalEulerAngle);

                    EditorGUILayout.Space();
                    _reference.axisTypeForRotationAnimation = (CoreEnums.AxisType)EditorGUILayout.EnumPopup(
                            "Curve Mode",
                            _reference.axisTypeForRotationAnimation
                        );


                    {
                        EditorGUI.indentLevel += 1;
                        switch (_reference.axisTypeForRotationAnimation)
                        {

                            case CoreEnums.AxisType.OneForAll:

                                _reference.curveForLocalEulerAngleAnimation = EditorGUILayout.CurveField("Curve :(x,y,z)", _reference.curveForLocalEulerAngleAnimation);

                                break;

                            case CoreEnums.AxisType.Seperate:

                                _reference.curveForLocalEulerAngleAnimationOnX = EditorGUILayout.CurveField("Curve : (x)", _reference.curveForLocalEulerAngleAnimationOnX);
                                _reference.curveForLocalEulerAngleAnimationOnY = EditorGUILayout.CurveField("Curve : (y)", _reference.curveForLocalEulerAngleAnimationOnY);
                                _reference.curveForLocalEulerAngleAnimationOnZ = EditorGUILayout.CurveField("Curve : (z)", _reference.curveForLocalEulerAngleAnimationOnZ);

                                break;
                        }
                        EditorGUI.indentLevel -= 1;
                    }

                    EditorGUI.indentLevel -= 1;
                }
                EditorGUILayout.EndVertical();
                DrawHorizontalLine();
            }

        }

        private void ScalingGUI()
        {

            _reference.isScalingAnimationEnabled = EditorGUILayout.Foldout(
                            _reference.isScalingAnimationEnabled,
                            (_reference.isScalingAnimationEnabled ? "[Enabled]" : "[Disabled]") + " : Scaling",
                            true
                        );

            if (_reference.isScalingAnimationEnabled)
            {

                EditorGUILayout.BeginVertical();
                {
                    EditorGUI.indentLevel += 1;

                    EditorGUILayout.PropertyField(_sp_initialScalePoint);

                    switch (_reference.initialScalePoint)
                    {

                        case CoreEnums.InitialScalePoint.Current:

                            break;
                        case CoreEnums.InitialScalePoint.New:
                            EditorGUILayout.PropertyField(_sp_initialLocalScale);
                            break;
                    }

                    EditorGUILayout.PropertyField(_sp_animatedLocalScale);

                    EditorGUILayout.Space();
                    _reference.axisTypeForScalingAnimation = (CoreEnums.AxisType)EditorGUILayout.EnumPopup(
                            "Curve Mode",
                            _reference.axisTypeForScalingAnimation
                        );


                    {
                        EditorGUI.indentLevel += 1;
                        switch (_reference.axisTypeForScalingAnimation)
                        {

                            case CoreEnums.AxisType.OneForAll:

                                _reference.curveForLocalScaleAnimation = EditorGUILayout.CurveField("Curve :(x,y,z)", _reference.curveForLocalScaleAnimation);

                                break;

                            case CoreEnums.AxisType.Seperate:

                                _reference.curveForLocalScaleAnimationOnX = EditorGUILayout.CurveField("Curve : (x)", _reference.curveForLocalScaleAnimationOnX);
                                _reference.curveForLocalScaleAnimationOnY = EditorGUILayout.CurveField("Curve : (y)", _reference.curveForLocalScaleAnimationOnY);
                                _reference.curveForLocalScaleAnimationOnZ = EditorGUILayout.CurveField("Curve : (z)", _reference.curveForLocalScaleAnimationOnZ);

                                break;
                        }
                        EditorGUI.indentLevel -= 1;
                    }

                    EditorGUI.indentLevel -= 1;
                }
                EditorGUILayout.EndVertical();
                DrawHorizontalLine();
            }
        }

        #endregion
    }

}
