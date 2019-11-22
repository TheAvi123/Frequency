using UnityEngine;

public class GradientMesh : MonoBehaviour
{
    [System.Serializable]
    private struct ColorPair
    {
        public Color topColor;
        public Color botColor;
    }

    //Mesh Configuration Parameters
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

    //Gradient Configuration Parameters
    [SerializeField] Material meshMaterial = null;
    [SerializeField] Gradient colorGradient = null;
    [SerializeField] float gradientSpeed = 0.015f;

    //State Variables
    private ColorPair currentColors;
    private float topTicker = 0f;
    private float botTicker = 0f;

    //Color State Variables
    private float H, S, V;

    //Internal Methods
    private void Awake() {
        AddMeshComponents();
        SetMeshMaterial();
        SetRandomColor();
    }

    private void AddMeshComponents() {
        meshFilter = gameObject.AddComponent<MeshFilter>() as MeshFilter;
        meshRenderer = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    private void SetMeshMaterial() {
        meshRenderer.material = meshMaterial;
    }

    private void SetRandomColor() {
        topTicker = Random.value;       //Set Ticker to Random Color on Gradient
    }

    private void OnSceneChange() {
        ResetMeshPosition();
        SetScreenCornerVectors();
        AssignMeshVerticesAndTriangles();
    }   //Called Through Singleton

    private void ResetMeshPosition() {
        transform.position = Vector2.zero;
    }

    private void SetScreenCornerVectors() {
        Camera gameCamera = Camera.main;
        topLeft = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 60));
        topRight = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, 60));
        botLeft = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 60));
        botRight = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 60));
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

    private void Update() {
        UpdateTickers();
        UpdateCurrentColors();
        UpdateMeshColors();
    }

    private void UpdateTickers() {
        topTicker += Time.deltaTime * gradientSpeed;   //Increase Ticker per Frame
        if (topTicker > 1) {
            topTicker -= 1;    //Reset to 0 to Start Back at Beginning of Gradient
        }
        botTicker = topTicker + 0.20f;  //Increase Ticker per Frame Alonside topTicker
        if (botTicker > 1) {
            botTicker -= 1;    //Reset to 0 to Start Back at Beginning of Gradient
        }
    }

    private void UpdateCurrentColors() {
        currentColors.topColor = DesaturateColor(colorGradient.Evaluate(topTicker), 0.65f);
        currentColors.botColor = DesaturateColor(colorGradient.Evaluate(botTicker), 0.35f);
    }

    private Color DesaturateColor(Color color, float saturation) {
        Color.RGBToHSV(color, out H, out S, out V);
        S = saturation;
        return Color.HSVToRGB(H, S, V);
    }

    private void UpdateMeshColors() {
        mesh.colors = new Color[] {currentColors.topColor, currentColors.topColor, currentColors.botColor, currentColors.botColor};
    }

    //Public Getter Methods
    public Gradient GetColorGradient() {
        return colorGradient;
    }

    public float GetGradientSpeed() {
        return gradientSpeed;
    }

    public float GetTickerValue() {
        return topTicker;
    }
}
