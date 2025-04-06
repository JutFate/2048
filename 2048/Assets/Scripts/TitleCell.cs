using UnityEngine;

public class TitleCell : MonoBehaviour
{
  public Vector2Int coordinates { get; set; }
    public Title tile { get; set; }
    public bool emty => tile == null;
    public bool occupied => tile != null;


}
