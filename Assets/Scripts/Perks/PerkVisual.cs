using UnityEngine;
using TMPro; // Add this namespace for TextMesh Pro

public class PerkVisual : MonoBehaviour
{
    public Perk perk;                     // Reference to the Perk script
    public GameObject shape;              // Shape GameObject
    public TMP_Text symbolText;           // TextMesh Pro Text for the symbol

    private void Start()
    {
        // Set the shape based on the perk's shape type
        SetShape(perk.perkData.shapeType, perk.perkData.perkName);
        // Set the symbol text based on the perk's name or description
        SetSymbol(perk.perkData.perkName);
    }

private void SetShape(ShapeType shapeType, string symbolText)
{
    // Ensure the shape GameObject has a MeshFilter and MeshRenderer
    MeshFilter meshFilter = shape.GetComponent<MeshFilter>();
    MeshRenderer meshRenderer = shape.GetComponent<MeshRenderer>();

    if (meshFilter == null)
    {
        meshFilter = shape.AddComponent<MeshFilter>(); // Add if not already attached
    }

    if (meshRenderer == null)
    {
        meshRenderer = shape.AddComponent<MeshRenderer>(); // Add if not already attached
        meshRenderer.material = new Material(Shader.Find("Standard")); // Ensure material exists
    }

    // Create or get the TextMeshPro component
    TextMeshPro textMesh = shape.GetComponentInChildren<TextMeshPro>();
    if (textMesh == null)
    {
        GameObject textObj = new GameObject("SymbolText");
        textObj.transform.SetParent(shape.transform);  // Make it a child of the shape
        textMesh = textObj.AddComponent<TextMeshPro>();

        // Set alignment to center
        textMesh.alignment = TextAlignmentOptions.Center;
    }

    // Assign the symbol text
    textMesh.text = symbolText;
    textMesh.enableAutoSizing = true;
    textMesh.fontSizeMin = 0.1f;
    textMesh.fontSizeMax = 10f;

    // Center the text within the shape
    RectTransform rectTransform = textMesh.rectTransform;
    rectTransform.localPosition = Vector3.zero;
    rectTransform.sizeDelta = new Vector2(1, 1);
    rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    // Set different shapes and materials based on the shape type
switch (shapeType)
{
    case ShapeType.Triangle:
        meshRenderer.material.color = Color.red;
        shape.transform.localScale = new Vector3(1, 1, 1); // Scale appropriately for visibility
        meshFilter.mesh = CreateTriangleMesh(); // Set custom triangle mesh
        break;

    case ShapeType.Circle:
        meshRenderer.material.color = Color.blue;
        shape.transform.localScale = new Vector3(1, 1, 1); // Scale appropriately for visibility
        meshFilter.mesh = CreateCircleMesh(32); // Set circle mesh with 32 segments
        break;

    case ShapeType.Square:
        meshRenderer.material.color = Color.green;
        shape.transform.localScale = new Vector3(1, 1, 0.1f); // Adjust for 2D square
        meshFilter.mesh = CreateSquareMesh(); // Set square mesh
        break;
}

}

// Method to create a triangle mesh
private Mesh CreateTriangleMesh()
{
    Mesh mesh = new Mesh();

    // Define 3 vertices of a triangle (in counter-clockwise order)
    Vector3[] vertices = new Vector3[]
    {
        new Vector3(0, 1, 0),    // Top vertex
        new Vector3(-1, -1, 0),  // Bottom-left vertex
        new Vector3(1, -1, 0)    // Bottom-right vertex
    };

    // Apply 180-degree rotation around the Y-axis to each vertex
    for (int i = 0; i < vertices.Length; i++)
    {
        vertices[i] = Quaternion.Euler(0, 180, 0) * vertices[i];
    }

    // Define the triangles (in counter-clockwise order)
    int[] triangles = new int[] { 0, 1, 2 };

    // Assign the vertices and triangles to the mesh
    mesh.vertices = vertices;
    mesh.triangles = triangles;

    // Recalculate normals to ensure they face outward
    mesh.RecalculateNormals();

    return mesh;
}





private Mesh CreateCircleMesh(int segments)
{
    Mesh mesh = new Mesh();

    // Create vertices for the circle
    Vector3[] vertices = new Vector3[segments + 1];
    vertices[0] = Vector3.zero; // Center point

    float angleStep = 360f / segments;

    for (int i = 1; i <= segments; i++)
    {
        float angle = Mathf.Deg2Rad * (i * angleStep);
        vertices[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0); // Circle vertices
    }

    // Apply 180-degree rotation around the Y-axis to each vertex
    for (int i = 0; i < vertices.Length; i++)
    {
        vertices[i] = Quaternion.Euler(0, 180, 0) * vertices[i];
    }

    // Create triangles for the circle (in counter-clockwise order)
    int[] triangles = new int[segments * 3];
    for (int i = 0; i < segments; i++)
    {
        triangles[i * 3] = 0; // Center vertex
        triangles[i * 3 + 1] = (i + 1) % segments + 1; // Current vertex
        triangles[i * 3 + 2] = (i + 2) % segments + 1; // Next vertex
    }

    // Assign vertices and triangles to the mesh
    mesh.vertices = vertices;
    mesh.triangles = triangles;

    // Recalculate normals to ensure they face outward
    mesh.RecalculateNormals();

    return mesh;
}





// Method to create a square mesh
private Mesh CreateSquareMesh()
{
    Mesh mesh = new Mesh();

    // Define 4 vertices of the square
    Vector3[] vertices = new Vector3[]
    {
        new Vector3(-1, -1, 0), // Bottom-left
        new Vector3(1, -1, 0),  // Bottom-right
        new Vector3(-1, 1, 0),  // Top-left
        new Vector3(1, 1, 0)    // Top-right
    };

    // Define the triangles (two triangles make up a square)
    int[] triangles = new int[]
    {
        0, 2, 1,  // First triangle (bottom-left, top-left, bottom-right)
        1, 2, 3   // Second triangle (bottom-right, top-left, top-right)
    };

    // Assign vertices and triangles to the mesh
    mesh.vertices = vertices;
    mesh.triangles = triangles;

    // Optionally calculate normals for lighting
    mesh.RecalculateNormals();

    return mesh;
}




    private void SetSymbol(string symbol)
    {
        symbolText.text = symbol; // Set the text to the perk's name
    }


}
