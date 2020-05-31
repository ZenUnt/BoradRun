﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    // 定数定義
    private const int RESPAN_TIME = 8600; // ブロックが発生する時間間隔

    // データセーブ用キー
    private string KEY_HIGH_SCORE;

    // オブジェクト参照
    public GameObject[] blockPrefabs; // ブロックプレハブ
    public GameObject map;
    public Text textScore;
    public Text textHighScore;
    public Text textNewRecord;
    public GameObject FinishedUI;
    public Text textFinisfed;

    // プライベート変数
    private float runDistance;  // 走行距離
    private int highScore;

    private DateTime lastDateTime;

    void Start() {
        PlayerPrefs.DeleteAll();
        runDistance = 0f;
        lastDateTime = DateTime.UtcNow;
        Time.timeScale = 1.0f;
        KEY_HIGH_SCORE = "HIGH_SCPRE" + SceneManager.GetActiveScene().name;
        highScore = PlayerPrefs.GetInt(KEY_HIGH_SCORE);
        textHighScore.text = highScore.ToString();

        CreateNewBlock();
    }

    void Update() {
        textScore.text = runDistance.ToString("0");
    }

    private void FixedUpdate() {
        TimeSpan timeSpan = DateTime.UtcNow - lastDateTime;

        // RESPAN_TIME秒毎にブロックを生成
        if (timeSpan >= TimeSpan.FromMilliseconds(RESPAN_TIME)) {
            CreateNewBlock();
            lastDateTime += timeSpan;
        }
    }

    // 新しいブロックの生成
    public void CreateNewBlock() {
        int rand;
        if (runDistance < 30f) {
            rand = RandomInt(0, 1);
        } else if (runDistance < 200f) {
            rand = RandomInt(2, 4);
        } else {
            rand = RandomInt(2, blockPrefabs.Length - 1);
        }
        GameObject block = (GameObject)Instantiate(blockPrefabs[rand]);
        block.transform.SetParent(map.transform, false);
        block.transform.localPosition = new Vector3(
            30f,
            -5f,//UnityEngine.Random.Range(-2.0f, 5.0f),
            0f);
        BlockManager b = block.GetComponent<BlockManager>();
        b.SetSpeed(0.08f);
    }

    // 走行距離を加算
    public void AddRunDistance(float dist) {
        runDistance += dist;
    }

    // ゲームオーバー
    public void GameOver() {
        Time.timeScale = 0;
        int score = (int)runDistance;
        if (score > highScore) {
            PlayerPrefs.SetInt(KEY_HIGH_SCORE, score);
            PlayerPrefs.Save();
            textNewRecord.text = "\\\\ 新記録! //";
        }
        textFinisfed.text = "今回の記録：" + score.ToString() + "m";
        
        FinishedUI.SetActive(true);
    }

    // リスタート
    public void RestartGameScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ホーム画面に戻る
    public void LoadHomeScene() {
        SceneManager.LoadScene("HomeScene");
    }

    // xからyの間のランダムな整数を返す
    private int RandomInt(int x, int y) {
        return UnityEngine.Random.Range(x, y + 1);
    }
}
