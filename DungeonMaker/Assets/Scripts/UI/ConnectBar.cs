using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectBar : MonoBehaviour
{
    public Button connectButton;
    public InputField nameField;
    public static ConnectBar current;
    public CanvasGroup slider;
    GameObject bar;
    public bool opened = true;
    public void Start()
     {
        current = this;
        bar = this.gameObject;
        connectButton = transform.Find("GO").GetComponentInChildren<Button>();
        nameField = transform.GetComponentInChildren<InputField>();
        connectButton.onClick.AddListener(Connect);
        slider = GetComponent<CanvasGroup>();

    }
    void Connect()
    {
        Client.instance.name = nameField.text;
        Client.instance.ConnectToMainServer();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void open()
    {
        if(!opened)
        {
        StartCoroutine("FadeIn");
        opened = true;
        }
    }
    public void close()
    {
        if(opened)
        {
        StartCoroutine("FadeOut");
        opened = false;
        }
    }
    IEnumerator FadeOut () {
		for (float ft = 1f; ft >= 0; ft -= 0.1f) {
			slider.alpha = ft;
			yield return null;
		}
		slider.alpha = 0;
		bar.SetActive(false);
        slider.interactable = false;
        slider.blocksRaycasts = false;
		yield return null;
	}

	IEnumerator FadeIn () {

		bar.SetActive(true);
		for (float ft = 0f; ft <= 1; ft += 0.1f) {
			slider.alpha = ft;
			yield return null;
		}
		slider.alpha = 1;
        slider.interactable = true;
        slider.blocksRaycasts = true;

		yield return null;
	}
}
