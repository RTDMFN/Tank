using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Tank
{

    /*
    This is the base enemy class. It inherits basic features from the tank class.
    This class allows basic pathfinding and manipulation of some of the variables involved in the enemy AI.
    */

    public Transform target;

    public float minimumDistanceFromNextWaypoint = 0.2f;
    public float repeatRate = 0.2f;
    public float firingDistance = 3f;
    public float maxFiringDistance = 5f;

    //States
    int state = 1;

    int currentWaypoint;
    float distanceToNextWaypoint;
    
    Path path;
    Seeker seeker;
    RaycastHit2D hit;

    protected override void Awake(){
        base.Awake();
        seeker = GetComponent<Seeker>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Start(){
        InvokeRepeating("UpdatePath",0f, repeatRate);
    }

    void UpdatePath(){
        if(seeker.IsDone()) seeker.StartPath(tankBase.position,target.position,OnPathComplete);
    }

    void OnPathComplete(Path completedPath){
        path = completedPath;
    }

    void FixedUpdate(){
        if(path == null || currentWaypoint >= path.vectorPath.Count) return;

        distanceToNextWaypoint = Vector2.Distance(tankBase.position,path.vectorPath[currentWaypoint]);
        if(distanceToNextWaypoint <= minimumDistanceFromNextWaypoint) currentWaypoint++;

        Vector2 lookDirection = ((Vector2)path.vectorPath[currentWaypoint] - tankBaseRB.position).normalized;
        Rotate(lookDirection,baseRotateSpeed);

        if(EnemyCanSeePlayer() && WithinFiringDistance() && state == 1) state = 2;
        if((!WithinMaxDistance() || !EnemyCanSeePlayer()) && state == 2) state = 1;
        switch(state){
            case 1:
                Move(baseMoveSpeed);
                break;
            case 2:
                Fire(baseFireCooldown,0f);
                UpdateGunPosition();
                break;
        }

        Aim(target.position);

    }

    bool WithinFiringDistance(){
        if(Vector2.Distance(target.position,tankBaseRB.position) <= firingDistance) return true;
        else return false;
    }

    bool EnemyCanSeePlayer(){
        hit = Physics2D.Raycast(tankBaseRB.position,(Vector2)target.position - tankBaseRB.position,Vector2.Distance(target.position,tankBaseRB.position),LayerMask.GetMask("Player","Environment"));
        if(hit.collider == null) return false;
        else if(hit.collider.tag == "Player") return true;
        else return false;
    }

    bool WithinMaxDistance(){
        if(Vector2.Distance(target.position,tankBaseRB.position) <= maxFiringDistance) return true;
        else return false;
    }

}
