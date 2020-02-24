using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class ObstacleTowerAgent : Agent
{
    Rigidbody rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

}
