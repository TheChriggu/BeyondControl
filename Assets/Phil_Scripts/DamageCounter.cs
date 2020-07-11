using UnityEngine;
using UnityEngine.UI;

public class DamageCounter : MonoBehaviour
{
    #region Singleton
    public static DamageCounter instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one DamageCounter Instance!!!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    int combinedDamage = 0;
    public Text text;
    public Canvas canvas;

    public void AddPropertyDamage(int dollarz, Vector3 placeOfCrime)
    {
        combinedDamage += dollarz;
        text.text = combinedDamage + ".00 $";
        GameObject floatingText = new GameObject();
        floatingText.transform.parent = canvas.transform;
        FloatingNumbers txtScrpt = floatingText.AddComponent<FloatingNumbers>();
        txtScrpt.setNumber(dollarz);
        Vector3 textPos = Camera.main.WorldToScreenPoint(placeOfCrime);
        floatingText.transform.position = textPos;
    }
}
