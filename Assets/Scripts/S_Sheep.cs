using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class S_Sheep : MonoBehaviour
{

    [SerializeField] private Transform Player;
    [SerializeField] private float DistanceToFollow;

    [SerializeField]private NavMeshAgent agent;
    private bool PlayerIsFound;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player").transform;
    }

    private IEnumerator FollowPlayer()
    {

        if (PlayerIsFound == false) yield break;

        agent.SetDestination(Player.transform.position);
        yield return new WaitForSeconds(1.0f);

    }


    void Update()
    {

        float PlayerDist = Vector3.Distance(Player.position, transform.position);

        PlayerIsFound = PlayerDist < DistanceToFollow ? true : false;

        if (PlayerIsFound == true && PlayerDist < DistanceToFollow)
        {

            StartCoroutine(FollowPlayer());
        }
    }
}
