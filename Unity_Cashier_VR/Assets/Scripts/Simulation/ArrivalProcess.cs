using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ArrivalProcess : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform customerSpawnPlace;
    public Transform customerLineupPoint;
    public Transform customerServicePoint;
    public Transform customerLeavePoint;
    public SimulationParameters simulationParameters;
    public Image progressBar;
    public Slider timeScaleSlider;
    public Slider lambdaSlider;
    public Slider muSlider;
    public TextMeshProUGUI timeScaleText;
    public TextMeshProUGUI lambdaText;
    public TextMeshProUGUI muText;



    public bool generateArrivals;
    public bool servicePointOccupied = false;
    int priority = 100;

    Queue<NavMeshAgent> customers = new Queue<NavMeshAgent>();
    NavMeshAgent servicingCustomer = null;
    float serviceTime = 0;
    float serviceTimer = 0;
    
    void Start()
    {
        lambdaSlider.value = simulationParameters.lambda;
        muSlider.value = simulationParameters.mu;
        timeScaleSlider.value = Time.timeScale;
        timeScaleText.text = timeScaleSlider.value.ToString();
        lambdaText.text = lambdaSlider.value.ToString();
        muText.text = muSlider.value.ToString();
        
        StartCoroutine(GenerateArrivals());
    }

    IEnumerator GenerateArrivals()
    {
        while (generateArrivals)
        {
            var agent = Instantiate(customerPrefab, customerSpawnPlace.position, Quaternion.identity).GetComponent<NavMeshAgent>();
            agent.gameObject.SetActive(true);
            float interArrivalTime = -Mathf.Log(1 - UnityEngine.Random.value) / simulationParameters.lambda;
            print("Interarrival time: " + interArrivalTime);

            agent.avoidancePriority = priority--;

            customers.Enqueue(agent);
            
            if(customers.Count == 1)
            {
                agent.SetDestination(customerLineupPoint.position);
            }
            else
            {
                agent.SetDestination(new List<NavMeshAgent>(customers)[customers.Count - 2].transform.position);
            }

            yield return new WaitForSeconds(interArrivalTime);
        }
    }

    IEnumerator ServiceCustomer()
    {
        serviceTime = -Mathf.Log(1 - UnityEngine.Random.value) / simulationParameters.mu;
        yield return new WaitForSeconds(serviceTime);
        servicingCustomer.SetDestination(customerLeavePoint.position);
        servicePointOccupied = false;
        servicingCustomer = null;
        serviceTime = 0;
    }

    void Update()
    {
        for(int i=1; i<customers.Count; i++)
        {
            var list = new List<NavMeshAgent>(customers);
            var customer = list[i];
            customer.SetDestination(list[i-1].transform.position);
        }

        if(customers.Count > 0 && !servicePointOccupied)
        {
            var customer = customers.Peek();
            if(Vector3.Distance(customer.transform.position, customerLineupPoint.position) < customer.stoppingDistance)
            {
                servicePointOccupied = true;
                customer.SetDestination(customerServicePoint.position);
                servicingCustomer = customer;
                customers.Dequeue();
            }
            else
            {
                customer.SetDestination(customerLineupPoint.position);
            }
        }

        if(servicingCustomer != null && serviceTime <= 0)
        {
            if(Vector3.Distance(servicingCustomer.transform.position, customerServicePoint.position) < servicingCustomer.stoppingDistance)
            {
                StartCoroutine(ServiceCustomer());
            }
        }

        if(serviceTime > 0)
        {
            serviceTimer += Time.deltaTime;
            progressBar.fillAmount = serviceTimer / serviceTime;
        }
        else
        {
            progressBar.fillAmount = 0;
            serviceTimer = 0;
        }
    }

    public void ChangeTimeScale()
    {
        Time.timeScale = timeScaleSlider.value;
        timeScaleText.text = timeScaleSlider.value.ToString();
    }

    public void ChangeLambda()
    {
        simulationParameters.lambda = lambdaSlider.value;
        lambdaText.text = lambdaSlider.value.ToString();
    }

    public void ChangeMu()
    {
        simulationParameters.mu = muSlider.value;
        muText.text = muSlider.value.ToString();
    }
}
