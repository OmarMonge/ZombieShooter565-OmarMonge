/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacter : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject[] characterPrefabs;
    private int currentPrefabIndex = 0;
    private GameObject currentCharacter;
    private void OnEnable()
    {
        instantiateChar();


    }

    public void instantiateChar() {
        //var go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        currentCharacter = Instantiate(characterPrefabs[currentPrefabIndex], Vector3.zero, Quaternion.identity);
        currentCharacter.tag = "MainCharacter";
        currentCharacter.transform.position = new Vector3(742, 4.9f, 636);
        currentCharacter.transform.rotation = Quaternion.Euler(-90, 90, 90);
    }
    public void CycleNextCharacter()
    {
        // Increment the index to cycle to the next character prefab
        var mainCharacters = GameObject.FindGameObjectsWithTag("MainCharacter");

        // Destroy each GameObject in the array
        foreach (GameObject mainChar in mainCharacters)
        {
            Destroy(mainChar);
        }

        currentPrefabIndex = (currentPrefabIndex + 1) % characterPrefabs.Length;
        instantiateChar();
    }

}

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacter : MonoBehaviour
{
    //public GameObject CustomChar;
    public GameObject[] characterPrefabs;
    private int currentPrefabIndex = 0;
    private GameObject currentCharacter;
    public GameObject CustomScreen;
    public GameObject City;
    public Material Sky;
    public Camera mainCamera;


    private void OnEnable()
    {
        //InstantiateChar();
    }
    public void ButtonClicked(int id)
    {
        //var post = mainCamera.GetComponent<PSXPostProcessEffect>();
        //var customChar = CustomChar.transform.GetComponent<CustomCharacter>();
        switch (id)
        {
            case 0:
                characterPrefabs[0].SetActive(false);
                Debug.Log("body");
                CycleNextCharacter();
                break;
            case 1:
                Debug.Log("head");
                CycleNextCharacter();
                //customChar.CycleNextCharacter();
                break;
            case 2:
                enableScene();
                break;
        }
    }
    public void enableScene() { 
        CustomScreen.SetActive(false);
        City.SetActive(true);
        ChangeToSkybox();
    }

    public void ChangeToSkybox()
    {
        if (mainCamera != null)
        {
            mainCamera.clearFlags = CameraClearFlags.Skybox;
            mainCamera.GetComponent<Skybox>().material = Sky;
        }
    }

    public void InstantiateChar()
    {
        if (currentCharacter != null)
        {
            currentCharacter.SetActive(false);
        }

        currentCharacter = characterPrefabs[currentPrefabIndex];
        currentCharacter.SetActive(true);
//        currentCharacter.tag = "MainCharacter";

    }

    public void CycleNextCharacter()
    {
        if (currentCharacter != null)
        {
            currentCharacter.SetActive(false);
        }

        currentPrefabIndex = (currentPrefabIndex + 1) % characterPrefabs.Length;

        InstantiateChar();
    }
}
