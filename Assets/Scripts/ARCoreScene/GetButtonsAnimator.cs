using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт для включения анимации появления кнопок взаимодействия с объектами мебели.
/// </summary>
public class GetButtonsAnimator : MonoBehaviour
{
    //Панель хранения кнопок.
    [SerializeField]
    private Canvas panelObj;

    [SerializeField]
    private Canvas panelNetwork;

    [SerializeField]
    public Button create;
    [SerializeField]
    public Button join;
    [SerializeField]
    public Button leave;

    private HintsScript hintsScript;

    public static GetButtonsAnimator Instace { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instace = this;


        //Получаем компонент-кнопку, к которой привязаны.
        var btn = GetComponent<Button>();

        btn.onClick.AddListener(StartAnim);
    }

    // Update is called once per frame
    void Update()
    {
        if (hintsScript == null)
        {
            hintsScript = HintsScript.Instance;
        }
    }

    private IEnumerator AddTip()
    {
        string hint = "\nСверху появились кнопки работы с сетью. " +
              "Слева направо: СОЗДАТЬ, ПРИСОЕДИНИТЬСЯ, ВЫЙТИ из комнаты.\n";
        hintsScript.AddToTextHint(hint, 2);
        yield return new WaitForSeconds(2f);
        hint = "Справа сбоку кнопки для работы с объектами в сцене. Сверху вниз: ВРАЩАТЬ, УДАЛЯТЬ и ПОКАЗАТЬ объекты.\n" +
        "Для получения объектов мебели НАЖМИ на кнопку с желтым креслом.\n";
        hintsScript.AddToTextHint(hint, 2);
    }

    /// <summary>
    /// Устанавливает флаги для анимации отображения кнопок.
    /// </summary>
    private void StartAnim()
    {
        if (HintsScript.hintId < 2)
        {
            StartCoroutine(AddTip());
        }
        Debug.Log($"Animation GetButtonsAnimator.");

        //Блок с анимацией кнопок для объектов.
        Animator anim = panelObj.GetComponent<Animator>();
        Animator anim1 = GetComponent<Animator>();

        if (anim != null)
        {
            bool isOpen = anim.GetBool("opened");
            anim.SetBool("opened", !isOpen);
        }

        if (anim1 != null)
        {
            bool isOpen = anim1.GetBool("open");
            anim1.SetBool("open", !isOpen);
        }

        //Блок с анимацией кнопок для сети.
        Animator animNetwork = panelNetwork.GetComponent<Animator>();

        bool isOpened = false;
        if (animNetwork != null)
        {
            isOpened = animNetwork.GetBool("getopen");
            animNetwork.SetBool("getopen", !isOpened);
        }

        if (!isOpened)
        {
            /*Делаем выключенными соединиться, создать, если мы на сервере,
             и включенным выйти, если на сервере.*/
            create.interactable = !LobbyManagerUnity.IsNetwork;
            join.interactable = !LobbyManagerUnity.IsNetwork;
            leave.interactable = LobbyManagerUnity.IsNetwork;
        }
    }

    //Вызывается при уничтожении MonoBehaviour.
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
