using System.Collections;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject _popUpUI;
    public int waitTime;

    void Start()
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(waitTime);
        _popUpUI.SetActive(false);
    }
}
