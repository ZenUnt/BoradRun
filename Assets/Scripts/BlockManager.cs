using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    // オブジェクト参照
    public LayerMask defaultLayer;  // ブロックレイヤー
    public GameObject GameManager;  // ゲームマネージャー

    //プライベート変数
    private float speed; // ブロックが左に流れるスピード
    private float prevX; // 全フレームのx座標

    void Start() {
        speed = 0.08f;
        prevX = transform.position.x;
    }
    
    void FixedUpdate() {
        this.transform.Translate(-speed, 0, 0);

        if (transform.position.x < -10) {
            if (gameObject.layer == defaultLayer) { // 走行距離計算用の特殊ブロックは無限ループ
                transform.Translate(30, 0, 0);
                prevX += 30;
            } else {
                Destroy(gameObject);
            }
        }

        if (gameObject.layer == defaultLayer) { // 走行距離計算用の特殊ブロック
            GameManager.GetComponent<GameManager>().AddRunDistance(prevX - transform.position.x);
            prevX = transform.position.x;
        }
    }
}
