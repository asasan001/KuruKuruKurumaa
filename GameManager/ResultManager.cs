using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : SingletonMonoBehaviour<ResultManager>
{
    [SerializeField] private Canvas ingameCanvas;
    [SerializeField] private Canvas resultCanvas;
    [SerializeField] private Text finalPointText = null;
    private int finalPoint;
    // Start is called before the first frame update
    void Start()
    {
        ingameCanvas.gameObject.SetActive(true);
        resultCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DisplayResult(int point)
    {
        ingameCanvas.gameObject.SetActive(false);
        resultCanvas.gameObject.SetActive(true);
        finalPoint = point;
        finalPointText.text = point.ToString();
    }
    public void OnTwitterButton()
    {
        string s = "くるくるまわって" + finalPoint.ToString() + "点ゲットしたけど、貴殿は私の記録を越えられるかな";
        naichilab.UnityRoomTweet.Tweet("kurukurukurumaa", s, "unityroom", "unity1week");
    }
    public void OnRankingButton() {
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(finalPoint);
    }
    public void OnRestartButton()
    {
        SceneManager.LoadScene("RunScene");
    }
}
