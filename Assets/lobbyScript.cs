using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class lobbyScript : MonoBehaviour {
    public InputField[] NameFields;
    public Image[] ColorPanels;
    public Canvas LobbyCanvas;
    public InputField numOfPlayers;
    public InputField WinScore;

    private Color[] PlayerColors;
    private int[] currentColors;
    
	private List<string> AllMaps;
	public InputField mapInput;
	private int CurrMap = 0;
	// Use this for initialization
	void Start () {
        PlayerColors = new Color[4];
        PlayerColors[0] = new Color32(255, 238, 13, 255);
        PlayerColors[1] = new Color32(232, 94, 12, 255);
        PlayerColors[2] = new Color32(234, 0, 255, 255);
        PlayerColors[3] = new Color32(12, 99, 232, 255);
        //PlayerColors[4] = new Color32(0, 255, 69, 255);

        currentColors = new int[NameFields.Length];
        currentColors[0] = 0;
        currentColors[1] = 1;
        currentColors[2] = 2;
        currentColors[3] = 3;
        for(int i = 0; i < ColorPanels.Length; i++)
        {
            ColorPanels[i].color = PlayerColors[i];
        }
		AllMaps = new List<string> ();
		foreach (string arenaName in ArenaDataLoader.arenas.Keys) 
		{
			AllMaps.Add(arenaName);
		}
		if(AllMaps.Count>0)
			mapInput.text = AllMaps [CurrMap];
    }
	public string GetSelectedMap(){
		return AllMaps [CurrMap];
	}

	public void NextMap(int delta){
		CurrMap += delta;
		if (CurrMap > AllMaps.Count - 1) {
			CurrMap = 0;
		} else if (CurrMap < 0) {
			CurrMap = AllMaps.Count-1;
		}
		if (AllMaps.Count > 0) {
			mapInput.text = AllMaps [CurrMap];
		}
	}

    public void changeColorNext(int ind)
    {
        currentColors[ind] = (currentColors[ind] + 1) % PlayerColors.Length;
        ColorPanels[ind].color = PlayerColors[currentColors[ind]];
    }

    public void changeColorPrev(int ind)
    {
        currentColors[ind] = (currentColors[ind] - 1);
        if (currentColors[ind] == (-1))
        {
            currentColors[ind] = PlayerColors.Length - 1;
        }
        ColorPanels[ind].color = PlayerColors[currentColors[ind]];
    }
    
    public int[] getPlayerColorInd()
    {
        return currentColors;
    }

    public string[] getNames()
    {
        string[] names = new string[NameFields.Length];
        for (int i=0; i < NameFields.Length; i++)
        {
           names[i] = NameFields[i].text;
        }

        return names;
    }

	public int[] getTeams(){
		Dictionary<int, int> teams = new Dictionary<int, int> ();
		int[] inds = getPlayerColorInd ();
		int count = 0;
		int playercount = getNumOfPlayers ();
		for (int i = 0; i < playercount; i++) {
			if (!teams.ContainsKey (inds [i])) {
				teams.Add (inds [i], count);
				count++;
			}
		}
		int[] ret = new int[playercount];
		for (int i = 0; i < playercount; i++) {
			ret [i] = teams [inds[i]];
		}
		return ret;
	}

    public void hideLobby()
    {
        LobbyCanvas.enabled = false;
    }

    public void showLobby()
    {
        LobbyCanvas.enabled = true;
    }

    public void clipPlayers()
    {
        int num = int.Parse(numOfPlayers.text);
        if (2 > num)
        {
            numOfPlayers.text = "2";
        }
        else if (4 < num)
        {
            numOfPlayers.text = "4";
        }
    }

    public int getNumOfPlayers()
    {
        int num = 4;
        if (numOfPlayers.text != "")
        {
            num = int.Parse(numOfPlayers.text); 
        }
        return num;
    }

    public int getWinScore()
    {
        int score = 10;
        if (WinScore.text != "")
        {
			score = int.Parse(WinScore.text);
            if (score < 1)
            {
                score = 1;
            }
        }
        return score;
    }
}
