using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    // 定数定義
    private const int JUMP_BORDER_TIME = 180; // 小ジャンプと大ジャンプの境界のミリ秒数

    public LayerMask blockLayer;    // ブロックレイヤー
    private Rigidbody2D rbody;      // プレイヤー制御用Rigidbody2D

    private float jumpPower = 300;   // ジャンプパワー

    private bool canJump = false;   // ジャンプ可能か否か
    private bool canSmallJump = true;// スモールジャンプ可能か否か
    private bool goJump = false;    // ジャンプしたか否か
    private DateTime mouseDownTime; // 画面がクリックされた時刻
    private TimeSpan spanMouseDownUp; // マウスがクリックされてから離されるまでの時間
    private GameObject GameManager;

    void Start() {
        rbody = GetComponent<Rigidbody2D>();
        GameManager = GameObject.Find("GameManager");
    }

    void Update() {
        canJump =
            Physics2D.Linecast(transform.position - (transform.right * 0.3f),
                transform.position - (transform.up * 0.5f), blockLayer) ||
            Physics2D.Linecast(transform.position + (transform.right * 0.3f),
                transform.position - (transform.up * 0.5f), blockLayer);

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) { // 画面クリックかスペースキーでジャンプ
            mouseDownTime = DateTime.UtcNow;
            PushJumpButton();
        }

        // 押す時間が短ければ小ジャンプ
        if (Input.GetMouseButtonUp(0) && canSmallJump) {
            spanMouseDownUp = DateTime.UtcNow - mouseDownTime;
            if (spanMouseDownUp < TimeSpan.FromMilliseconds(JUMP_BORDER_TIME)) {
                rbody.AddForce(Vector2.down * jumpPower * 0.4f);
            }
            canSmallJump = false;
        }

        if (transform.position.y < -8) {
            GameOver();
        }
    }

    void FixedUpdate() {
        if (goJump) {
            rbody.AddForce(Vector2.up * jumpPower);
            goJump = false;
            canSmallJump = true;
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
        GameManager.GetComponent<GameManager>().GameOver();
    }
}
