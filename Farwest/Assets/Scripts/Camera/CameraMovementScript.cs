using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{

    float speed = 0.06f;
    float zoomSpeed = 10.0f;
    float rotateSpeed;

    float maxHeight = 40f;
    float minHeight = 4f;

    public float movement_x;
    public float movement_z;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 0.12f;
            zoomSpeed = 0.20f;
        }
        else
        {
            speed = 0.035f;
            zoomSpeed = 20.0f;
        }


        float hsp = transform.position.y * speed * Input.GetAxis("Horizontal");
        float vsp = transform.position.y * speed * Input.GetAxis("Vertical");
        if(transform.position.y < 1.5f)
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, 1.5f, 0);
        }
        float scrollSp = Mathf.Log(transform.position.y) * 150 * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel");



        movement_x = Input.GetAxisRaw("Horizontal") * 20 * Time.deltaTime;
        movement_z = Input.GetAxisRaw("Vertical") * 20 * Time.deltaTime;

        gameObject.transform.Translate(0, 0, scrollSp, Space.Self);
        gameObject.transform.position = gameObject.transform.position + new Vector3(hsp * 100 * Time.deltaTime, 0, vsp * 100 * Time.deltaTime);


    }
}
