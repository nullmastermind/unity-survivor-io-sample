using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ControlFreak2;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
  public float speed = 5;
  public Camera theCamera;
  public Vector3 cameraOffset = Vector3.zero;
  public GameObject bulletPrefab;
  public float dps = 3;
  public int bullets = 1;
  public SceneManager2 sceneManager;

  private double _lastShootAt;
  private float _radius;

  private void Start()
  {
    if (!theCamera)
    {
      theCamera = Camera.main;
    }

    var cameraW = theCamera.orthographicSize * theCamera.aspect;
    _radius = cameraW;
  }

  private void Update()
  {
    var moveX = CF2Input.GetAxis("Horizontal") * speed * Time.deltaTime;
    var moveY = CF2Input.GetAxis("Vertical") * speed * Time.deltaTime;
    var p = transform.position;
    var p1 = theCamera.transform.position;
    p = new Vector3(p.x + moveX, p.y + moveY, p.z);
    transform.position = p;
    theCamera.transform.position = new Vector3(p.x, p.y, p1.z) + cameraOffset;

    AutoShoot();
  }

  private void AutoShoot()
  {
    if (Utility.CurrentMillis() - _lastShootAt > 1000.0 / dps)
    {
      var nearlyEnemies = GetClosestEnemies();
      for (var i = 0; i < bullets; i++)
      {
        if (i < nearlyEnemies.Count)
        {
          Shoot(nearlyEnemies[i].transform);
        }
      }
    }
  }

  private List<Collider2D> GetClosestEnemies()
  {
    var hitColliders = Physics2D.OverlapCircleAll(transform.position, _radius)
                           .ToList()
                           .FindAll(hit => hit.transform.CompareTag("Enemy"));
    var currentPosition = transform.position;
    return hitColliders.OrderBy(hit => (hit.transform.position - currentPosition).sqrMagnitude).ToList();
  }

  private void Shoot(Transform enemy)
  {
    if (bulletPrefab)
    {
      var bullet = Instantiate(bulletPrefab);
      bullet.transform.position = transform.position;
      bullet.GetComponent<Bullet>().targetPosition = enemy.position;
    }

    _lastShootAt = Utility.CurrentMillis();
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.transform.CompareTag("Enemy"))
    {
      if (sceneManager)
      {
        sceneManager.OnEnd();
      }
    }
  }
}