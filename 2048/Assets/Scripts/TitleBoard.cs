using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class TitleBoard : MonoBehaviour
{

    public GameManager gameManager;
    public Title titlePrefab;
    public TitleState[] titleStates;
    private TitleGrid grid;
    private List<Title> tiles;

    private bool waiting;

    private void Awake()
    {
        grid = GetComponentInChildren<TitleGrid>();
        tiles = new List<Title>(16);
    }
   

    public void ClearBoard()
    {
        foreach(var cell in grid.cells)
        {
            cell.tile = null;
        }

        foreach (var title in tiles)
        {
            Destroy(title.gameObject);
        }
        tiles.Clear();
    }
    public void CreateTitle()
    {
        Title title = Instantiate(titlePrefab, grid.transform);
        title.SetState(titleStates[0], 2);
        title.Spawn(grid.GetRandomEmtyCell());
        tiles.Add(title);
    }

    private void Update()
    {

        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
        }
    }

    private  void Move(Vector2Int direction,int startX,int incrementX, int startY,int incrementY)
    {
        bool changed = false;
        for (int x=startX; x>=0 && x<grid.width; x+=incrementX)
        {
            for(int y = startY; y>=0 && y < grid.height; y+= incrementY)
            {
                TitleCell cell = grid.GetCell(x, y);

                if(cell.occupied)
                {
                    changed |=MoveTile(cell.tile, direction);
                }
            }
            }

        if(changed)
        {
            StartCoroutine(WaitForChange());
        }
        }

    private bool MoveTile(Title tile,Vector2Int direction)
    {
        TitleCell newCell = null;

        TitleCell adjacentCell = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacentCell !=null)
        {
            if(adjacentCell.occupied)
            {
                if(CanMerge(tile,adjacentCell.tile))
                {
                    Merge(tile, adjacentCell.tile);
                    return true;
                }
                break;
            }

            newCell = adjacentCell;
            adjacentCell = grid.GetAdjacentCell(adjacentCell, direction);
        }
        if (newCell !=null)
            {
                tile.MoveTo(newCell);
                return true;
            }
            
        
        return false;
    }

    private bool CanMerge(Title a,Title b)
    {
        return a.number == b.number && !b.locked;
    }

    private void Merge(Title a, Title b)
    {
        
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state)+1 , 0, titleStates.Length -1);
        int number = b.number * 2;

        b.SetState(titleStates[index], number);

        gameManager.IncreaseScore(number);
    }

    private int IndexOf(TitleState state)
    {
       for(int i = 0; i < titleStates.Length; i++)
        {
            if (state == titleStates[i])
            {
                return i;
            }
        }
        return -1;
    }
    private IEnumerator WaitForChange()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);

        waiting = false;
        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        if (tiles.Count != grid.size)
        {
            CreateTitle();
        }
        if(CheckForGameOver())
        {
            gameManager.GameOver();
        }

    }

    private bool CheckForGameOver()
    {
        if(tiles.Count != grid.size)
        {
            return false;
        }
        foreach (var tile in tiles)
        {
           TitleCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
           TitleCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
          TitleCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TitleCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if(up != null && CanMerge(tile,up.tile))
            {
                return false;
            }
            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }
            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }
            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }
        }
        return true;
    }
    }


    



