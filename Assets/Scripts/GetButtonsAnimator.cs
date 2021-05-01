using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetButtonsAnimator : MonoBehaviour
{
    [SerializeField]
    private Canvas panel;

    // Start is called before the first frame update
    void Start()
    {
        //Получаем компонент-кнопку, к которой привязаны.
        var btn = GetComponent<Button>();

        btn.onClick.AddListener(StartAnim);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void StartAnim()
    {
        Debug.Log("Stat");
        Animator anim = panel.GetComponent<Animator>();

        if (anim != null)
        {
            bool isOpen = anim.GetBool("opened");
            anim.SetBool("opened", !isOpen);
        }
    }
}
