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

public class PuzzleGameScript : MonoBehaviour
{
    [SerializeField] private Transform emptySpace = null;
    [SerializeField] private Camera cam;
    [SerializeField] private TileScript[] tiles;
    private int emptySpaceIndex = 8;

    //[SerializeField] InputActionReference triggeredAction = null;
    //[SerializeField] XRRayInteractor interactor;

    ////public XRBaseController rightController;
    ////public LayerMask layerMask;
    ////public float raycastDistance = 100f;

    //private RaycastHit2D hit;



    

    

    void Start()
    {
        //cam = Camera.main;

        Shuffle();

    }



    



    void Update()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit)
            {
                Debug.Log(hit.point);
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
        }



        //if (controller)
        //{
        //    if (controller.enableInputActions)
        //    {
        //        // Check if the activate button is pressed down
        //        InputHelpers.IsPressed(controller.inputDevice, activateButton, out isActivated);

        //        // If the activate button is pressed down, shoot a raycast and check for a hit
        //        if (triggeredAction)
        //        {
        //            RaycastHit hit;
        //            if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, Mathf.Infinity, layerMask))
        //            {
        //                Debug.Log("Hit point: " + hit.point);
        //            }
        //        }
        //    }
        //}


        //if (triggeredAction.action.IsPressed())
        //{

        //    Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
        //    hit = Physics2D.Raycast(ray.origin, ray.direction);

        //    if (hit)
        //    {
        //                                                                                                                           Debug.Log(hit.transform.position);
        //        if (Vector2.Distance(emptySpace.position, hit.transform.position) < 4)
        //        {
        //            MoveTile();
        //        }
        //    }
        //}
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
            Debug.Log("WOOONNN!!!!");
        }

        
    }

    //public void MoveTile()
    //{                                                                                                                                   Debug.Log("Activated");
        
    //}

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
        } while (invertion%2 != 0);
        
    }

    public int findIndex(TileScript ts)
    {
        
        for (int i = 0; i < tiles.Length;i++)
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
