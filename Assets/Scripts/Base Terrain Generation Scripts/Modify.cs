//This class implements the Terrain class to allow the dynamic modification of the terrain.
//Dylan Buchheim, 2019

using System.Collections;
using UnityEngine;

public class Modify : MonoBehaviour
{
    Vector2 rot;
    public float flightSpeed = 1.25f;
    public float mouseSpeed = 2;
    public bool onFPC = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100)) {
                Terrain.SetBlock(hit, new BlockAir());
            }
        }
        if(Input.GetKeyDown(KeyCode.Mouse1)) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
                Terrain.SetBlock(hit, new Block(), true);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            flightSpeed *= 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            flightSpeed /= 2;
        }

        if (!onFPC) {
            rot = new Vector2(rot.x + Input.GetAxis("Mouse X") * mouseSpeed, rot.y + Input.GetAxis("Mouse Y") * mouseSpeed);

            transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

            transform.position += transform.forward * flightSpeed * Input.GetAxis("Vertical");
            transform.position += transform.right * flightSpeed * Input.GetAxis("Horizontal");
        }
        
    }
}
