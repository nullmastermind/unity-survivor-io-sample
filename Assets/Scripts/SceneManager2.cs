using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager2 : MonoBehaviour
{
  public GameObject inputPanel;
  public GameObject restartUi;

  public void OnRestart()
  {
    var scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
  }

  public void OnEnd()
  {
    if (inputPanel)
    {
      inputPanel.SetActive(false);
    }

    if (restartUi)
    {
      restartUi.SetActive(true);
    }

    var respawns = FindObjectsOfType<Respawns2>();
    foreach (var respawn in respawns)
    {
      Destroy(respawn);
    }

    var enemies = GameObject.FindGameObjectsWithTag("Enemy");
    foreach (var enemy in enemies)
    {
      Destroy(enemy);
    }

    var players = GameObject.FindGameObjectsWithTag("Player");
    foreach (var player in players)
    {
      Destroy(player);
    }
  }
}