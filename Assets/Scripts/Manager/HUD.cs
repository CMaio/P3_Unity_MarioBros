using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour, IRestartGame
{
    [SerializeField] Text score;
    [SerializeField] Image healthUI;
    [SerializeField] GameObject die;
    [SerializeField] Color deadColor, healthyColor;
    [SerializeField] GameManager gm;
    [SerializeField] GameObject allHealthUI;
    [SerializeField] int secondsToShowHealthUI;
    private void Start()
    {
        DependencyContainer.GetDependency<IScoreManager>().scoreChangedDelegate += updateScore;
        DependencyContainer.GetDependency<ILifeManager>().lifeChangedDelegate += changeHealth;
        gm.addRestartListener(this);
    }
    private void OnDestroy()
    {
        DependencyContainer.GetDependency<IScoreManager>().scoreChangedDelegate -= updateScore;
        DependencyContainer.GetDependency<ILifeManager>().lifeChangedDelegate += changeHealth;

        gm.removeRestartListener(this);
    }
    public void updateScore(IScoreManager scoreManager)
    {
        score.text = "Score: " + scoreManager.getPoints().ToString("0");
    }
    public void changeHealth(ILifeManager lifeManager)
    {
        showHealthUI();
        healthUI.fillAmount = lifeManager.getLife();
        healthUI.color = Color.Lerp(deadColor, healthyColor, healthUI.fillAmount);
    }

    void IRestartGame.RestartGame()
    {
        die.SetActive(false);
    }

    void IRestartGame.Die()
    {
        die.SetActive(true);
    }
    void showHealthUI()
    {
        allHealthUI.SetActive(true);
        StartCoroutine(showHealthTime(secondsToShowHealthUI));
    }
    private IEnumerator showHealthTime(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        allHealthUI.SetActive(false);
    }
}