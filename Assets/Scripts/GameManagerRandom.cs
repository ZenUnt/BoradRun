using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManagerRandom : MonoBehaviour {
    // 定数定義
    private const int RESPAN_TIME = 300; // ブロックが発生する時間間隔(ミリ秒)

    // データセーブ用キー
    private string KEY_HIGH_SCORE;

    // オブジェクト参照
    public GameObject blockPrefabs; // ブロックプレハブ
    public GameObject map;
    public Text textScore;
    public Text textHighScore;
    public Text textNewRecord;
    public GameObject FinishedUI;
    public Text textFinisfed;

    // プライベート変数
    private float runDistance;  // 走行距離
    private int highScore;
    TimeSpan timeSpan;

    private DateTime lastCreateBlockTime;

    void Start() {
        PlayerPrefs.DeleteAll();
        runDistance = 0f;
        Time.timeScale = 1.0f;
        KEY_HIGH_SCORE = "HIGH_SCPRE" + SceneManager.GetActiveScene().name;
        highScore = PlayerPrefs.GetInt(KEY_HIGH_SCORE);
        textHighScore.text = highScore.ToString();

        lastCreateBlockTime = DateTime.UtcNow;
        CreateNewBlock();
    }

    void Update() {
        textScore.text = runDistance.ToString("0");
    }

    private void FixedUpdate() {
        timeSpan = DateTime.UtcNow - lastCreateBlockTime;

        // RESPAN_TIME秒毎にブロックを生成
        if (timeSpan >= TimeSpan.FromMilliseconds(RESPAN_TIME)) {
            CreateNewBlock();
            lastCreateBlockTime = DateTime.UtcNow;
        }
    }

    private void OnApplicationPause(bool pause) {
        if (pause) {
            // アプリがバックグラウンドへ移行
        } else {
            // バックグラウンドから復帰
            lastCreateBlockTime = DateTime.UtcNow - timeSpan;
        }
    }

    // 新しいブロックの生成
    public void CreateNewBlock() {
        GameObject block = (GameObject)Instantiate(blockPrefabs);
        block.transform.SetParent(map.transform, false);
        block.transform.localPosition = new Vector3(
            27f,
            UnityEngine.Random.Range(-2.0f, 5.0f),
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
