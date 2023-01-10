using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  public float speed = 21;
  public Vector3 targetPosition;
  public bool canDestroy = false;

  private void Update()
  {
    if (canDestroy)
    {
      Destroy(gameObject);
      return;
    }
    transform.Rotate(1, 1, 1);
    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    if (transform.position == targetPosition)
    {
      canDestroy = true;
    }
  }
}