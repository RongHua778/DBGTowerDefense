using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : Singleton<LevelUIManager>
{

    [SerializeField] Text _money_Txt;
    [SerializeField] Button _speedUp_Btn;
    // Start is called before the first frame update
    void Start()
    {
        _speedUp_Btn.onClick.AddListener(SpeedUp);
    }

    private void SpeedUp()
    {
        Debug.Log("Speed Up");
    }

    public void UpdateMoneyTxt(int amount)
    {
        _money_Txt.text = amount.ToString();
    }


}
