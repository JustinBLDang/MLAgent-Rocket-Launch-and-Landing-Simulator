using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class RocketAgent : Agent
{
    [SerializeField] GoalManager goalManager;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject fThrust;
    [SerializeField] GameObject bThrust;
    [SerializeField] GameObject rThrust;
    [SerializeField] GameObject lThrust;
    [SerializeField] float mainThrust = 1;
    [SerializeField] Transform startPosition;
    [SerializeField] Material failMaterial;
    [SerializeField] Material progressMaterial;
    [SerializeField] Material successMaterial;
    [SerializeField] MeshRenderer platformMesh;
    [SerializeField] GameObject LandingPad;
    //int successRange = 30;
    float currentSpeedSquared;
    //float timeSinceLastExecution;
    //Vector3 lastPosition;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startPosition.localPosition;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        goalManager.resetGoals();
        // timeSinceLastExecution = Time.time; // timeSinceLastExecution = 0f
        //lastPosition = transform.position;
        LandingPad.gameObject.tag = "ground";
    }
    //private void FixedUpdate()
    //{
    //    if (Time.time - timeSinceLastExecution >= 2f)
    //    {
    //        if (Vector3.Magnitude(transform.position - lastPosition) <= 3f)
    //        {
    //            AddReward(-3f);
    //        }
    //        timeSinceLastExecution = Time.time;
    //    }
    //}
    public override void CollectObservations(VectorSensor sensor)
    {
        // Here, we observe every goal attached to our GoalManager
        foreach (GameObject goal in goalManager.goals)
        {
            sensor.AddObservation(goal.transform.localPosition);
        }
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(rb.velocity);
        sensor.AddObservation(LandingPad.gameObject.transform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        MainThrust(actions.ContinuousActions[0]); 
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetKey("space") ? 1 : 0;
    }
    private void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.tag == "heightGoal")
        {
            //Debug.Log("heightGoal");
            currentSpeedSquared = rb.velocity.y * rb.velocity.y;
            platformMesh.material = progressMaterial;
            AddReward(4000 / currentSpeedSquared);
            LandingPad.gameObject.tag = "landingGoal";
        }
        if(other.gameObject.tag == "heightLimit")
        {
            platformMesh.material = failMaterial;
            EndEpisode();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "landingGoal")
        {
            //Debug.Log("landingGoal");
            AddReward(4000 / currentSpeedSquared);
            platformMesh.material = successMaterial;
            EndEpisode();
        }
    }
    void MainThrust(float strength)
    {
        rb.AddForce(transform.up.normalized * mainThrust * Math.Max(strength, 0), ForceMode.Force);
    }
}
