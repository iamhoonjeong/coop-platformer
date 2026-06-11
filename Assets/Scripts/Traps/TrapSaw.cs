using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSaw : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;

    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float cooldown = 1f;
    [SerializeField] Transform[] waypoint;

    Vector3[] wayPointPosition;

    public int wayPointIndex = 1;
    public int moveDirection = 1;
    bool canMove = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        List<TrapSawWaypoints> wayPointList = new List<TrapSawWaypoints>(GetComponentsInChildren<TrapSawWaypoints>());

        if (wayPointList.Count != waypoint.Length)
        {
            waypoint = new Transform[wayPointList.Count];

            for (int i = 0; i < wayPointList.Count; i++)
            {
                waypoint[i] = wayPointList[i].transform;
            }
        }

        UpdateWaypointsInfo();
        transform.position = wayPointPosition[0];
    }

    private void UpdateWaypointsInfo()
    {
        wayPointPosition = new Vector3[waypoint.Length];

        for (int i = 0; i < waypoint.Length; i++)
        {
            wayPointPosition[i] = waypoint[i].position;
        }
    }

    void Update()
    {
        anim.SetBool("active", canMove);

        if (!canMove) return;

        transform.position = Vector2.MoveTowards(transform.position, wayPointPosition[wayPointIndex], moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, wayPointPosition[wayPointIndex]) < 0.1f)
        {
            if (wayPointIndex == wayPointPosition.Length - 1 || wayPointIndex == 0)
            {
                moveDirection *= -1;
                StartCoroutine(StopMovement(cooldown));
            }
            wayPointIndex = wayPointIndex + moveDirection;
        }
    }

    IEnumerator StopMovement(float delay)
    {
        canMove = false;

        yield return new WaitForSeconds(delay);

        canMove = true;
        sr.flipX = !sr.flipX;
    }
}
