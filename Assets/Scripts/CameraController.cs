using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform FollowTarget;
    public float FollowSpeed;
    public float FollowSnap;

    private void Update()
    {
        if(FollowTarget != null)
        {
            float z = transform.position.z;
            Vector2 position = transform.position;
            
            if(Vector2.Distance(transform.position, FollowTarget.position) < FollowSnap)
            {
                position.x = FollowTarget.position.x;
            }
            else
            {
                position.x = Mathf.Lerp(position.x, FollowTarget.position.x, FollowSpeed * Time.deltaTime);
            }

            transform.position = new Vector3(position.x, position.y, z);
        }
    }
}
