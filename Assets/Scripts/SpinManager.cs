using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages the spinning sequence, timing, and evaluation of multiple roller cylinders.
/// </summary>
public class SpinManager : MonoBehaviour
{
    [Header("Reels Configuration")]
    [Tooltip("List of roller cylinders controlled by this manager.")]
    public List<RollerCylinder> listReels = new List<RollerCylinder>();

    [Tooltip("Delay time in seconds between each reel spin start.")]
    public float delayBetweenReels = 0.5f;

    [Header("Spin Settings")]
    [Tooltip("Base spinning speed of the reels.")]
    public float speedRoller;

    [Tooltip("Enable random spinning speed for each spin.")]
    public bool useRandomSpeed = false;

    [Header("UI Components")]
    [Tooltip("Reference to the spin button enabler.")]
    public EnableButton enableButton;

    [Header("Pattern Manager")]
    [Tooltip("Manages symbol patterns evaluation after the spin.")]
    public PatternManager patternManager;

    [Header("Current State")]
    [Tooltip("Current state of the slot machine.")]
    public SlotState state = SlotState.Idle;

    [Tooltip("List of symbols active after a spin.")]
    public List<Symbols> symbolsActivesSpin = new List<Symbols>();

    [Tooltip("List of GameObjects active after a spin.")]
    public List<GameObject> GOActivesSpin = new List<GameObject>();

    private int currentIndex = 0;
    private int currentIndexToStop = 0;

    private float timer = 0f;
    private float timer2 = 0f;
    private float timerToStop = 0f;

    private bool spinningSequence = false;
    private bool stoppingSequence = false;
    private bool durationSequence = false;
    private bool evaluated = false;

    /// <summary>
    /// Initializes the slot machine state at start.
    /// </summary>
    void Start()
    {
        try
        {
            state = SlotState.Starting;
            SwitchSlot();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in Start: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates slot machine logic and state machine each frame.
    /// </summary>
    void Update()
    {
        try
        {
            if (Input.GetKeyDown(KeyCode.W))
                ButtonSpin();

            SwitchSlot();

            if (durationSequence)
            {
                timer2 += Time.deltaTime;

                if (timer2 >= timerToStop)
                {
                    durationSequence = false;
                    stoppingSequence = true;
                    timer2 = 0f;
                    timer = 0f;
                    currentIndexToStop = 0;
                    state = SlotState.Stopping;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in Update: {ex.Message}");
        }
    }

    /// <summary>
    /// Spins each reel sequentially with a delay between them.
    /// </summary>
    private void SpinTheWheel()
    {
        try
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
                    timerToStop = Random.Range(2f, 4f);
                    durationSequence = true;

                    currentIndex = 0;
                    timer = 0f;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in SpinTheWheel: {ex.Message}");
        }
    }

    /// <summary>
    /// Stops each reel sequentially with a delay between them.
    /// </summary>
    private void StopTheWheel()
    {
        try
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
                    stoppingSequence = false;
                    currentIndexToStop = 0;
                    timer = 0f;
                    state = SlotState.FillingList;
                    enableButton.OnSpinFinish();
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in StopTheWheel: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles spin button press, starts the spin sequence.
    /// </summary>
    public void ButtonSpin()
    {
        try
        {
            if (useRandomSpeed)
            {
                speedRoller = Random.Range(350, 500);
            }
            spinningSequence = true;
            stoppingSequence = false;
            durationSequence = false;

            currentIndex = 0;
            currentIndexToStop = 0;
            evaluated = false;
            timer = 0f;
            timer2 = 0f;

            state = SlotState.Spinning;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in ButtonSpin: {ex.Message}");
        }
    }

    /// <summary>
    /// Controls the slot state machine and transitions between states.
    /// </summary>
    private void SwitchSlot()
    {
        try
        {
            switch (state)
            {
                case SlotState.Starting:
                    foreach (var reel in listReels)
                        reel.StartState();
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

                case SlotState.FillingList:
                    if (!evaluated)
                    {
                        GOActivesSpin.Clear();
                        symbolsActivesSpin.Clear();

                        foreach (var reel in listReels)
                        {
                            GOActivesSpin.AddRange(reel.GOActivein());
                            symbolsActivesSpin.AddRange(reel.SymbolsActives());
                        }

                        evaluated = true;
                        state = SlotState.Evaluating;
                    }
                    break;

                case SlotState.Evaluating:
                    if (patternManager != null)
                    {
                        patternManager.Roller5.Clear();
                        patternManager.ReelsActives.Clear();

                        patternManager.FillListRollers(symbolsActivesSpin);
                        patternManager.FillReels(GOActivesSpin);

                        patternManager.CheckPatterns();
                    }
                    break;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in SwitchSlot: {ex.Message}");
        }
    }
}