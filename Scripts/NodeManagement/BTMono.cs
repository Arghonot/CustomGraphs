using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using BT;

/// <summary>
/// This AI's mono context.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class BTMono : MonoBehaviour
{
    public Transform target;
    public Collider area;

    public float DistaneToShoot;
    public float DistanceToFollow;

    public GunBehavior gun;

    GenericDictionary gd = new GenericDictionary();

    //Animator _anim;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    void Awake()
    {
        var agent = GetComponent<NavMeshAgent>();

        gd.Set<Transform>("self", transform);
        gd.Set<Transform>("target", target);
        gd.Set<NavMeshAgent>("agent", agent);
        gd.Set<Collider>("wanderArea", area);
        gd.Set<float>("AgressiveDistance", DistaneToShoot);
        gd.Set<float>("DistanceToFollow", DistanceToFollow);
        gd.Set<GunBehavior>("Gun", gun);

        BTExecutor.Instance.RegisterContext(gd);
        //agent.updatePosition = false;

    }

    //UNCOMMENT THIS IF YOU'RE USING AN ANIMATOR CONTROLLER
    // YOU'LL NEED TO USE two variables (VelX AND VelY)
    //private void Update()
    //{
    //    Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

    //    // Map 'worldDeltaPosition' to local space
    //    float dx = Vector3.Dot(transform.right, worldDeltaPosition);
    //    float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
    //    Vector2 deltaPosition = new Vector2(dx, dy);

    //    // Low-pass filter the deltaMove
    //    float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
    //    smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

    //    // Update velocity if time advances
    //    if (Time.deltaTime > 1e-5f)
    //        velocity = smoothDeltaPosition / Time.deltaTime;

    //    bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

    //    // Update animation parameters
    //    //_anim.SetBool("move", shouldMove);

    //    _anim.SetFloat("VelX", velocity.x);
    //    _anim.SetFloat("VelY", velocity.y);
    //}

    //void OnAnimatorMove()
    //{
    //    // Update position to agent position
    //    transform.position = agent.nextPosition;
    //}
}
