using UnityEngine;

public class TintMeshController
{
    //primeworld 教程Mesh
    private Mesh _mesh;
    private Vector3[] _vertices;
    private GameObject _gameObject;
    private BoxCollider[] _colliders;
    private Color[] _colors;
    private UISprite _sprite;

    protected Camera _uiCamera;
    protected UIRoot _root;

    //protected Mesh mesh
    //{
    //    get { return _mesh; }
    //    set { _mesh = value; }
    //}

    protected Vector3[] vertices
    {
        get { return _vertices; }
        set { _vertices = value; }
    }

    protected Color[] colors
    {
        get { return _colors; }
        set { _colors = value; }
    }

    protected void SetupColliders(Rect outerRect)
    {

        
        float width = Screen.width;
		float height = Screen.height;

		float halfWidth = Screen.width / 2f;
		float halfHeight = Screen.height / 2f;

        //Top part
        _colliders[0].center = new Vector3(0f, (halfHeight + outerRect.yMax) / 2f, 0f);
        _colliders[0].size = new Vector3(width, (_colliders[0].center.y - outerRect.yMax) * 2f, 0f);

        //Bottom part
        _colliders[1].center = new Vector3(0f, (-halfHeight + outerRect.yMin) / 2f, 0f);
        _colliders[1].size = new Vector3(width, (_colliders[1].center.y - outerRect.yMin) * 2f, 0f);

        //Left part
        _colliders[2].center = new Vector3((-halfWidth + outerRect.xMin) / 2f, outerRect.center.y, 0f);
        _colliders[2].size = new Vector3((_colliders[2].center.x - outerRect.xMin) * 2f, outerRect.height, 0f);

        //Right part
        _colliders[3].center = new Vector3((halfWidth + outerRect.xMax) / 2f, outerRect.center.y, 0f);
        _colliders[3].size = new Vector3((_colliders[3].center.x - outerRect.xMax) * 2f, outerRect.height, 0f);
    }

    public void SetAlpha(float alpha)
    {
        for (int i = 0; i < _colors.Length; i++)
            _colors[i] = new Color(1f, 1f, 1f, alpha);

        _mesh.colors = _colors;
    }

    public virtual void SetTintPosition(Vector3 position, Vector3 scale)
    {

		float width = 1920;
		float height = 1080;

		float halfWidth = 1920 / 2f;
		float halfHeight = 1080 / 2f;

        //scale = GUIUtils.GetGUIScale(scale,_uiCamera,_root);
        Vector3 halfScale = scale / 2f;


        //Out rect
        _vertices[0] = new Vector3(-halfWidth, halfHeight);
        _vertices[1] = new Vector3(halfWidth, halfHeight);
        _vertices[6] = new Vector3(-halfWidth, -halfHeight);
        _vertices[7] = new Vector3(halfWidth, -halfHeight);

        //In rect
        Rect rect = new Rect(position.x - halfScale.x, position.y - halfScale.y, scale.x, scale.y);

        _vertices[2] = new Vector3(rect.xMin, rect.yMax);
        _vertices[3] = new Vector3(rect.xMax, rect.yMax);
        _vertices[4] = new Vector3(rect.xMin, rect.yMin);
        _vertices[5] = new Vector3(rect.xMax, rect.yMin);

        _vertices[8] = new Vector3(rect.xMin, rect.yMax);
        _vertices[9] = new Vector3(rect.xMax, rect.yMax);
        _vertices[10] = new Vector3(rect.xMin, rect.yMin);
        _vertices[11] = new Vector3(rect.xMax, rect.yMin);

        SetupColliders(rect);
        UpdateVertices();
    }

    protected void UpdateVertices()
    {
        _mesh.vertices = _vertices;
    }

    public bool enabled
    {
        get { return _gameObject.activeSelf; }
        set { _gameObject.SetActive(value); }
    }

