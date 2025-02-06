using Unity.Mathematics;
using UnityEngine;

public partial class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    [SerializeField] TileCtrl tilePrefab;
    [SerializeField] GirdWord girdPrefab;
    [SerializeField] Transform tileParent;
    public GirdWord gird;
    TileCtrl[] tiles;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        TouchSystem.Instance.OnTouchBegan += OnTouchBegan;
        TouchSystem.Instance.OnTouchMoved += OnTouchMoved;
        TouchSystem.Instance.OnTouchEnd += OnTouchEnd;
    }

    private void OnDestroy()
    {
        TouchSystem.Instance.OnTouchBegan -= OnTouchBegan;
        TouchSystem.Instance.OnTouchMoved -= OnTouchMoved;
        TouchSystem.Instance.OnTouchEnd -= OnTouchEnd;
    }

    public void InitTile()
    {
        gird = Instantiate(girdPrefab, transform);
        var girdSoup = new int2(8, 8);
        var scale = new float2(2, 2);
        gird.InitGird(girdSoup, scale);
        var amoutTile = girdSoup.x * girdSoup.y;

        tiles = new TileCtrl[amoutTile];
        chesses = new ChessCtrl[amoutTile];

        for (int i = 0; i < amoutTile; i++)
        {
            var position = gird.ConvertIndexToWordPos(i);
            var tile = Instantiate(tilePrefab, tileParent);
            tile.transform.localScale = new float3(scale.x, 0.5f, scale.y);
            tile.transform.position = position;
            tiles[i] = tile;
            gird.SetValue(i,gird.EmmtyValue);
            
            var girdPos = gird.ConvertIndexToGridPos(i);
            if ((girdPos.x + girdPos.y) % 2 == 0)
            {
                tile.InjectColor(Color.white);
            }
            else
            {
                tile.InjectColor(Color.black);
            }
        }
    }
}
