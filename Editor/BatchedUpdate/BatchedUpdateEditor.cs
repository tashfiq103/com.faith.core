using UnityEngine;
using UnityEditor;
using com.faith.core;
using System.Collections;

[CustomEditor(typeof(BatchedUpdate))]
public class BatchedUpdateEditor : BaseEditorClass
{

    #region Private Variables

    private BatchedUpdate _reference;

    private SerializedProperty showTracker;

    #endregion

    #region Custom GUI

    private void TrackerView() {

        showTracker.boolValue = EditorGUILayout.Foldout(
                showTracker.boolValue,
                showTracker.name
            );

        if (showTracker.boolValue) {

            int numberOfItem = _reference.BatchUpdateHandlerTracker.Count;

            IBatchedUpdateHandler[] batchedUpdateHandlers = new IBatchedUpdateHandler[numberOfItem];
            ICollection keys = _reference.BatchUpdateHandlerTracker.Keys;
            keys.CopyTo(batchedUpdateHandlers, 0);

            BatchedUpdate.UpdateInfo[] updateInfos = new BatchedUpdate.UpdateInfo[numberOfItem];
            ICollection values = _reference.BatchUpdateHandlerTracker.Values;
            values.CopyTo(updateInfos, 0);

            EditorGUI.indentLevel += 1;

            for (int i = 0; i < numberOfItem; i++) {

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField(string.Format("Element({0})", i), EditorStyles.boldLabel);

                    EditorGUI.indentLevel += 1;

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(string.Format("Key : {0}", batchedUpdateHandlers[i]));
                        EditorGUILayout.LabelField(string.Format("Value : {0}", batchedUpdateHandlers[i]));
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.indentLevel -= 1;
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUI.indentLevel -= 1;
        }

    }

    private void InstanceViwerGUI() {

        for (int i = 0; i < _reference.NumberOfInstances; i++) {

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField(string.Format("BatchedUpdateInstance({0}) : Interval({1})", i, _reference.BatchUpdateInstances[i].Interval));

                EditorGUI.indentLevel += 1;
                {
                    for (int j = 0; j < _reference.BatchUpdateInstances[i].NumberOfActiveBucket; j++)
                        BucketViwerGUI(i, j);

                }
                EditorGUI.indentLevel -= 1;
                
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void BucketViwerGUI(int instanceIndex, int bucketIndex) {

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(string.Format("Bucket({0}) : Update({1})", bucketIndex, _reference.BatchUpdateInstances[instanceIndex].BatchUpdateBuckets[bucketIndex].NumberOfBatchedUpdateHandlerInBucket));
        }
        EditorGUILayout.EndHorizontal();
    }



    #endregion

    #region Editor

    public override void OnEnable()
    {
        base.OnEnable();

        _reference = (BatchedUpdate)target;

        if (_reference == null)
            return;

        showTracker = serializedObject.FindProperty("showTracker");
    }

    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();

        serializedObject.Update();

        CoreEditorModule.DrawHorizontalLine();

        InstanceViwerGUI();

        serializedObject.ApplyModifiedProperties();

    }

    #endregion

}
