using UnityEngine;
using UnityEditor;
using com.faith.core;

[CustomEditor(typeof(BatchedUpdate))]
public class BatchedUpdateEditor : BaseEditorClass
{

    #region Private Variables

    private BatchedUpdate _reference;

    #endregion

    #region Custom GUI

    private void InstanceViwerGUI() {

        for (int i = 0; i < _reference.NumberOfInstances; i++) {

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField(string.Format("BatchedUpdateInstance({0})", i));

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
