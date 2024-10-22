using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    void Update()
    {

        if (!IsOwner)
        {
            return;
        }

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKeyDown(KeyCode.W)) moveDir.z = +5f;
        if (Input.GetKeyDown(KeyCode.S)) moveDir.z = -5f;
        if (Input.GetKeyDown(KeyCode.D)) moveDir.x = -5f;
        if (Input.GetKeyDown(KeyCode.A)) moveDir.x = +5f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
