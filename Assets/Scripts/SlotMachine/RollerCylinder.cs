using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the possible states of a slot roller.
/// </summary>
public enum SlotState { Idle, Starting, Spinning, Stopping, FillingList ,Evaluating, ShowingWin }
/// <summary>
/// Controls the behavior and mechanics of a roller cylinder in the slot machine.
/// </summary>
public class RollerCylinder : MonoBehaviour
{
    [Header("Roller Sprites")]
    [Tooltip("List of symbols available to fill the roller.")]
    public List<Symbols> symbols = new List<Symbols>();

    [Tooltip("List of currently active symbols on the roller.")]
    public List<Symbols> symbolsActives = new List<Symbols>();

    [Tooltip("List of currently active GameObjects on the roller.")]
    public List<GameObject> GOActives = new List<GameObject>();

    [Tooltip("Array of symbol IDs used to fill the roller.")]
    public int[] idSymbols;

    [Tooltip("Sprites available to represent symbols.")]
    public Sprite[] sprites;

    [Tooltip("Reference to the unique symbol list.")]
    public UniqueList uniqueList;

    [Header("Roller Parameters")]
    [Tooltip("Radius of the roller cylinder.")]
    public float radius = 3f;

    [Tooltip("Prefab of the sprite to be instantiated on the roller.")]
    public GameObject spritePrefab;

    [Tooltip("Initial angle offset of the roller.")]
    public float angleOffset = 0f;

    [Tooltip("Rotation speed of the roller.")]
    public float speedRoller = 100f;

    private int spriteCount;
    private float angleStep;

    [Tooltip("Duration of the roller spin in seconds.")]
    public float spinDuration = 0f;

    [Tooltip("Indicates if the roller is currently spinning.")]
    public bool startSpinnig = false;


