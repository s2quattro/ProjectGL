using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName;

    [SerializeField]
    private GameObject titleObjects;

    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private Text progressText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PressToStart()
    {
        titleObjects.SetActive(false);


        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation oper = SceneManager.LoadSceneAsync(gameSceneName);
        // 비동기식으로 씬을 불러온다.  
        oper.allowSceneActivation = false;
        // allowSceneActivation이 true가 되는 시점이 다음 씬으로 넘어가는 시점

        float timer = 0.0f;

        while (!oper.isDone)    // 씬 로딩이 완료되지 않았을때 반복
        {
            yield return null;

            timer += Time.deltaTime;    // 로딩이 너무 빠른경우를 대비해 타이머를 설정. 마지막 부분은 천천히 진행시킨다.

            if (oper.progress >= 0.9f)  // 바의 진행상황이 거의 다 찼을경우
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); // 로딩바를 보간하여 완료시킨다.
                progressText.text = ((int)(progressBar.fillAmount * 100f)).ToString() + " %";

                if (progressBar.fillAmount == 1.0f) // 로딩바가 다 찼을때
                {
                    oper.allowSceneActivation = true;   // 씬 전환을 허용
                    progressText.text = "100% 완료";
                }
            }
            else   // 진행 상태가 90퍼센트 이하일때 로딩바 처리
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, oper.progress, timer);
                progressText.text = ((int)(progressBar.fillAmount * 100f)).ToString() + " %";

                if (progressBar.fillAmount >= oper.progress)
                {
                    timer = 0f;
                }
            }
        }
    }
}