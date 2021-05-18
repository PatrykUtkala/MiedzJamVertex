using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardController : MonoBehaviour
{
    public float newx = 0;
    public float oldx = 0;
    public float brake = 100;
    public float amount = 0;
    public float currentx = 0;
    public bool state = false;
    public float amount_scaler;
    Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        amount = Screen.width*amount_scaler;
        tr = gameObject.GetComponent<Transform>();
        oldx = tr.transform.position.x;
        newx = oldx + amount;
        currentx = oldx;
    }

    private void Update()
    {
        amount = Screen.width * amount_scaler;
        float x = tr.transform.position.x;
        if (Mathf.Abs(currentx - x) > 5)
            tr.transform.Translate((currentx-x)/100, 0, 0);
    }

    public void changeState()
    {
        if (state)
        {
            //tr.transform.position = new Vector3(newx, tr.transform.position.y, tr.transform.position.z);
            currentx = oldx;
            state = false;
        }
        else
        {
            //tr.transform.position = new Vector3(oldx, tr.transform.position.y, tr.transform.position.z);
            currentx = newx;
            state = true;
        }
    }

    public void hide()
    {
        currentx = oldx;
        state = false;
    }
}