    public TintMeshController(Transform parent, UISprite sprite, Material tintMaterial, Camera uiCamera, UIRoot root)
    {
        _uiCamera = uiCamera;
        _root = root;


        _sprite = sprite;

        sprite.enabled = false;

        _gameObject = new GameObject();
        _gameObject.name = "TintMesh_" + sprite.spriteName;
        _gameObject.layer = parent.gameObject.layer;
        _gameObject.transform.parent = parent;
        _gameObject.transform.localPosition = new Vector3(0f, 0f, -300f);
        _gameObject.transform.localScale = Vector3.one;

        Material[] materials = new Material[2];
        materials[0] = sprite.atlas.spriteMaterial;
        materials[1] = tintMaterial;
        _gameObject.AddComponent<MeshRenderer>().materials = materials;


        _colliders = new BoxCollider[4];
        for (int i = 0; i < 4; i++)
        {
            GameObject go = new GameObject();
            go.transform.parent = _gameObject.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.layer = _gameObject.layer;
            _colliders[i] = go.AddComponent<BoxCollider>();
        }
    }

    public void SetMesh()
    {
        _mesh = CreateTintMesh();
        _gameObject.AddComponent<MeshFilter>().mesh = _mesh;
    }

    protected virtual Mesh CreateTintMesh()
    {
        const int VertexCount = 12;

        Mesh mesh = new Mesh();
        vertices = new Vector3[VertexCount];
        colors = new Color[VertexCount];

        int[] outerTriangles = new int[24];
        Vector2[] uv = new Vector2[VertexCount];

        for (int i = 0; i < VertexCount; i++)
            colors[i] = new Color(1f, 1f, 1f, 1f);

        uv[0] = new Vector2(0f, 0f);
        uv[1] = new Vector2(1f, 0f);
        uv[2] = new Vector2(0f, 1f);
        uv[3] = new Vector2(1f, 1f);

        uv[4] = new Vector2(0f, 0f);
        uv[5] = new Vector2(1f, 0f);
        uv[6] = new Vector2(0f, 1f);
        uv[7] = new Vector2(1f, 1f);

        float onePix = 2f / _sprite.atlas.texture.width;
        uv[8] = new Vector2(_sprite.outerUV.xMin + onePix, _sprite.outerUV.yMin + onePix);
        uv[9] = new Vector2(_sprite.outerUV.xMax - onePix, _sprite.outerUV.yMin + onePix);
        uv[10] = new Vector2(_sprite.outerUV.xMin + onePix, _sprite.outerUV.yMax - onePix);
        uv[11] = new Vector2(_sprite.outerUV.xMax - onePix, _sprite.outerUV.yMax - onePix);

        //Outer
        outerTriangles[0] = 0;
        outerTriangles[1] = 1;
        outerTriangles[2] = 3;
        outerTriangles[3] = 0;
        outerTriangles[4] = 3;
        outerTriangles[5] = 2;

        outerTriangles[6] = 0;
        outerTriangles[7] = 2;
        outerTriangles[8] = 6;
        outerTriangles[9] = 2;
        outerTriangles[10] = 4;
        outerTriangles[11] = 6;

        outerTriangles[12] = 4;
        outerTriangles[13] = 7;
        outerTriangles[14] = 6;
        outerTriangles[15] = 4;
        outerTriangles[16] = 5;
        outerTriangles[17] = 7;

        outerTriangles[18] = 3;
        outerTriangles[19] = 1;
        outerTriangles[20] = 5;
        outerTriangles[21] = 5;
        outerTriangles[22] = 1;
        outerTriangles[23] = 7;

        //Inner
        int[] innerTriangles = new int[6];

        innerTriangles[0] = 8;
        innerTriangles[1] = 11;
        innerTriangles[2] = 10;
        innerTriangles[3] = 8;
        innerTriangles[4] = 9;
        innerTriangles[5] = 11;

        mesh.subMeshCount = 2;
        mesh.vertices = vertices;
        //mesh.triangles = triangles;
        mesh.SetTriangles(innerTriangles, 0);
        mesh.SetTriangles(outerTriangles, 1);
        mesh.uv = uv;
        mesh.colors = colors;

        return mesh;
    }
}
