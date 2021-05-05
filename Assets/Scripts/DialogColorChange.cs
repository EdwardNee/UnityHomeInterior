using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogColorChange : MonoBehaviour
{
    [SerializeField]
    private Transform shadowPanel;
    [SerializeField]
    private Button closeBtn;
    private Transform panel;
    private ProgramManager programManagerScript;

    // Start is called before the first frame update
    public void StartThis()
    {
        panel = GetComponent<Transform>();
        panel.gameObject.SetActive(true);
        shadowPanel.gameObject.SetActive(true);
        programManagerScript = FindObjectOfType<ProgramManager>();
        closeBtn.onClick.AddListener(Close);
        programManagerScript.colorChangedDel += Close;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Close()
    {
        panel.gameObject.SetActive(false);
        shadowPanel.gameObject.SetActive(false);
    }
}
