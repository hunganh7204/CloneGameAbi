using UnityEngine;
using TMPro;

public class CombatText : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI hpText;
    public void OnInit(float damage)
    {
        hpText.text = damage.ToString();
        Invoke(nameof(OnDespawn), 1f);
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }
}
