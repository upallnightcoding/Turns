using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinArrow : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float turn;
    [SerializeField] private float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, turn * Time.deltaTime * turnSpeed));
    }
}
