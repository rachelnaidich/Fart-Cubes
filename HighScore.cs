using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {

	public static int StoreHighscore(int newHighscore)
	{
		int oldHighscore = PlayerPrefs.GetInt("highscore", 0);    
		if(newHighscore > oldHighscore){
			PlayerPrefs.SetInt("highscore", newHighscore);
			return newHighscore;
			PlayerPrefs.Save();
			}
		else{
			return oldHighscore;
		}
	}
}
