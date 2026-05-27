
using UnityEngine;
using System.Text;
using UnityEditor;


public static class CopyHierarchyToClipboard
{
    [MenuItem("Tools/Copy Selected Hierarchy To Clipboard %#c")] // Ctrl/Cmd + Shift + C
    public static void CopySelectedHierarchy()
    {
        var go = Selection.activeGameObject;
        if (go == null)
        {
            Debug.LogWarning("No GameObject selected.");
            return;
        }

        var sb = new StringBuilder();
        BuildHierarchy(go.transform, sb, 0);
        EditorGUIUtility.systemCopyBuffer = sb.ToString();
        Debug.Log("Hierarchy copied to clipboard:\n" + sb.ToString());
    }

    private static void BuildHierarchy(Transform t, StringBuilder sb, int depth)
    {
        sb.Append(' ', depth * 2);
        sb.Append(t.name);
        sb.Append(" (pos: ");
        var p = t.localPosition;
        sb.AppendFormat("{0:0.###},{1:0.###},{2:0.###}", p.x, p.y, p.z);
        sb.Append("; rot: ");
        var r = t.localEulerAngles;
        sb.AppendFormat("{0:0.###},{1:0.###},{2:0.###}", r.x, r.y, r.z);
        sb.Append("; scale: ");
        var s = t.localScale;
        sb.AppendFormat("{0:0.###},{1:0.###},{2:0.###}", s.x, s.y, s.z);
        sb.AppendLine(")");

        for (int i = 0; i < t.childCount; i++)
            BuildHierarchy(t.GetChild(i), sb, depth + 1);
    }
}
