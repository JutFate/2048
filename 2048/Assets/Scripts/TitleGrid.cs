using UnityEngine;

public class TitleGrid : MonoBehaviour
{
    public TitleRow[] rows { get; private set; }
    public TitleCell[] cells { get; private set; }

    public int size => cells.Length;
    public int height => rows.Length;

    public int width => size/height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TitleRow>();
        cells = GetComponentsInChildren<TitleCell>();
    }

    private void Start()
    {
        for(int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates= new Vector2Int(x,y);
            }
        }
    }

    public TitleCell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[y].cells[x];
        }
        return null;
    }
    

    public TitleCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }
    public TitleCell GetAdjacentCell(TitleCell cell,Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;
        return GetCell(coordinates);
    }

    public TitleCell GetRandomEmtyCell()
    {
        int randomIndex = Random.Range(0, cells.Length);
        int startIndex = randomIndex;
        while (cells[randomIndex].occupied)
        {
            randomIndex++;

            if(randomIndex >= cells.Length)
            {
                randomIndex = 0;
            }
            if(randomIndex == startIndex)
            {
                return null;
            }
        }
        return cells[randomIndex];
    }
}
