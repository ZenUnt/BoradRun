using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    // 定数定義
    private const int RESPAN_TIME = 300; // ブロックが発生する時間間隔

    // オブジェクト参照
    public GameObject blockPrefab; // ブロックプレハブ
    public GameObject map;

    private DateTime lastDateTime;

    void Start() {

        lastDateTime = DateTime.UtcNow;
    }

    void Update() {

        TimeSpan timeSpan = DateTime.UtcNow - lastDateTime;

        // RESPAN_TIME秒毎にブロックを生成
        if (timeSpan >= TimeSpan.FromMilliseconds(RESPAN_TIME)) {
            CreateNewBlock();
            lastDateTime += TimeSpan.FromMilliseconds(RESPAN_TIME);
        }

    }

    // 新しいブロックの生成
    public void CreateNewBlock() {
        GameObject block = (GameObject)Instantiate(blockPrefab);
        block.transform.SetParent(map.transform, false);
        block.transform.localPosition = new Vector3(
            20f,
            UnityEngine.Random.Range(-3.0f, 5.0f),
            0f);
    }
}
