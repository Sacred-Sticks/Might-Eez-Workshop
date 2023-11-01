using UnityEditor;
using UnityEditor.Rendering;

[CustomEditor(typeof(Workstation))]
public class WorkstationEditor : Editor
{
    private SerializedProperty workstationType;

    private const string toyPartPropertyName = "toyPart";
    private const string numToyPartsPropertyName = "numToyParts";

    private void OnEnable()
    {
        workstationType = serializedObject.FindProperty(nameof(workstationType));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(workstationType);

        var myComponent = (Workstation)target;

        var prop = serializedObject.GetIterator();
        bool enterChildren = true;
        while (prop.NextVisible(enterChildren))
        {
            if (prop.name != "m_Script" && prop.name != nameof(workstationType) && prop.name != toyPartPropertyName && prop.name != numToyPartsPropertyName)
            {
                EditorGUILayout.PropertyField(prop, true);
            }
            enterChildren = false;
        }

        switch (myComponent.WorkstationType)
        {
            case Workstation.WorkstationCategory.Molder:
                EditorGUILayout.PropertyField(serializedObject.FindProperty(toyPartPropertyName));
                break;
            case Workstation.WorkstationCategory.Assembler:
                EditorGUILayout.PropertyField(serializedObject.FindProperty(numToyPartsPropertyName));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}