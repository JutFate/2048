using UnityEngine;

public class TitleRow : MonoBehaviour
{
  public TitleCell[] cells { get; private set; }

    private void Awake()
    {
        cells = GetComponentsInChildren<TitleCell>();
       
    }
}
