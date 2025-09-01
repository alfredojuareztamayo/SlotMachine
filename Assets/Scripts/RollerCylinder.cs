using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SlotState { Idle, Starting, Spinning, Stopping, FillingList ,Evaluating, ShowingWin }
public class RollerCylinder : MonoBehaviour
{
    [Header("Sprites del rodillo")]
    public List<Symbols> symbols = new List<Symbols>();
    public List<Symbols> symbolsActives = new List<Symbols>();
    public List<GameObject> GOActives = new List<GameObject>();
    public int[] idSymbols;
    public Sprite[] sprites;
    public UniqueList uniqueList;

    [Header("Par�metros del rodillo")]
    public float radius = 3f;
    public GameObject spritePrefab;
    public float angleOffset = 0f;
    public float speedRoller = 100f; // velocidad de rotaci�n

    private int spriteCount;
    

   
    public float spinDuration = 0f;
    private float angleStep;

    public bool startSpinnig = false;

    
    void GetSymbolsToFill()
    {
        foreach(var i in idSymbols)
        {
            symbols.Add(uniqueList.symbols[i-1]);
        }
    }
    void GenerateCylinder()
    {
        if (symbols == null || symbols.Count == 0) return;

        spriteCount = symbols.Count;
        angleStep = 360f / spriteCount;
        int iter = 0;
        for (int i = 0; i < spriteCount; i++)
        {
            iter++;
            if(iter == spriteCount)
                iter = 0;
            float angle = (-i * angleStep + angleOffset) * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(
                0f,
                Mathf.Sin(angle) * radius ,   // eje vertical
                Mathf.Cos(angle) * radius    // profundidad
            );

            GameObject go = Instantiate(spritePrefab, transform);
            go.transform.localPosition = pos;
            go.name = symbols[iter].name;

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sprite = symbols[iter].Sprite;
        }
       // transform.rotation = Quaternion.Euler(angleStep, 0f, 0f);
    }

    // Hace girar el rodillo
    public void Spin(float speed)
    {
        transform.Rotate(Vector3.right * speed * Time.deltaTime);
    }

    void Update()
    {
        // Mantener sprites derechitos
        foreach (Transform child in transform)
        {
            child.rotation = Quaternion.identity;
        }

        // Activar solo los 3 sprites m�s cercanos a la c�mara
        UpdateVisibleSprites();
        if (startSpinnig)
        {
            Spin(speedRoller);
        }

        //SwitchSlot();
    }

    private void UpdateVisibleSprites()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Ordena los hijos seg�n la distancia al frente de la c�mara
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            children[i] = transform.GetChild(i);

        // Compara la posici�n en Z en c�mara
        System.Array.Sort(children, (a, b) =>
        {
            float az = cam.WorldToViewportPoint(a.position).z;
            float bz = cam.WorldToViewportPoint(b.position).z;
            return bz.CompareTo(az); // de mayor a menor
        });

        // Desactiva todos primero
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        // Activa solo los 3 m�s cercanos al frente de la c�mara
        for (int i = 0; i < Mathf.Min(3, children.Length); i++)
            children[i].gameObject.SetActive(true);
    }
   

    public void StartState()
    {
        GetSymbolsToFill();
        GenerateCylinder();
       
    }
    public void IdleState()
    {
        bool activeWin = true;

        //
    }
    public void SpinningState()
    {
        TurnOffChilds();
       startSpinnig = true;
    }
    public void StoppingState()
    {
        StopAtNearestSprite();
    }
    // Alinea el rodillo al sprite m�s cercano
    private void StopAtNearestSprite()
    {
        startSpinnig = false;
        float currentX = transform.eulerAngles.x;
        float targetX = Mathf.Round(currentX / angleStep) * angleStep;
        transform.rotation = Quaternion.Euler(targetX, 0f, 0f);
    }

    public List<GameObject> GOActivein()
    {
        GOActives.Clear();

        Camera cam = Camera.main;
        if (cam == null) return GOActives;

        // Ordena los hijos activos seg�n la distancia a la c�mara
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
            // proyecta las posiciones al viewport de la c�mara
            Vector3 aView = cam.WorldToViewportPoint(a.position);
            Vector3 bView = cam.WorldToViewportPoint(b.position);

            // compara por Y en pantalla: Y m�s alto = m�s arriba visualmente
            return bView.y.CompareTo(aView.y);
        });

        GOActives.AddRange(System.Array.ConvertAll(activeChildren, t => t.gameObject));
        return GOActives;
    }

    public List<Symbols> SymbolsActives()
    {
        symbolsActives.Clear();

        Camera cam = Camera.main;
        if (cam == null) return symbolsActives;

        // Ordena los hijos activos seg�n la distancia a la c�mara
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
            return bView.y.CompareTo(aView.y); // de arriba hacia abajo
        });

        foreach (var child in activeChildren)
        {
            Symbols symbol = symbols.Find(s => s.name == child.name);
            if (symbol != null)
                symbolsActives.Add(symbol);
        }

        return symbolsActives;
    }

    public void TurnOffChilds()
    {
        foreach (Transform child in transform)
        {
           child.GetChild(0).gameObject.SetActive(false);
        }
    }
}
