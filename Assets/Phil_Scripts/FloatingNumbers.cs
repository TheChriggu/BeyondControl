using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class FloatingNumbers : MonoBehaviour
{
    Text text;
    float timeofdeath;
    void Awake()
    {
        text = GetComponent<Text>();
        timeofdeath = Time.time + 0.5f;
    }

    public void setNumber(int dollarz)
    {
        text.text = dollarz + ".00 $";
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.font = ArialFont;
        text.material = ArialFont.material;
        text.color = Color.green;
    }

    void Update()
    {
        transform.position += Vector3.up * 50 * Time.deltaTime;
        if (timeofdeath < Time.time) Destroy(gameObject);
    }
}
