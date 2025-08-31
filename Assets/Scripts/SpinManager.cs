using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinManager : MonoBehaviour
{
    public List<RollerCylinder> listReels = new List<RollerCylinder>();
    public float delayBetweenReels = 0.5f; // tiempo entre rodillos

    private int currentIndex = 0;
    private int currentIndexToStop = 0;

    private float timer = 0f;
    private float timer2 = 0f;      // contador para esperar la duración
    private float timerToStop = 0f; // duración aleatoria antes de empezar a detener

    private bool spinningSequence = false;
    private bool stoppingSequence = false;
    private bool durationSequence = false; // fase de duración
    public float speedRoller;

    public EnableButton enableButton;

    public SlotState state = SlotState.Idle;
    void Start()
    {
        state = SlotState.Starting;
        SwitchSlot();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            ButtonSpin();

        SwitchSlot();
        //SpinTheWheel();

        if (durationSequence)
        {
            timer2 += Time.deltaTime;

            // cuando el tiempo acumulado supera el tiempo asignado empezamos a detener
            if (timer2 >= timerToStop)
            {
                durationSequence = false;   // apagamos la espera
                stoppingSequence = true;    // encendemos la fase de detener
                timer2 = 0f;                // reseteamos contador
                timer = 0f;                 // reseteamos delay
                currentIndexToStop = 0;     // reiniciamos índice de parada
                state = SlotState.Stopping;
                Debug.Log("Estoy entrando a stopping");
            }
        }

        //StopTheWheel(); //ahora también se evalúa en Update
    }

    private void SpinTheWheel()
    {
       
        if (spinningSequence)
        {
            timer += Time.deltaTime;

            if (timer >= delayBetweenReels && currentIndex < listReels.Count)
            {
               
                listReels[currentIndex].spinDuration = Random.Range(2f, 5f);
                listReels[currentIndex].speedRoller = speedRoller;
                listReels[currentIndex].SpinningState();

                currentIndex++;
                timer = 0f;
            }

            if (currentIndex >= listReels.Count)
            {
                spinningSequence = false;

                //generamos duración global antes de detener
                timerToStop = Random.Range(2f, 4f);
                durationSequence = true; //ahora entra en fase de duración

                currentIndex = 0;
                timer = 0f;
            }
        }
    }

    private void StopTheWheel()
    {
        if (stoppingSequence)
        {
            timer += Time.deltaTime;

            if (timer >= delayBetweenReels && currentIndexToStop < listReels.Count)
            {
                listReels[currentIndexToStop].StoppingState();

                currentIndexToStop++;
                timer = 0f;
            }

            if (currentIndexToStop >= listReels.Count)
            {
                stoppingSequence = false; //fin de la fase
                currentIndexToStop = 0;
                timer = 0f;
                state = SlotState.Evaluating;
                enableButton.OnSpinFinish();
            }
        }
    }

    public void ButtonSpin()
    {
        speedRoller = Random.Range(350, 500);
        spinningSequence = true;
        stoppingSequence = false;
        durationSequence = false;

        currentIndex = 0;
        currentIndexToStop = 0;

        timer = 0f;
        timer2 = 0f;
        state = SlotState.Spinning;
    }

    private void SwitchSlot()
    {
        switch (state)
        {
            case SlotState.Starting:
                foreach(var reel in listReels)
                {
                    reel.StartState();
                }
                state = SlotState.Idle;
                break;
            case SlotState.Idle:
                break;

            case SlotState.Spinning:

                SpinTheWheel();

                break;

            case SlotState.Stopping:
                StopTheWheel();
                break;

            case SlotState.Evaluating:
                // Aquí iría la lógica para evaluar patrones
                break;
        }
    }
}