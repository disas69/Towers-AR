using Framework.Editor;
using Game.Gameplay.RingStructure.Configuration;
using UnityEditor;
using UnityEngine;

namespace Game.Gameplay.RingStructure.Editor
{
    [CustomEditor(typeof(RingsStorage))]
    public class RingsStorageEditor : CustomEditorBase<RingsStorage>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.LabelField("Rings Prefabs", HeaderStyle);
            if (GUILayout.Button("Add"))
            {
                RecordObject("Rings Storage Change");
                Target.RingsPrefabs.Add(null);
            }

            var list = serializedObject.FindProperty("RingsPrefabs");
            var count = list.arraySize;

            if (count > 0)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    for (int i = 0; i < count; i++)
                    {
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            var element = list.GetArrayElementAtIndex(i);
                            var elementName = element.objectReferenceValue != null
                                ? element.objectReferenceValue.name
                                : "None";

                            EditorGUILayout.PropertyField(element, new GUIContent(elementName));
                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                RecordObject("Rings Storage Change");
                                Target.RingsPrefabs.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
    }
}