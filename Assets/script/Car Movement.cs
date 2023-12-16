using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CarMovement : MonoBehaviour
{
    public SplineContainer mainRoute;
    public SplineContainer reverseRoute;

   private SplineAnimate spline;

    private LevelManager levelManager;
    private GameManager gameManager;

    public string newTag = "moving";

    private HelicopterMovement HelicopterMovement;

    private bool reversed;

    private Transform arrow;
    private Transform arrowR;

    

    // Start is called before the first frame update
    void Start()
    {
        reversed = false;
        gameManager=FindFirstObjectByType<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();
        HelicopterMovement = FindFirstObjectByType<HelicopterMovement>();
        arrow = transform.Find("arrow");
        arrowR = transform.Find("arrowR");
        spline = GetComponent<SplineAnimate>();
        arrowR.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnMouseDown()
    {
        if (gameManager.reversing==false && gameManager.helicopter==false)
        {
            gameObject.tag = newTag;
            if (!reversed)
            {
                spline.Container = mainRoute;
            }
            spline.Restart(true);
           
        }
        else if (gameManager.reversing==true && gameManager.helicopter==false)
        {
            reversed = true;
            arrow.gameObject.SetActive(false);
            arrowR.gameObject.SetActive(true);
      
            spline.Container = reverseRoute;
            gameManager.reversing = false;
        }
        else if(gameManager.reversing==false && gameManager.helicopter==true)
        {
            gameManager.helicopter= false;
            HelicopterMovement.MoveHelicopter(transform);
        }
        else
        {
            Debug.Log("error in heli");
        }

    }

   
    public void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.CompareTag("car") )
        {
            spline.Pause();
            StartCoroutine(ReverseAnimation());

            gameManager.HealthUpdate(20);


        }
        if (collision.gameObject.CompareTag("human"))
        {
            spline.Pause();
            StartCoroutine(ReverseAnimation());
            gameManager.HealthUpdate(20);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("finish"))
        {
            levelManager.CarCount--;
            gameObject.SetActive(false);
        }
    }
    private IEnumerator ReverseAnimation()
    {
        float duration = spline.ElapsedTime;
        float elapsedTime = 0.001f;
       
        while (elapsedTime < duration && spline.ElapsedTime > 0f)
        {
            spline.ElapsedTime -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        gameObject.tag = "car";
    }

   
}
