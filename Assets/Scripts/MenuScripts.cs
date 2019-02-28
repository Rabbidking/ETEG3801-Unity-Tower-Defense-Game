using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScripts : MonoBehaviour
{
	private AssetBundle myLoadedAssetBundle;
	private string[] scenePaths;
	void Start()
	{
        //REMOVED FOR 2/29/DEMO by RyanTollefson
        //myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
		///scenePaths = myLoadedAssetBundle.GetAllScenePaths();
	}
    // Start is called before the first frame update
    public void startGame()
    {
        //REMOVED FOR 2/29/DEMO by RyanTollefson
        //SceneManager.LoadScene(scenePaths[0], LoadSceneMode.Single);
        
        //ADDED FOR 2/29/DEMO by RyanTollefson
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    public void loadGame()
    {
        
    }
	
	public void loadSettingScreen()
	{
		
	}
	
	public void ExitGame()
	{
        Application.Quit();
	}
}
