using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public int totalKills;

    public void AddKill()
    {
        totalKills += 1;
    }
}
