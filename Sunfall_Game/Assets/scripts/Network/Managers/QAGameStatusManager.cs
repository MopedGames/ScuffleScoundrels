using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QAGameStatusManager : PhotonManager<QAGameStatusManager>
{
    [SerializeField, Tooltip("Players currently active in the scene")]
    private List<GameObject> currentPlayers;

    [SerializeField, Tooltip("The number of players needed to play the game in this scene / map")]
    private int numberOfPlayers = 2;

    [SerializeField, Tooltip("Delay before starting the end game animation")]
    private float gameOverFadeDelay = 0.2f;

    private bool gameIsStarted = false;
    private bool gameIsOver = false;

    [SerializeField, Tooltip("Remaining players after a check, who ever needs to see the end game animation")]
    private List<GameObject> remainingPlayers;

    private GameObject removedPlayer;
    private GameObject neutralPlayer;
    private string winnerUserName = "No One";
    private int winnerTeamNumber = 0;

    // is the game over?
    public bool GameIsOver
    { get { return gameIsOver; } set { gameIsOver = value; } }

    /// <summary>
    /// the players in the game.
    /// </summary>
    public List<GameObject> Players
    { get { return currentPlayers; } set { currentPlayers = value; } }

    /// <summary>
    /// the number of players allowed in the level
    /// </summary>
    public int NumberOfPlayers
    { get { return numberOfPlayers; } }

    /// <summary>
    /// the time before the player list is checked for whether or not the game should end.
    /// </summary>
    public float GameOverFadeDelay
    { get { return gameOverFadeDelay; } set { gameOverFadeDelay = value; } }

    public bool GameIsStarted
    { get { return gameIsStarted; } set { gameIsStarted = value; } }

    private void Start()
    {
        gameIsStarted = false;
    }

    private void Update()
    {
        if (PhotonNetwork.playerList.Length >= numberOfPlayers) // make sure we run it after the players are all instantiated //toDO make this dynamic with connection handler
        {
            if (gameIsStarted == false) // make sure we only run it once
            {
                GameObject[] checkPlayers = GameObject.FindGameObjectsWithTag("Player"); // temporary list of players to make sure not to start anything before everyone is ready

                if (checkPlayers.Length == numberOfPlayers) // is everyone here?
                {
                    photonView.RPC("GameStarted", PhotonTargets.AllBuffered); // lets start it UUUP!
                }
            }
        }
    }

    /// <summary>
    /// Run at the start of the game
    /// </summary>
    [PunRPC]
    public void GameStarted()
    {
        if (remainingPlayers.Count >= 1)
        {
            remainingPlayers.Clear(); // clear the lists to start fresh
        }
        currentPlayers.Clear();
        currentPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player")); // find the players

        if (currentPlayers.Count == numberOfPlayers)
        {
            foreach (var p in currentPlayers) // run though them
            {
                Player player = p.GetComponent<Player>(); // for ease of use.

                //if (!p.GetComponentInChildren<Town>())
                //{
                //    player.CreateTown(); // create our home town. // give the player a place to live
                //}
            }
            gameIsStarted = true; // now that everything is ready the game is started.
        }
        if (currentPlayers.Count == numberOfPlayers /*&& DestinctionHandler.Instance.IsPositioned && gameIsStarted*/)
        {
            StartCoroutine(Delay(.2f)); // start delay which checks the players.
        }
    }
    /// <summary>
    /// Game is over, who won? start the animation.
    /// </summary>
    /// <param name="winnerName">name of the winning player</param>
    /// <param name="winnerNumber"> winning players number</param>
    //[PunRPC]
    //public void GameOver(string winnerName, int winnerNumber)
    //{
    //    remainingPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player")); // find all players. ( even people who died)

    //    foreach (var p in remainingPlayers)
    //    {
    //        if (p.GetComponent<PhotonView>().isMine)
    //        {
    //            p.GetComponent<Player>().GameOverPnl.SetActive(true); // gameoverpnl is activated

    //            p.GetComponentInChildren<GameOver>().gameObject.GetComponent<Animator>().SetBool("GameOver", true); // set the bool to start the animation
    //            if (winnerNumber != 0) // if the winner is a player
    //            {
    //                p.GetComponentInChildren<GameOver>().GameOverTxt.text = winnerName + "(Player " + winnerNumber + ")" + " Wins the Game"; // txt for the end screen
    //            }
    //            else
    //            {
    //                p.GetComponentInChildren<GameOver>().GameOverTxt.text = "Computer wins!";
    //            }
    //        }
    //    }

    //    GameIsOver = true; // game is over.
    //}

    /// <summary>
    /// check if the players are present and accounted for and they have a place to live.
    /// </summary>
    [PunRPC]
    public void CheckPlayers()
    {
        //foreach (var p in currentPlayers) // run through the active players ( after the removal process)
        //{
        //if (!p.GetComponentInChildren<Town>()) // if the player doesnt has a hometown
        //{
        //    CleanupPlayerList(p.GetComponent<PhotonView>().ownerId);
        //    break;
        //}
        //    if (currentPlayers.Count == 1 && numberOfPlayers != 1) // only one guy left;
        //    {
        //        if (p.GetComponentInChildren<Town>()) // if the player has a town
        //        {
        //            winnerUserName = p.GetComponent<Player>().Username; // set him as winner
        //            winnerTeamNumber = p.GetComponent<Player>().TeamNumber;
        //        }
        //        if (PhotonNetwork.isMasterClient)
        //        {
        //            photonView.RPC("GameOver", PhotonTargets.AllBuffered, winnerUserName, winnerTeamNumber);// game over yo.
        //        }

        //        break;
        //    }
        //}
        //if (currentPlayers.Count == 0 && numberOfPlayers == 1 || currentPlayers.Count == 0 && SceneManagerHelper.ActiveSceneName == ConnectionHandler.CHInstance.L) // levelforOne testing case
        //{
        //    winnerUserName = "Computer"; // set him as winner
        //    winnerTeamNumber = 0;
        //    if (PhotonNetwork.isMasterClient) // game over yo.
        //    {
        //        photonView.RPC("GameOver", PhotonTargets.AllBuffered, winnerUserName, winnerTeamNumber);
        //    }
        //}
    }

    /// <summary>
    /// reset the level to its natural neutral state ( remove buffs, reset positions etc.
    /// </summary>
    //public void ResetLevel()
    //{
    //    TurncoatBuilding[] neutralBuildingsToReturn = removedPlayer.GetComponentsInChildren<TurncoatBuilding>(); // make an array of the players we need to give back from the removed player (player who lost)
    //    Buildings b = neutralPlayer.GetComponentInChildren<Buildings>(); // where the buildings are going

    //    foreach (var nB in neutralBuildingsToReturn)
    //    {
    //        nB.GetComponent<PhotonView>().TransferOwnership(neutralPlayer.GetComponent<PhotonView>().ownerId); // change owner to the scene
    //        nB.transform.parent = neutralPlayer.GetComponentInChildren<Buildings>().transform; // change it in the heirachy
    //        DestinctionHandler.Instance.SetColor(); // change the colour to neutral
    //        nB.UnderControl = false; // make sure it is no longer under player control
    //    }
    //}
    /// <summary>
    /// delay the player check, adds time between player losing his town and running the end game animation
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    public IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("CheckPlayers", PhotonTargets.All);
        }
    }
    /// <summary>
    /// cleanup the list of current players, either after they leave or they die.
    /// </summary>
    /// <param name="id">id of the player who needs to be removed</param>
    [PunRPC]
    public void CleanupPlayerList(int id)
    {
        foreach (var p in currentPlayers)
        {
            if (p.GetComponent<PhotonView>().ownerId == id)
            {
                removedPlayer = p;
                currentPlayers.Remove(p); // remove him from the active players
                //ResetLevel(); // reset the level - i.e. give back their buildings etc.
                break;
            }
        }
        photonView.RPC("CheckPlayers", PhotonTargets.All); // rerun checkplayers. to see if twe need to end the game.
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}