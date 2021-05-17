using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : Openable
{
    public static LoadingScreen instance;

    //transform of the info text
    public Transform text;
    public TextMeshProUGUI infoTextField;
    private UIAnimation animationInfoText;
    private UIAnimation animationScreen;

    private string[] infoText =
    {
        "Benutze W/A/S/D zum laufen",
        "Manche Dungeons sind nur zu viert schaffbar",
        "Wenn du mal nicht weiter kommst, kannst du jeder Zeit ohne Strafe die Runde abbrechen",
        "Vielleicht gewinnst du etwas wenn du die Bestzeit schlägst",
        "Wenn du einen Gegner schlägst, der die gleiche Farbe wie du hat, verursachst du mehr Schaden",
        "Eier und Salz, Milch und Mehl",
        "Auf, auf und davon!",
        "Vorwärts immer, rückwärts nimmer",
        "Nach meiner Kenntnis ist das sofort",
        "Niemand hat die Absicht einen Dungeon zu errichten",
        "Kein Walkout",
        "So ein geiles Produkt",
        "Wer das liest, niest",
        "Wer anderen eine Bratwurst brät, der hat ein Bratwurstbratgerät",
        "Immer wenn ich traurig bin, trink ich einen Korn",
        "Ba-Ba-Banküberfall",
        "Wie eine Fatamorgana",
        "Mister Meier bitte sei mein Samurai",
        "Für Garderobe keine Haftung",
        "Gut gebügelt ist halb genäht",
        "Die A, B, C und die 6",
        "Haben wir noch Pepsi? Garkeine mehr?",
        "Wer ein Smiley zu viel macht, der hat irgendwann auch nixmehr zu lachen",
        "Ich bin der Held der Steine, in Frankfurt am Main, im Herzen von Europa, in meinem wunderschönen kleinen Lädchen," +
        "an einem fantastischen Tag und schaut mal, was ich euch mitgebracht habe :)",
        "Die Abenteuer der Rückseite",
        "Na Hallo wie gehts denn so",
        "General Genobi, euer Wagemut ist beeindruckend",
        "Häuser können nicht brennen",
        "Leute haut ma einen raus",
        "Die Sonne scheint, das Bier ist kalt",
        "Qualle",
        "100% futureknick frei",
        "anatomisch komisch",
        "Rehformation",
        "Ich hab deine Nase - Scheiße er hat ne Nase",
        "Gott ist Brot und wir haben ihn getoastet",
        "Schau mein Pferd, mein Pferd ist unglaublich",
        "Jede Farbe kann besonders gut mit einem Element umgehen",
        "Wir sind die wahren Spezialisten, unsere Spezialausrüstung ist dort in den Kisten",
        "Wir produzieren Fleisch - Fleisch, Fleisch, Fleisch",
        "Wein auf Bier, das rat ich dir. Bier auf Wein ... das rat ich dir"
    };

    //Helps with blocking UI
    private GraphicRaycaster rc;


    public override void Awake()
    {
        base.Awake();
        if (instance != null) Destroy(this);
        instance = this;
        text = transform.Find("Screen").Find("TextInfo");
        infoTextField = text.GetComponent<TextMeshProUGUI>();
        rc = transform.GetComponent<GraphicRaycaster>();


        animationScreen = new Fade();
        animationScreen.target = transform;
        // THIS DOES NOT TRACK FIX THIS LATER!
        /*
        animationScreen.InEndedEvent.AddListener( () => { UnityEngine.Debug.Log("FINISHED"); Finished = true; });
        animationScreen.OutEndedEvent.AddListener(()=> { UnityEngine.Debug.Log("FINISHED"); Finished = true; });
        */
        animationInfoText = new Fade();
        animationInfoText.target = text;
    }


    public override void OnOpen()
    {
        animationScreen.Play();
    }

    public override void OnClose()
    {
        animationScreen.Play();
    }
}