using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class UIMenuNaviguation : MonoBehaviour
{
    private InputSystemUIInputModule inputSystem;
    private PlayerInputActions inputActions;
    private bool m_IsLevel2Locked = true;
    public bool IsLevel2Locked { get; set; }

    private bool m_IsLevel3Locked = true;
    public bool IsLevel3Locked { get; set; }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level_01");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level_02");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level_03");
    }

    public void GoBackButton()
    {
        SceneManager.LoadScene("MainGameScene");
    }
}