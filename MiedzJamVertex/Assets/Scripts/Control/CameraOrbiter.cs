using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbiter : MonoBehaviour
{
    enum PlayingState { Before, Playing, After }

    [SerializeField] Transform orbitPoint;
    [SerializeField] float startTime = 5f;
    [SerializeField] float endTime = 35f;
    [SerializeField] float speed = 3f;

    PlayingState state = PlayingState.Before;
    private float lastChangeTime = 0f;

    private void Update()
    {
        lastChangeTime += Time.deltaTime;

        if(state == PlayingState.Before && lastChangeTime >= startTime)
        {
            lastChangeTime = 0f;
            state = PlayingState.Playing;
        }
        if(state == PlayingState.Playing && lastChangeTime >= endTime)
        {
            lastChangeTime = 0f;
            state = PlayingState.After;
        }

        TryOrbit();
    }

    private void TryOrbit()
    {
        if (state != PlayingState.Playing)
            return;

        Vector3 toOrbit = orbitPoint.position - Camera.main.transform.position;
        Vector3 newToOrbit = Quaternion.Euler(0, speed * Time.deltaTime, 0) * toOrbit;

        Camera.main.transform.position = orbitPoint.position - newToOrbit;
        Camera.main.transform.LookAt(orbitPoint);
    }
}
