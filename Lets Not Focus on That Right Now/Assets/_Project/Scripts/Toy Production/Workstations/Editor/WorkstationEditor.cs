using UnityEditor;

[CustomEditor(typeof(Workstation))]
public class WorkstationEditor : Editor
{
    private SerializedProperty workstationType;

    private const string toyPartPropertyName = "toyPart";

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
            if (prop.name != "m_Script" && prop.name != nameof(workstationType) && prop.name != toyPartPropertyName)
            {
                EditorGUILayout.PropertyField(prop, true);
            }
            enterChildren = false;
        }

        if (myComponent.WorkstationType == Workstation.WorkstationCategory.Molder)
            EditorGUILayout.PropertyField(serializedObject.FindProperty(toyPartPropertyName));

        serializedObject.ApplyModifiedProperties();
    }
}