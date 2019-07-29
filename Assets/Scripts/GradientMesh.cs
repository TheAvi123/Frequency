using UnityEngine;
using System.Collections.Generic;

public class GradientMesh : MonoBehaviour
{
    [System.Serializable]
    private class ColorPair     //Inner Class For Private Use
    {
        public Color topColor;
        public Color botColor;

        public ColorPair(Color topColor, Color botColor) {
            this.topColor = topColor;
            this.botColor = botColor;
        }
    }

    [Header("Mesh Components")]
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    [Header("Mesh Variables")]
    private Vector3[] vertices;
    private Color[] vertexColors;
    private int[] triangles;

    [Header("Mesh Vertices")]
    private Vector3 topLeft;
    private Vector3 topRight;
    private Vector3 botLeft;
    private Vector3 botRight;

    [Header("Mesh Parameters")]
    [SerializeField] Material meshMaterial = null;
    [SerializeField] ColorPair[] colorPairs;
    [SerializeField] float transitionSpeed = 1f;

    //State Variables
    private ColorPair oldColors;
    private ColorPair newColors;
    private Color currentTopColor;
    private Color currentBotColor;
    private bool colorsChanged = true;
    private float lerpTimer = 1f;

    //Methods
    private void Start() {
        SetScreenCornerVectors();
        AddMeshComponents();
        AssignMeshVerticesAndTriangles();
        SetMeshMaterial();
    }

    private void SetScreenCornerVectors() {
        Camera gameCamera = Camera.main;
        topLeft = gameCamera.ViewportToWorldPoint(new Vector3(0, 1));
        topRight = gameCamera.ViewportToWorldPoint(new Vector3(1, 1));
        botLeft = gameCamera.ViewportToWorldPoint(new Vector3(0, 0));
        botRight = gameCamera.ViewportToWorldPoint(new Vector3(1, 0));
    }

    private void AddMeshComponents() {
        meshFilter = gameObject.AddComponent<MeshFilter>() as MeshFilter;
        meshRenderer = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    private void AssignMeshVerticesAndTriangles() {
        mesh.Clear();   //Clear All Vertices and Triangles in Mesh
        vertices = new Vector3[] {topLeft, topRight, botLeft, botRight};
        mesh.vertices = vertices;
        triangles = new int[] { 0, 1, 2, /**/  2, 1, 3 };
        mesh.triangles = triangles;
        mesh.RecalculateNormals();  //Recalculate Normals Based on New Vertices
        mesh.RecalculateBounds();   //Recalculate Volume Bound Based on New Triangles
    }

    private void SetMeshMaterial() {
        meshRenderer.material = meshMaterial;
        newColors = colorPairs[Random.Range(0, colorPairs.Length)];
        oldColors = newColors;
        mesh.colors = new Color[] {oldColors.topColor, oldColors.topColor, oldColors.botColor, oldColors.botColor};
    }
    
    private void Update() {
        /*        if (lerpTimer >= 1) {
                    SelectNewColors();
                } else {
                    UpdateMeshColors();
                }*/

        //Commented Out for Testing

        //TestMethod
        mesh.colors = new Color[] { colorPairs[0].topColor, colorPairs[0].topColor, colorPairs[0].botColor, colorPairs[0].botColor };

    }

    private void SelectNewColors() {
        oldColors = newColors;
        while (newColors == oldColors) {
            newColors = colorPairs[Random.Range(0, colorPairs.Length)];
        }
        lerpTimer = 0f;
    }

    private void UpdateMeshColors() {
        lerpTimer += Time.deltaTime * transitionSpeed;
        currentTopColor = Color.Lerp(oldColors.topColor, newColors.topColor, lerpTimer);
        currentBotColor = Color.Lerp(oldColors.botColor, newColors.botColor, lerpTimer);
        mesh.colors = new Color[] {currentTopColor, currentTopColor, currentBotColor, currentBotColor};
    }
}
