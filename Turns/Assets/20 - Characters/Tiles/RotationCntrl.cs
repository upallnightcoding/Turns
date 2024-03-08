using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCntrl : MonoBehaviour
{
    private float rotateSpeed = 5.0f;
    private bool turnTile = false;
    private float direction = 1.0f;
    private float startDeg, endDeg;
    private float rotate = 0.0f;
    private float t = 0.0f;
    private float lastRotation = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (turnTile)
        {
            rotate = Mathf.Lerp(startDeg, endDeg, t);
            transform.Rotate(0.0f, (rotate - lastRotation) * direction, 0.0f);
            lastRotation = rotate;

            if (t > 1.0f)
            {
                turnTile = false;
            }

            t += rotateSpeed * Time.deltaTime;
        }
    }

    /*public void RotateTile(bool turn)
    {
        if (turn)
        {
            StartCoroutine(RotateTileClockwise());
        } else
        {
            StartCoroutine(RotateTileCounterClockwise());
        }
        
    }*/

    public void TurnTile(bool turn)
    {
        direction = (turn) ? 1.0f : -1.0f;
        startDeg = transform.rotation.y;
        endDeg = transform.rotation.y + 90.0f;
        lastRotation = startDeg;
        t = 0.0f;
        turnTile = true;
    }

    /*private IEnumerator RotateTileClockwise()
    {
        float direction = 1.0f;
        float delta = 0.0f;
        float amount = 0.0f;

        while (amount <= 90.0f)
        {
            transform.RotateAround(transform.position, Vector3.up, direction * delta);
            yield return null;

            delta = rotateSpeed * 0.001f; // Time.deltaTime;
            amount += delta;
        }

        delta = amount - 90.0f;
        transform.RotateAround(transform.position, Vector3.up, direction * delta);
        yield return null;
    }*/

    /*private IEnumerator RotateTileCounterClockwise()
    {
        float direction = -1.0f;
        float delta = 0.0f;
        float amount = 0.0f;

        while (amount <= 90.0f)
        {
            transform.RotateAround(transform.position, Vector3.up, direction * delta);
            yield return null;

            delta = rotateSpeed * 0.001f; // Time.deltaTime;
            amount += delta;
        }

        delta = amount - 90.0f;
        transform.RotateAround(transform.position, Vector3.up, direction * delta);
        yield return null;
    }*/
}
