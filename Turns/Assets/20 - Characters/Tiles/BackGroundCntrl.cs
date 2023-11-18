using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundCntrl : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float turnDistance;
    private Vector3 initPos, targetPos;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        targetPos = GetPoint();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < turnDistance)
        {
            targetPos = GetPoint();
        }
    }

    private Vector3 GetPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle * size;

        Vector3 position = Vector3.zero;
        position.x = initPos.x + randomPoint.x;
        position.y = initPos.y;
        position.z = initPos.z + randomPoint.y;

        return (position);
    }
}
