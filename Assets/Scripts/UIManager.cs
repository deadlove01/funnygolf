using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    #region simple singleton

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

#endregion

    public Image arrowDirection;
    public Image circleImage;
    public Slider powerSlider;
    public Image powerImage;

    public Color phase1Color;
    public Color phase2Color;
    public Color phase3Color;

    public Text strokeText;
    public Text turnText;

    public GameObject gameoverPanel;
    public Text resultText;
    public GameObject loseTitle;
    public GameObject winTitle;

    public void ShowHideCircle(bool value)
    {
        if (circleImage == null)
            return;
        circleImage.gameObject.SetActive(value);
    }

    public void ShowHideArrow(bool value)
    {
        if (arrowDirection == null)
            return;
        arrowDirection.gameObject.SetActive(value);
    }


    public void DrawArrowDirection(Vector2 startPosition, Vector2 endPosition
        )
    {
        var vectorToTarget = startPosition - endPosition;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        arrowDirection.rectTransform.position = startPosition;
        arrowDirection.rectTransform.rotation = q;

        var distance = Vector2.Distance(startPosition, endPosition);
        // 210 => 100
        // 210 = 15f distance
        if (distance > 15f)
        {
            //print("minWidth: " + arrowDirection.rectTransform.rect.width);
            var newWidth = distance * arrowDirection.rectTransform.rect.width / 15f;
            var oldSize = arrowDirection.rectTransform.sizeDelta;
            //arrowDirection.rectTransform.sizeDelta = new Vector2(newWidth, oldSize.y);
            //print("new width: "+newWidth);
        }
       
    }

    public void UpdatePowerUI(float percent)
    {
        if (powerSlider == null || powerImage == null)
            return;
        powerSlider.value = percent;
        if (percent <= 0.5f)
            powerImage.color = phase1Color;
        else if(percent <=0.75f)
            powerImage.color = phase2Color;
        else
        {
            powerImage.color = phase3Color;
        }
    }


    public void UpdateTextUI(int stroke)
    {
        if (strokeText == null)
            return;
        strokeText.text = stroke.ToString();
    }

    public void UpdateTurnText(string name)
    {
        if (turnText == null)
            return;
        turnText.text = name;
    }

    public void ShowGameOverPanel(bool value, bool isPlayerWon)
    {
        gameoverPanel.SetActive(value);
        if (value)
        {
            winTitle.SetActive(isPlayerWon);
            loseTitle.SetActive(!isPlayerWon);
            resultText.gameObject.SetActive(true);
            resultText.text = isPlayerWon ? Constants.WIN_STR : Constants.LOSE_STR;
        }
    }

}
