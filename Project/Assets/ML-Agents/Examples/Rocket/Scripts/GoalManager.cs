using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public GameObject[] goals;

    public void hit(GameObject goal)
    {
        goal.SetActive(false);
    }

    public void resetGoals()
    {
        foreach (GameObject goal in goals)
        {
            goal.SetActive(true);
        }
    }
}
