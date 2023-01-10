using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Respawns2 : MonoBehaviour
{
  public GameObject enemyPrefab;
  public float waveTime = 5000;
  public int waveEnemies = 10;
  public Camera theCamera;
  public TextMeshProUGUI textMeshPro;
  public PlayerController playerController;

  private float _lastWaveAt;
  private int _waveNumber;

  private void Start()
  {
    if (!theCamera)
    {
      theCamera = Camera.main;
    }

    _lastWaveAt = -waveTime - 1;
  }

  private void Update()
  {
    if (Utility.CurrentMillis() - _lastWaveAt > waveTime)
    {
      Spawn();
    }
  }

  private void Spawn()
  {
    if (enemyPrefab)
    {
      var radius = theCamera.orthographicSize + 3;
      var p = theCamera.transform.position;
      var numEnemies = waveEnemies + _waveNumber;

      // inc old enemies speed
      var oldEnemies = GameObject.FindGameObjectsWithTag("Enemy");
      foreach (var oldEnemy in oldEnemies)
      {
        var enemyCtrl = oldEnemy.GetComponent<EnemyController>();
        var r = Random.Range(0, 10);
        switch (r)
        {
        case < 5:
          enemyCtrl.speed += Random.Range(0.1f, 1.0f);
          enemyCtrl.speed = Mathf.Max(enemyCtrl.speed, 6);
          break;
        case > 8:
          enemyCtrl.hp += Random.Range(1, _waveNumber / 2);
          break;
        }
      }

      for (var i = 0; i < numEnemies; i++)
      {
        var enemy = Instantiate(enemyPrefab);
        enemy.transform.position = new Vector2(p.x, p.y) + Random.insideUnitCircle.normalized *radius;
        enemy.GetComponent<EnemyController>().speed = Random.Range(1.0f, 6.0f);
      }

      // inc player DPS
      if (_waveNumber % 5 == 0 && _waveNumber > 0)
      {
        if (playerController)
        {
          playerController.dps = Mathf.Min(10, playerController.dps + 1);
          if (_waveNumber % 10 == 0)
          {
            playerController.bullets = Mathf.Min(playerController.bullets + 1, 20);
          }
        }
      }

      _waveNumber++;
      textMeshPro.SetText("Wave: " + _waveNumber);
    }

    _lastWaveAt = Utility.CurrentMillis();
  }
}