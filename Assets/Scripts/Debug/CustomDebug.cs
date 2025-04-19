using UnityEngine;

public static class CustomDebug
{
    /// <summary>
    /// Draws a wireframe box in the Scene view.
    /// </summary>
    /// <param name="center">Center of the box</param>
    /// <param name="size">Half-extents (size from center to edge)</param>
    /// <param name="rotation">Orientation of the box</param>
    /// <param name="color">Color of the debug lines</param>
    /// <param name="duration">How long the lines should remain visible</param>
    public static void DebugBox(Vector3 center, Vector3 size, Quaternion rotation, Color color, float duration = 0)
    {
        // Calculate box edges
        Vector3[] edges = GetBoxEdges(center, size, rotation);

        // Draw the 12 lines of the box
        Debug.DrawLine(edges[0], edges[1], color, duration);
        Debug.DrawLine(edges[1], edges[2], color, duration);
        Debug.DrawLine(edges[2], edges[3], color, duration);
        Debug.DrawLine(edges[3], edges[0], color, duration);

        Debug.DrawLine(edges[4], edges[5], color, duration);
        Debug.DrawLine(edges[5], edges[6], color, duration);
        Debug.DrawLine(edges[6], edges[7], color, duration);
        Debug.DrawLine(edges[7], edges[4], color, duration);

        Debug.DrawLine(edges[0], edges[4], color, duration);
        Debug.DrawLine(edges[1], edges[5], color, duration);
        Debug.DrawLine(edges[2], edges[6], color, duration);
        Debug.DrawLine(edges[3], edges[7], color, duration);
    }

    private static Vector3[] GetBoxEdges(Vector3 center, Vector3 size, Quaternion rotation)
    {
        Vector3[] edges = new Vector3[8];
        
        // Calculate the 8 corners of the box
        edges[0] = center + rotation * new Vector3(-size.x, -size.y, -size.z); // Bottom-back-left
        edges[1] = center + rotation * new Vector3(size.x, -size.y, -size.z);  // Bottom-back-right
        edges[2] = center + rotation * new Vector3(size.x, -size.y, size.z);   // Bottom-front-right
        edges[3] = center + rotation * new Vector3(-size.x, -size.y, size.z);  // Bottom-front-left
        
        edges[4] = center + rotation * new Vector3(-size.x, size.y, -size.z);  // Top-back-left
        edges[5] = center + rotation * new Vector3(size.x, size.y, -size.z);   // Top-back-right
        edges[6] = center + rotation * new Vector3(size.x, size.y, size.z);    // Top-front-right
        edges[7] = center + rotation * new Vector3(-size.x, size.y, size.z);   // Top-front-left

        return edges;
    }
}