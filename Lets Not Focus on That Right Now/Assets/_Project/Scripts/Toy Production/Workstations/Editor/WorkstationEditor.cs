using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Workstation))]
public class WorkstationEditor : Editor
{
    private SerializedProperty m_Script;
    private SerializedProperty workstationType;
    private SerializedProperty materialType;
    private SerializedProperty toyPart;
    private SerializedProperty numToyParts;

    private void OnEnable()
    {
        m_Script = serializedObject.FindProperty(nameof(m_Script));
        workstationType = serializedObject.FindProperty(nameof(workstationType));
        materialType = serializedObject.FindProperty(nameof(materialType));
        toyPart = serializedObject.FindProperty(nameof(toyPart));
        numToyParts = serializedObject.FindProperty(nameof(numToyParts));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var workstation = (Workstation)target;

        var desiredProperties = new List<SerializedProperty>
        {
            m_Script,
            workstationType,
        };

        switch (workstation.WorkstationType)
        {
            case Workstation.WorkstationCategory.Dispenser:
                desiredProperties.Add(materialType);
                break;
            case Workstation.WorkstationCategory.Processor:
                desiredProperties.Add(materialType);
                break;
            case Workstation.WorkstationCategory.Molder:
                desiredProperties.Add(toyPart);
                break;
            case Workstation.WorkstationCategory.Assembler:
                desiredProperties.Add(numToyParts);
                break;
            case Workstation.WorkstationCategory.Output:
                break;
        }
        
        desiredProperties.ForEach(p => EditorGUILayout.PropertyField(p, true));
        serializedObject.ApplyModifiedProperties();
    }
}