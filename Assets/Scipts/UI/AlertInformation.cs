using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertInformation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI alertText;
    [SerializeField]
    private float showTime = 2f;
    
    
    private Coroutine Routine;
    private Queue<string> alertString = new Queue<string>();
    //private StringBuilder builder = new StringBuilder();
    public void Alert(string s)
    {
        alertString.Enqueue(s);
        if (!gameObject.activeSelf)
        { 
            gameObject.SetActive(true);
            StartCoroutine(alertRoutine());
        }
    }

    IEnumerator alertRoutine()
    {
        string s;
        while (alertString.TryDequeue(out s))
        {
            alertText.text = s;
            yield return new WaitForSeconds(showTime);
        }
        this.gameObject.SetActive(false);
    }

}
