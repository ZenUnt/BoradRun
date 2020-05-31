using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

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
    private int RESPAN_TIME = 8600; // ブロックが発生する時間間隔(ミリ秒)
    private int highScore;
    private DateTime lastCreateBlockTime;
    private TimeSpan timeSpan;

    private string gameSceneName;

    void Start() {
        //PlayerPrefs.DeleteAll();
        runDistance = 0f;
        Time.timeScale = 1.0f;
        gameSceneName = SceneManager.GetActiveScene().name;
        KEY_HIGH_SCORE = "HIGH_SCPRE" + gameSceneName;
        highScore = PlayerPrefs.GetInt(KEY_HIGH_SCORE);
        textHighScore.text = highScore.ToString();

        lastCreateBlockTime = DateTime.UtcNow;
        CreateNewBlock();
    }

    void Update() {
        textScore.text = ((int)runDistance).ToString();
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
        if (gameSceneName == "GameScene") {
            int rand;
            float blockY = -5f;
            if (runDistance < 20f) {
                rand = RandomInt(0, 1);
            } else if (runDistance < 200f) {
                rand = RandomInt(2, 4);
            } else if (runDistance < 500f) {
                rand = RandomInt(2, blockPrefabs.Length - 2);
            } else {
                rand = blockPrefabs.Length - 1;
                RESPAN_TIME = 300;
                blockY = UnityEngine.Random.Range(-2.0f, 5.0f);
            }
            GameObject block = (GameObject)Instantiate(blockPrefabs[rand]);
            block.transform.SetParent(map.transform, false);
            block.transform.localPosition = new Vector3(
                30f,
                blockY,//UnityEngine.Random.Range(-2.0f, 5.0f),
                0f);
            BlockManager b = block.GetComponent<BlockManager>();
            b.SetSpeed(0.08f);
        } else {
            RESPAN_TIME = 300;
            GameObject block = (GameObject)Instantiate(blockPrefabs[blockPrefabs.Length - 1]);
            block.transform.SetParent(map.transform, false);
            block.transform.localPosition = new Vector3(
                27f,
                UnityEngine.Random.Range(-2.0f, 5.0f),
                0f);
            BlockManager b = block.GetComponent<BlockManager>();
            b.SetSpeed(0.08f);
        }
    }

    // 走行距離を加算
    public void AddRunDistance(float dist) {
        runDistance += dist;
    }

    // ゲームオーバー
    public void GameOver() {
        int score = (int)runDistance;
        textScore.text = score.ToString();
        Time.timeScale = 0;
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
