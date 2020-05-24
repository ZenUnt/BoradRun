using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{

    private float speed; // ブロックが左に流れるスピード

    void Start() {
        speed = 0.08f;
    }

    void FixedUpdate() {
        this.transform.Translate(-speed, 0, 0);

        if (transform.position.x < -10) {
            Destroy(gameObject);
        }
    }
}
