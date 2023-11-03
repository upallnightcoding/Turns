using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCntrl : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float durationSec;
    [SerializeField] private float speed;
    [SerializeField] private float rotationAmount;

    void Start()
    {
        //StartCoroutine(RotateTile());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rotateTile(bool turn)
    {
        StartCoroutine(RotateTile(turn));
    }

    private IEnumerator RotateTile(bool turn)
    {
        float rotation = 0.0f;
        float amount = 0.0f;
        float clockWise = turn ? 1.0f : -1.0f;

        while (amount <= 90.0)
        {
            rotation += Time.deltaTime;
            transform.localRotation = Quaternion.Euler(new Vector3(0.0f, clockWise * amount, 0.0f));
            amount += curve.Evaluate(rotation);
            yield return null;
        }
    }

    private IEnumerator xRotateTile()
    {
        float seconds = 0.0f;
        float curveAmount = curve.Evaluate(seconds);

        while (curveAmount < 1.0f)
        {
            seconds += Time.deltaTime;
            curveAmount = curve.Evaluate(seconds);
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, curveAmount * speed, 0.0f));
            yield return null;
        }
    }
}
