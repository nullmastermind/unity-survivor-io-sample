using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public GameObject player;
  public float speed = 2;
  public float hp = 1;

  private Vector3 _originScale;

  private void Start()
  {
    if (!player)
    {
      player = GameObject.FindGameObjectWithTag("Player");
    }

    _originScale = transform.localScale * 1f;
  }

  private void Update()
  {
    if (player)
    {
      var p = transform.position;
      var p1 = player.transform.position;
      transform.position = Vector3.MoveTowards(p, p1, speed * Time.deltaTime);
      var l1 = 0.1f * (hp - 1);
      var scaleTo = _originScale + Vector3.one * l1;
      scaleTo.z = _originScale.z;
      transform.localScale = scaleTo;
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.transform.CompareTag("Bullet"))
    {
      // Calc HP here
      hp -= 1;
      if (hp <= 0)
      {
        Destroy(gameObject);
      }

      other.GetComponent<Bullet>().canDestroy = true;
    }
    else
    {
      if (other.transform.CompareTag("Enemy"))
      {
        var ec = other.GetComponent<EnemyController>();
        if (ec.hp > hp || (Math.Abs(ec.hp - hp) < 0.001f && ec.speed > speed))
        {
          ec.hp += hp;
          ec.speed = (speed + ec.speed) / 2f;
          Destroy(gameObject);
        }
      }
    }
  }
}