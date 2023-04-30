using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{

    public Vector3 targetPosition;
    private Vector3 correctPosition;
    private SpriteRenderer spriteRenderer;


    public int number;
    public bool inRightPlace;


    void Awake()
    {
        targetPosition = transform.position;
        correctPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        if (targetPosition == correctPosition)
        {
            spriteRenderer.color = new Color(0.5f, 1f, 0.5f);
            inRightPlace = true;
        }
        else
        {
            spriteRenderer.color = Color.white;
            inRightPlace = false;
        }
    }
}
