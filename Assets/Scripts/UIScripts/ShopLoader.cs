using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopLoader : MonoBehaviour
{
    public void LoadShop()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ShopScene");
    }
}
