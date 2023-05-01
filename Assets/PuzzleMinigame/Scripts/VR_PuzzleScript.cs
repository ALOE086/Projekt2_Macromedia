using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;

public class VR_PuzzleScript : MonoBehaviour
{
    [SerializeField] private Transform emptySpace = null;
    [SerializeField] private TileScript[] tiles;
    private int emptySpaceIndex = 8;

    [SerializeField] InputActionProperty rightControllerActivate;
    [SerializeField] XRBaseController rightHand;

    private bool allowMove = true;

    RaycastHit hit;


    void Start()
    {
        Shuffle();
    }


    private void FixedUpdate()
    {
        if (rightControllerActivate.action.IsPressed())
        {
            
            if (allowMove == true)
            {
                StartCoroutine(MoveTiles());
            }
                

        }
    }

    void Update()
    {
        int correctTiles = 0;
        foreach (var a in tiles)
        {
            if (a != null)
            {
                if (a.inRightPlace)
                {
                    correctTiles++;
                }
            }

        }
        if (correctTiles == tiles.Length - 1)
        {
            allowMove = false;

        }


    }


   

    IEnumerator MoveTiles()
    {
        allowMove = false;

        if (Physics.Raycast(rightHand.transform.position, rightHand.transform.forward, out hit, 1000f))
        {
            
            if (Vector2.Distance(emptySpace.position, hit.transform.position) < 4)
            {
                Vector2 lastEmptySpacePostion = emptySpace.position;
                TileScript thisTile = hit.transform.GetComponent<TileScript>();
                emptySpace.position = thisTile.targetPosition;
                thisTile.targetPosition = lastEmptySpacePostion;
                int tileIndex = findIndex(thisTile);
                tiles[emptySpaceIndex] = tiles[tileIndex];
                tiles[tileIndex] = null;
                emptySpaceIndex = tileIndex;
                
            }
            
        }
        yield return new WaitForSeconds(0.3f);
        allowMove = true;
    }
    

    public void Shuffle()
    {
        if (emptySpaceIndex != 8)
        {
            var tileOn8LastPos = tiles[8].targetPosition;
            tiles[8].targetPosition = emptySpace.position;
            emptySpace.position = tileOn8LastPos;
            tiles[emptySpaceIndex] = tiles[8];
            tiles[8] = null;
            emptySpaceIndex = 8;
        }
        int invertion;
        do
        {
            for (int i = 0; i <= 7; i++)
            {
                if (tiles[i] != null)
                {
                    var lastPos = tiles[i].targetPosition;
                    int randomIndex = UnityEngine.Random.Range(0, 7);
                    tiles[i].targetPosition = tiles[randomIndex].targetPosition;
                    tiles[randomIndex].targetPosition = lastPos;
                    var tile = tiles[i];
                    tiles[i] = tiles[randomIndex];
                    tiles[randomIndex] = tile;

                }
            }
            invertion = GetInversions();
            Debug.Log("Puzzle shuffled");
        } while (invertion % 2 != 0);

    }

    public int findIndex(TileScript ts)
    {

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                if (tiles[i] == ts)
                {
                    return i;
                }
            }
        }

        return -1;
    }


    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            int thisTileInvertion = 0;
            for (int j = i; j < tiles.Length; j++)
            {
                if (tiles[j] != null)
                {
                    if (tiles[i].number > tiles[j].number)
                    {
                        thisTileInvertion++;
                    }
                }
            }
            inversionsSum += thisTileInvertion;
        }
        return inversionsSum;
    }
}
