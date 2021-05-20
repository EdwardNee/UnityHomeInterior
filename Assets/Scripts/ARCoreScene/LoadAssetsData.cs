using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

/// <summary>
/// Класс загрузки данных с Addressable на кнопки.
/// </summary>
public class LoadAssetsData : MonoBehaviour
{
    //Место хранения кнопок элементов.
    [SerializeField]
    private GameObject contentHandler;
    //Кнопка, по которой происходит загрузка элементов.
    [SerializeField]
    private Button click;
    //Префаб кнопки для элементов мебели.
    [SerializeField]
    private GameObject prefab;
    //Загружено или нет.
    private bool isLoaded;

    private HintsScript hintsScript;

    //Метка, по которой нужно качать элементы.
    [SerializeField]
    private string _label;
    //Коллекция загружаемых элементов.
    private List<Element> loadingElements;


    public Text txt;

    //Start is called before the first frame update
    private void Start()
    {
        hintsScript = HintsScript.Instance;
        loadingElements = new List<Element>();
        click.onClick.AddListener(LoadingAssets);
    }

    /// <summary>
    /// Метод загрузки и установки ассетов.
    /// </summary>
    public async void LoadingAssets()
    {
        if (!isLoaded)
        {
            if (HintsScript.hintId == 2)
            {
                hintsScript.Off();
            }
            await GetDataAsync(_label);

            Element currElement;
            for (int i = 0; i < loadingElements.Count; i++)
            {
                currElement = loadingElements[i];
                GameObject b = Instantiate(prefab, contentHandler.transform);
                b.GetComponent<ChooseObject>().chosedObj = currElement.ElementPrefab;
                b.GetComponent<Image>().sprite = currElement.ElementSprite;
            }
            isLoaded = true;
            if (HintsScript.hintId == 2)
            {
                hintsScript.DisplayHint("Снизу появился прокручеваемый список с элементами мебели.\n" +
                    "Выберите и кликлине на иконку с интересующей мебелью.", 2);
            }
        }
    }

    /// <summary>
    /// Метод получения данных с Firebase Storage.
    /// </summary>
    /// <param name="label">Метка для загружаемых данных.</param>
    public async Task GetDataAsync(string label)
    {
        //Загружаем ссылки.
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;

        foreach (var item in locations)
        {
            var element = await Addressables.LoadAssetAsync<Element>(item).Task;
            loadingElements.Add(element);   //items
        }
        //txt.text += $"loadded {loadingElements.Count}"; 
    }

}
