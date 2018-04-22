using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplaySharedFloat : MonoBehaviour {

    public SharedFloat DisplayFloat;
    private TextMeshProUGUI text;
    public string preString;
    public string postString;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update () {
        text.text = preString + DisplayFloat.val.ToString("N0") + postString;
	}
}
