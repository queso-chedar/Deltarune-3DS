using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaveDistort : MonoBehaviour
{
    [Header("Grid Settings")]
    public int cols = 12;
    public int rows = 8;           // subdivisiones verticales
    public float width = 3.2f;      // ancho en unidades (para 320px → 3.2u, escala 100px = 1u)
    public float height = 2.4f;     // alto en unidades (240px → 2.4u)

    [Header("Wave Settings")]
    public float speed = 1f;        // velocidad de desplazamiento de la onda
    public float amplitude = 0.1f;  // altura máxima de la onda
    public float wavelength = 1f;   // longitud de onda (en unidades del UV X)

    Mesh mesh;
    Vector3[] baseVerts;
    Vector2[] baseUV;

    void Start()
    {
        // Generar malla subdividida
        mesh = new Mesh();
        mesh.name = "WavePlane";
        GetComponent<MeshFilter>().mesh = mesh;

        int vCount = (cols + 1) * (rows + 1);
        Vector3[] verts = new Vector3[vCount];
        Vector2[] uvs   = new Vector2[vCount];
        int[] tris     = new int[cols * rows * 6];

        float dx = width  / cols;
        float dy = height / rows;
        int vi = 0, ti = 0;
        for (int y = 0; y <= rows; y++)
        {
            for (int x = 0; x <= cols; x++, vi++)
            {
                // posición local
                verts[vi] = new Vector3(-width/2 + x * dx, -height/2 + y * dy, 0);
                // UV en [0,1]
                uvs[vi] = new Vector2((float)x/cols, (float)y/rows);

                // triángulos
                if (x < cols && y < rows)
                {
                    int a = vi;
                    int b = vi + cols + 1;
                    tris[ti++] = a;
                    tris[ti++] = b;
                    tris[ti++] = a + 1;
                    tris[ti++] = a + 1;
                    tris[ti++] = b;
                    tris[ti++] = b + 1;
                }
            }
        }

        mesh.vertices  = verts;
        mesh.uv        = uvs;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        // Guardamos verts y UV para referencia
        baseVerts = mesh.vertices;
        baseUV    = mesh.uv;
    }

    void Update()
    {
        Vector3[] verts = new Vector3[baseVerts.Length];

        float t = Time.time * speed;
        for (int i = 0; i < baseVerts.Length; i++)
        {
            Vector3 v = baseVerts[i];
            Vector2 uv = baseUV[i];

            // onda senoidal en Y según coord X+tiempo
            float wave = Mathf.Sin((uv.x * wavelength + t) * Mathf.PI * 2f);
            v.y += wave * amplitude;

            verts[i] = v;
        }

        mesh.vertices = verts;
        // opcional: stir UVs para scroll si quieres
        // mesh.uv = modifiedUVs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
