
using UnityEngine;
using UnityEngine.UI;

public class ShowLvExp : MonoBehaviour
{
    [SerializeField]
    private Text LvText;
    [SerializeField]
    private Text TechExp;
    [SerializeField]
    private Text Exp;
    [SerializeField]
    private Image ExpBarImage;

    private void OnValidate()
    {
        SetExpPanelUpdate();
    }

    public void SetExpPanelUpdate()
    {
        LvText.text = "Lv." + string.Format("{0:D2}", GameManager.PlayerLv);
        TechExp.text = GameManager.TechExp.ToString();
        Exp.text = GameManager.currentExp.ToString() + "/" + GameManager.RquireExp[GameManager.PlayerLv - 1].ToString();
        ExpBarImage.fillAmount = GameManager.Exp / GameManager.RquireExp[GameManager.PlayerLv - 1];
    }

    private void Update()
    {
        SetExpPanelUpdate();
    }
}
