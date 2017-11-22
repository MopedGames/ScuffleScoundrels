using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestinctionHandler : PhotonManager<DestinctionHandler>
{
    [SerializeField, Tooltip("Team colors, neutral,1,2,3 etc.")]
    private Material[] teamColors;

    [SerializeField, Tooltip("Drag and drop the prefabs here NOT the ones from scene view hierarchy")]
    private GameObject[] startLocations; // TODO: find a solution for different maps with different positions

    [SerializeField]
    private List<GameObject> players;

    [SerializeField, Tooltip("the distance the camera starts behind the home town and when space is pushed")]
    private float camDistance = 40f;

    //[SerializeField, Tooltip("The astar pathing for scanning.")]
    //private AstarPath aPath;

    private Vector3 camPos;

    private float camY;

    private bool isPositioned;
    private bool isDistinct;

    public List<GameObject> Players
    { get { return players; } }

    public bool IsPositioned
    { get { return isPositioned; } set { isPositioned = value; } }

    public bool IsDistinct
    { get { return isDistinct; } set { isDistinct = value; } }

    /// <summary>
    /// Runs through the players and makes sure the name, number,startlocation and color is set correctly and updated for all players
    /// </summary>
    [PunRPC]
    public void DifferentiatePlayers()
    {


        //players.Clear(); // fresh start
        //players.Add(GameObject.Find("NeutralPlayer")); // add the neutral player, he has a coliur too
        //players.AddRange(GameObject.FindGameObjectsWithTag("Player")); // find the players

        //foreach (var p in Players) // run through the players
        //{
        //    Player player = p.GetComponent<Player>(); // ease of access
        //    PhotonView playerView = p.GetComponent<PhotonView>();

        //    if (p.gameObject.name != "NeutralPlayer") // for the actual players
        //    {
        //        player.Username = playerView.owner.name; // set their values
        //        player.TeamNumber = playerView.ownerId;
        //        player.TeamColor = teamColors[player.TeamNumber];

        //        if (p.GetComponentInChildren<Town>()) // if they have a town
        //        {
        //            player.HomeTown = p.GetComponentInChildren<Town>().gameObject; // make it their home

        //            for (int s = 0; s < startLocations.Length; s++) // locate them accordinly
        //            {
        //                player.HomeTown.transform.localPosition = startLocations[player.TeamNumber - 1].transform.localPosition; // TODO: Randomize starting location?
        //                player.HomeTown.transform.localRotation = startLocations[player.TeamNumber - 1].transform.localRotation;

        //                if (!isPositioned) // give their camera variables for positioning
        //                {
        //                    camPos = new Vector3(startLocations[player.TeamNumber - 1].transform.localPosition.x, Camera.main.transform.position.y, startLocations[player.TeamNumber - 1].transform.position.z);
        //                    camY = startLocations[player.TeamNumber - 1].transform.localRotation.eulerAngles.y;

        //                    CameraAtHomeTown();
        //                    aPath.Scan();
        //                    isPositioned = true; // run once.
        //                }
        //            }
        //        }
        //    }

        //    if (player.TeamNumber <= 0) // if the player is neutral player.. should never happen
        //    {
        //        player.TeamNumber = 0;
        //    }

        //    SetColor(); // change the colour,
        //}
    }
  

    /// <summary>
    /// Gives a ship
    /// </summary>
    [PunRPC]
    public void Paranting()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}