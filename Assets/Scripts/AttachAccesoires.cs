using UnityEngine;
using TMPro;

public class OrderUp : MonoBehaviour
{
    public GameObject BlankOrder;
    public GameObject FirstOrder;
    public GameObject SecondOrder;
    public GameObject ThirdOrder;
    public GameObject Camera;

    public TextMeshProUGUI scoreText;

    Vector3 target = new Vector3(79.2f, 1.22f, -10);

    private bool PressedButtonOnce = false;
    private bool PressedButtonTwice = false;
    private bool PressedButtonThrice = false;

    private bool HasStrawberry = false;
    private bool HasStar = false;
    private bool HasPiercing = false;
    //private bool HasBandaid = false;
    private bool HasCollar = false;

    //private bool blauwHead = false;
    private bool groenHead = false;
    private bool rozeHead = false;
    private bool roodHead = false;
    //private bool witHead = false;
    private bool zwartHead = false;

    private int orderState = 0;

    private int finalScore = 0;

    void Start()
    {
        BlankOrder.SetActive(true);
        FirstOrder.SetActive(false);
        SecondOrder.SetActive(false);
        ThirdOrder.SetActive(false);
    }

    void Update()
    {
        if (PressedButtonOnce)
        {
            BlankOrder.SetActive(false);
            FirstOrder.SetActive(true);
        }
        else if (PressedButtonTwice)
        {
            BlankOrder.SetActive(false);
            SecondOrder.SetActive(true);
        }
        else if (PressedButtonThrice)
        {
            BlankOrder.SetActive(false);
            ThirdOrder.SetActive(true);
        }
        else
        {
            BlankOrder.SetActive(true);
            FirstOrder.SetActive(false);
            SecondOrder.SetActive(false);
            ThirdOrder.SetActive(false);
        }

        if (Camera.transform.position == target)
        {
            if (FirstOrder.activeSelf)
            {
                finalScore = finalScore + 200;

                if (rozeHead) { finalScore = finalScore + 400; }
                if (HasStrawberry) { finalScore = finalScore + 200; }
                if (HasCollar) { finalScore = finalScore + 200; }
            }
            if (SecondOrder.activeSelf)
            {
                finalScore = finalScore + 200;

                if (groenHead) { finalScore = finalScore + 400; }
                if (HasStar) { finalScore = finalScore + 400; }
            }
            if (ThirdOrder.activeSelf)
            {
                finalScore = finalScore + 200;

                if (roodHead) { finalScore = finalScore + 400; }
                else if (zwartHead) { finalScore = finalScore + 400; }
                if (HasPiercing) { finalScore = finalScore + 200; }
                if (HasCollar) { finalScore = finalScore + 200; }
            }

            scoreText.text = "Money earned: " + finalScore.ToString();

            PressedButtonOnce = false;
            PressedButtonTwice = false;
            PressedButtonThrice = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Strawberry"))
        {
            HasStrawberry = true;
        }

        if (other.CompareTag("Star"))
        {
            HasStar = true;
        }

        if (other.CompareTag("Piercing"))
        {
            HasPiercing = true;
        }

        //if (other.CompareTag("Bandaid"))
        //{
        //    HasBandaid = true;
        //}

        if (other.CompareTag("Head canine black"))
        {
            zwartHead = true;
        }

        if (other.CompareTag("Head canine red"))
        {
            roodHead = true;
        }

        if (other.CompareTag("Head canine pink"))
        {
            rozeHead = true;
        }

        if (other.CompareTag("Head canine green"))
        {
            groenHead = true;
        }

        //if (other.CompareTag("Head canine blue"))
        //{
        //    blauwHead = true;
        //}

        //if (other.CompareTag("Head canine white"))
        //{
        //    witHead = true;
        //}
    }

    public void ButtonPress()
    {
        orderState += 1;
        finalScore = 0;

        HasStrawberry = false;
        HasStar = false;
        HasPiercing = false;
        //HasBandaid = false;
        zwartHead = false;
        roodHead = false;
        rozeHead = false;
        groenHead = false;
        //blauwHead = false;
        //witHead = false;

        if (orderState == 1)
        {
            PressedButtonOnce = true;
            PressedButtonTwice = false;
            PressedButtonThrice = false;
        }
        else if (orderState == 2)
        {
            PressedButtonOnce = false;
            PressedButtonTwice = true;
            PressedButtonThrice = false;
        }
        else if (orderState == 3)
        {
            PressedButtonOnce = false;
            PressedButtonTwice = false;
            PressedButtonThrice = true;
        }
    }
}
