using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveConst : MonoBehaviour
{
    [SerializeField] float speed = .3f;

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var targetPoint = Vector3.zero;
        targetPoint.y = GameDataManager.instance.EnemyHeight/2;

        transform.LookAt(targetPoint);
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }
}
