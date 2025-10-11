using UnityEngine;
using UnityEngine.SceneManagement;
public class UnCachedScene : MonoBehaviour
{
	[Header("The stuff")]
	[Tooltip("This is useful to prevent the scene to load from being cached in ram, ¡yummy!")]
	public string SceneName;
	void Start()
	{
		SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
	}
}