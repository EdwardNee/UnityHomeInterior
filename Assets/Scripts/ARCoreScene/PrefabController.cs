using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// Скрипт-контроллер для текущего префаба.
/// </summary>
public class PrefabController : MonoBehaviourPun, IPunObservable
{
    //Индексы для материалов.
    public static int current_mat = -1;
    public static int old_mat = -1;

    /// <summary>
    /// Конструктор объекта класса.
    /// </summary>
    public static PrefabController Instance
    {
        get; private set;
    }

    /// <summary>
    /// Автосвойство задетого объекта.
    /// </summary>
    public static GameObject HittedObj { get; set; }

    //Название задетого объекта.
    public static string objName;

    //Префаб текущего объекта.
    public GameObject prefab;
    //Массив всех материалов.
    public Material[] materials;

    public Text txt;

    // Start is called before the first frame update
    void Start()
    {
        txt = GameObject.FindWithTag("Logger").GetComponent<Text>();
        materials = MaterialsColor.Instance.Materials;
    }

    // Update is called once per frame
    void Update()
    {

        if (old_mat != current_mat)
        {
            SetMaterial(current_mat);
        }
    }


    /// <summary>
    /// Устанавливает материал на выбор по индексу - изменение цвета.
    /// </summary>
    /// <param name="materialId">Индекс материала.</param>
    private void SetMaterial(int materialId)
    {
        txt.text = "SETMATER" + current_mat;
        old_mat = current_mat = materialId;

        try
        {
            txt.text += objName;
            var transformArray = GameObject.Find(objName).GetComponentsInChildren<Transform>();

            foreach (var e in transformArray)
            {
                txt.text += e.name + " ";
                if (e.name.Equals("obj"))
                {
                    e.GetComponent<MeshRenderer>().material = materials[materialId];
                }
            }
            //for (int i = 0; i < mesh.Count; i++)
            //{
            //    mesh[i].GetComponent<MeshRenderer>().material = material[_mat];
            //    txt.text += "loop\n";
            //}
        }
        catch (System.Exception ex)
        {
            txt.text += ex.Message + "\n";
        }
    }

    /// <summary>
    /// Вызывается 10 раз в секунду для передачи данных.
    /// Реализовано из IPunObservable.
    /// </summary>
    /// <param name="stream">Поток информации.</param>
    /// <param name="info">Информация о текущей операции.</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Мы пишем данные
        if (stream.IsWriting)
        {
            stream.SendNext(current_mat);
            stream.SendNext(objName);
        }
        //Иначе читаем.
        else
        {
            current_mat = (int)stream.ReceiveNext();
            objName = (string)stream.ReceiveNext();
        }
    }
}
