using UnityEngine;
using System.Collections;

public class SortingOrderScript : MonoBehaviour
{
  public const string LAYER_NAME = "AlwaysFront";
  public int sortingOrder = 0;
  public SpriteRenderer sprite;
  public YSorting Scriptsorting;

  void Start()
  {
    Scriptsorting.enabled = false;
    StartCoroutine(MiCorrutina()); // Inicia la corrutina
  }
    IEnumerator MiCorrutina()
        {
       yield return new WaitForSeconds(1f);
      sprite = GetComponent<SpriteRenderer>();
            if (sprite)
    {
      sprite.sortingOrder = sortingOrder;
      sprite.sortingLayerName = LAYER_NAME;
    }
        }
}