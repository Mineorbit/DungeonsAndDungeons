using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerListItem : MonoBehaviour
{
    public TMP_Text nameField;
    // Start is called before the first frame update
    void Start()
    {
        nameField = transform.Find("Name").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void close()
    {
        this.gameObject.SetActive(false);
    }
    public void open()
    {
        this.gameObject.SetActive(true;
    }
    public void set(PlayerData p)
    {
        nameField.text = p.name;
    }
}
