using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;

    //public static UIManager Instance { 
    //    get 
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = FindObjectOfType<UIManager>();
    //        }
    //        return _instance;
    //    }
    //}

    private void Awake()
    {
        _instance = this;
    }

    [SerializeField] private TextMeshProUGUI coinText;

    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }


}
