using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour {

    public Transform FollowTarget;
    public float FollowSpeed;
    public float FollowSnap;
    public bool LockY;


    public void SnapToTarget()
    {
        if(FollowTarget != null)
        {
            transform.position = FollowTarget.position.ToVector2().ToVector3(transform.position.z);
        }
    }

    private void Update()
    {
        if(FollowTarget != null)
        {
            if (transform.position != FollowTarget.position)
            {
                float z = transform.position.z;
                Vector2 destination = FollowTarget.position;
                Vector2 distance = destination - transform.position.ToVector2();
                Vector2 direction = distance.normalized;

                Vector2 position = transform.position.ToVector2() + direction * FollowSpeed * Game.TimeScale * Time.deltaTime;

                if (distance.x < 0 && position.x < destination.x ||
                    distance.x > 0 && position.x > destination.x)
                {
                    position.x = destination.x;
                }

                if (distance.y < 0 && position.y < destination.y ||
                    distance.y > 0 && position.y > destination.y)
                {
                    position.y = destination.y;
                }

                if(LockY)
                {
                    position.y = transform.position.y;
                }

                transform.position = position.ToVector3(z);
            }
        }
        else
        {

        }
    }
}
