using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalLogic : MonoBehaviour
{
    GoalManager goalManager;
    private void Start()
    {
        goalManager = transform.parent.GetComponent<GoalManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        goalManager.hit(this.gameObject);
    }
}
