using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//se encarga de cambiar las imagenes y textos en el panel frente al jugador
public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("Partes")]
    public TextMeshProUGUI text;
    public GameObject title;
    public TextMeshProUGUI titleText;
    public GameObject footer;
    public TextMeshProUGUI footerText;
    public Image image;
    public Animator animator;
    public AnimationImages anim;
    public EyeLog logger;

    [Header("Intro")]
    public BuoyData Intro;

    [HideInInspector]
    public List<Sprite> sprites;

    public void Awake()
    {
        Instance = this;    

        if (logger == null)
        {
            logger = FindObjectOfType<EyeLog>();
        }
    }

    public void Start()
    {
        if (Intro != null) if (Intro.title != string.Empty) AssignData(Intro);

        StartCoroutine("IntroCoroutine");

    }

    //apagar intro despues de 5s
    IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(5f);

        if (titleText.text == Intro.title) Hide();
    }

    //asigna los datos de una boya en el UI
    public void AssignData(BuoyData p)
    {
        logger.Reset();
        logger.client = p;

        //setear textos
        if (p.text != string.Empty)
        {
            text.gameObject.SetActive(true);
            text.text = p.text;
        } else
        {
            text.gameObject.SetActive(false);
        }

        if (p.title != string.Empty)
        {
            SetHeader(p.title);
        }

        if (p.footer != string.Empty)
        {
            SetFooter(p.footer);
        } else
        {
            SetFooter("");
            footer.SetActive(false);
        }

        //setear imagen o animacion
        anim.ResetAnim();
        if (p.image.Count > 0)
        {
            image.gameObject.SetActive(true);
            if (p.image.Count > 1)
            {
                anim.StartAnim(p.image);
            } else
            {
                image.sprite = p.image[0];
            }
        } else
        {
            image.gameObject.SetActive(false);
        }        
    }

    public void SetHeader(string s)
    {
        title.SetActive(true);
        titleText.text = s;
    }

    public void SetFooter(string s)
    {
        footer.SetActive(true);
        footerText.text = s;
    }

    //animacion de mostrar
    public void Show()
    {
        animator.ResetTrigger("Hide");
        animator.SetTrigger("Show");
    }

    //animacion de esconder
    public void Hide()
    {
        animator.ResetTrigger("Show");
        animator.SetTrigger("Hide");

        logger.RecordData();
    }
}
