using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SlotState { Idle, Starting, Spinning, Stopping, Evaluating, ShowingWin }
public class RollerCylinder : MonoBehaviour
{
    [Header("Sprites del rodillo")]
    public List<Symbols> symbols = new List<Symbols>();
    public List<Symbols> symbolsActives = new List<Symbols>();
    public int[] idSymbols;
    public Sprite[] sprites;
    public UniqueList uniqueList;

    [Header("Parámetros del rodillo")]
    public float radius = 3f;
    public GameObject spritePrefab;
    public float angleOffset = 0f;
    public float speedRoller = 100f; // velocidad de rotación

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

        // Activar solo los 3 sprites más cercanos a la cámara
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

        // Ordena los hijos según la distancia al frente de la cámara
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            children[i] = transform.GetChild(i);

        // Compara la posición en Z en cámara
        System.Array.Sort(children, (a, b) =>
        {
            float az = cam.WorldToViewportPoint(a.position).z;
            float bz = cam.WorldToViewportPoint(b.position).z;
            return bz.CompareTo(az); // de mayor a menor
        });

        // Desactiva todos primero
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        // Activa solo los 3 más cercanos al frente de la cámara
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
        GetSymbolsToFill();
        GenerateCylinder();
    }
    public void SpinningState()
    {
       startSpinnig = true;
    }
    public void StoppingState()
    {
        StopAtNearestSprite();
        Debug.Log("Rodillo detenido y alineado");
    }
    // Alinea el rodillo al sprite más cercano
    private void StopAtNearestSprite()
    {
        startSpinnig = false;
        float currentX = transform.eulerAngles.x;
        float targetX = Mathf.Round(currentX / angleStep) * angleStep;
        transform.rotation = Quaternion.Euler(targetX, 0f, 0f);
    }
}
