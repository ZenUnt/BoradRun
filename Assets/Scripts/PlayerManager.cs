using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public LayerMask blockLayer; // ブロックレイヤー
    private Rigidbody2D rbody; // プレイヤー制御用Rigidbody2D

    private float jumpPower = 400;
    private bool goJump = false; // ジャンプしたか否か
    private bool canJump = false; // ジャンプ可能か否か

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        canJump =
            Physics2D.Linecast(transform.position - (transform.right * 0.3f),
                transform.position - (transform.up * 0.5f), blockLayer) ||
            Physics2D.Linecast(transform.position + (transform.right * 0.3f),
                transform.position - (transform.up * 0.5f), blockLayer);

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) { // 画面クリックかスペースキーでジャンプ
            PushJumpButton();
        }

        if (transform.position.y < -8) {
            GameOver();
        }
    }

    void FixedUpdate()
    {

        if (goJump) {
            rbody.AddForce(Vector2.up * jumpPower);
            goJump = false;
        }
    }

    // ジャンプボタンが押された時
    public void PushJumpButton() {
        if (canJump) {
            goJump = true;
        }
    }

    // ゲームオーバー
    public void GameOver() {
        Destroy(gameObject);
    }
}
