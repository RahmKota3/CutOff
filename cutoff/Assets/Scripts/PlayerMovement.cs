using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    PlayerStats stats;

    void CheckInput()
    {
        Vector3 moveDir = Vector3.zero;

        if (Input.GetButtonDown("Left")) {
            if (transform.position.x - 1 >= 0)
                moveDir.x = -1;
        }
        else if (Input.GetButtonDown("Right")) {
            if (transform.position.x + 1 < DEFINES.MAP_WIDTH)
                moveDir.x = 1;
        }
        else if (Input.GetButtonDown("Up")) {
            if (transform.position.y + 1 < DEFINES.MAP_HEIGHT)
                moveDir.y = 1;
        }
        else if (Input.GetButtonDown("Down")) {
            if (transform.position.y - 1 >= 0)
                moveDir.y = -1;
        }

        if (moveDir == Vector3.zero)
            return;
        
        transform.position += moveDir;

        MapManager.Instance.SubmitMove((int)transform.position.x, (int)transform.position.y, stats.Id);
    }

    private void Awake() {
        stats = GetComponent<PlayerStats>();
    }

    private void Update() {
        CheckInput();
    }
}
