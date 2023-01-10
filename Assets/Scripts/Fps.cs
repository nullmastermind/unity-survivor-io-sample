using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fps : MonoBehaviour
{
  private void Start()
  {
    Application.targetFrameRate = int.MaxValue;
    Application.runInBackground = true;
    Time.timeScale = 1f;
  }
}