    /// <summary>
    /// Fills the roller symbol list based on defined symbol IDs.
    /// </summary>
    void GetSymbolsToFill()
    {
        try
        {
            foreach (var i in idSymbols)
            {
                symbols.Add(uniqueList.symbols[i - 1]);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in GetSymbolsToFill: {ex.Message}");
        }
    }

    /// <summary>
    /// Generates the visual cylinder with the configured symbols.
    /// </summary>
    void GenerateCylinder()
    {
        try
        {
            if (symbols == null || symbols.Count == 0) return;

            spriteCount = symbols.Count;
            angleStep = 360f / spriteCount;
            int iter = 0;

            for (int i = 0; i < spriteCount; i++)
            {
                iter++;
                if (iter == spriteCount)
                    iter = 0;

                float angle = (-i * angleStep + angleOffset) * Mathf.Deg2Rad;

                Vector3 pos = new Vector3(
                    0f,
                    Mathf.Sin(angle) * radius,
                    Mathf.Cos(angle) * radius
                );

                GameObject go = Instantiate(spritePrefab, transform);
                go.transform.localPosition = pos;
                go.name = symbols[iter].name;

                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.sprite = symbols[iter].Sprite;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in GenerateCylinder: {ex.Message}");
        }
    }
    /// <summary>
    /// Rotates the roller along the X-axis.
    /// </summary>
    public void Spin(float speed)
    {
        try
        {
            transform.Rotate(Vector3.right * speed * Time.deltaTime);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in Spin: {ex.Message}");
        }
    }

    void Update()
    {
        try
        {
            foreach (Transform child in transform)
            {
                child.rotation = Quaternion.identity;
            }

            UpdateVisibleSprites();

            if (startSpinnig)
            {
                Spin(speedRoller);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in Update: {ex.Message}");
        }
    }


    /// <summary>
    /// Updates which sprites are visible in the main camera.
    /// </summary>
    private void UpdateVisibleSprites()
    {
        try
        {
            Camera cam = Camera.main;
            if (cam == null) return;

            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                children[i] = transform.GetChild(i);

            System.Array.Sort(children, (a, b) =>
            {
                float az = cam.WorldToViewportPoint(a.position).z;
                float bz = cam.WorldToViewportPoint(b.position).z;
                return bz.CompareTo(az);
            });

            foreach (Transform child in transform)
                child.gameObject.SetActive(false);

            for (int i = 0; i < Mathf.Min(3, children.Length); i++)
                children[i].gameObject.SetActive(true);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in UpdateVisibleSprites: {ex.Message}");
        }
    }


    /// <summary>
    /// Initializes the roller by filling and generating symbols.
    /// </summary>
    public void StartState()
    {
        try
        {
            GetSymbolsToFill();
            GenerateCylinder();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in StartState: {ex.Message}");
        }
    }
  

    /// <summary>
    /// Starts spinning the roller.
    /// </summary>
    public void SpinningState()
    {
        try
        {
            TurnOffChilds();
            startSpinnig = true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in SpinningState: {ex.Message}");
        }
    }

    /// <summary>
    /// Stops the roller at the nearest aligned symbol.
    /// </summary>
    public void StoppingState()
    {
        try
        {
            StopAtNearestSprite();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in StoppingState: {ex.Message}");
        }
    }

    /// <summary>
    /// Aligns the roller to the nearest symbol on stop.
    /// </summary>
    private void StopAtNearestSprite()
    {
        try
        {
            startSpinnig = false;
            float currentX = transform.eulerAngles.x;
            float targetX = Mathf.Round(currentX / angleStep) * angleStep;
            transform.rotation = Quaternion.Euler(targetX, 0f, 0f);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in StopAtNearestSprite: {ex.Message}");
        }
    }

    /// <summary>
    /// Returns a list of active GameObjects currently visible on the roller.
    /// </summary>
    public List<GameObject> GOActivein()
    {
        try
        {
            GOActives.Clear();
            Camera cam = Camera.main;
            if (cam == null) return GOActives;

            Transform[] activeChildren = new Transform[transform.childCount];
            int count = 0;
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeInHierarchy)
                    activeChildren[count++] = child;
            }
            System.Array.Resize(ref activeChildren, count);

            System.Array.Sort(activeChildren, (a, b) =>
            {
                Vector3 aView = cam.WorldToViewportPoint(a.position);
                Vector3 bView = cam.WorldToViewportPoint(b.position);
                return bView.y.CompareTo(aView.y);
            });

            GOActives.AddRange(System.Array.ConvertAll(activeChildren, t => t.gameObject));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in GOActivein: {ex.Message}");
        }
        return GOActives;
    }

    /// <summary>
    /// Returns a list of active symbols currently visible on the roller.
    /// </summary>
    public List<Symbols> SymbolsActives()
    {
        try
        {
            symbolsActives.Clear();
            Camera cam = Camera.main;
            if (cam == null) return symbolsActives;

            Transform[] activeChildren = new Transform[transform.childCount];
            int count = 0;
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeInHierarchy)
                    activeChildren[count++] = child;
            }
            System.Array.Resize(ref activeChildren, count);

            System.Array.Sort(activeChildren, (a, b) =>
            {
                Vector3 aView = cam.WorldToViewportPoint(a.position);
                Vector3 bView = cam.WorldToViewportPoint(b.position);
                return bView.y.CompareTo(aView.y);
            });

            foreach (var child in activeChildren)
            {
                Symbols symbol = symbols.Find(s => s.name == child.name);
                if (symbol != null)
                    symbolsActives.Add(symbol);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in SymbolsActives: {ex.Message}");
        }
        return symbolsActives;
    }

    /// <summary>
    /// Turns off child objects inside the roller.
    /// </summary>
    public void TurnOffChilds()
    {
        try
        {
            foreach (Transform child in transform)
            {
                child.GetChild(0).gameObject.SetActive(false);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in TurnOffChilds: {ex.Message}");
        }
    }
}
