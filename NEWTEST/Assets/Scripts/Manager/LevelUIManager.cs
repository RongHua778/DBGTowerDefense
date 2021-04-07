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
        NoTargetEffectConfig effect1 = new NoTargetEffectConfig(NoTargetBuffName.Overload, 15, 2f);
        NoTargetEffectConfig effect2 = new NoTargetEffectConfig(NoTargetBuffName.FastConveyor, 15, 1f);
        List<NoTargetEffectConfig> effects = new List<NoTargetEffectConfig> { effect1 ,effect2};
        LevelManager.Instance.ApplyNoTargetEffects(effects);
        Debug.Log("Speed Up");
    }

    public void UpdateMoneyTxt(int amount)
    {
        _money_Txt.text = amount.ToString();
    }


}
