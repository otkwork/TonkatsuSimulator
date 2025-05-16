using UnityEngine;

public class Fryer : MonoBehaviour
{
    [SerializeField] FryerNet[] fryNet = new FryerNet[2];   // 左右のフライヤー 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PutButton(int fryTime, int leftRight)
    {
        // 指定の網をfryTime分油に入れる
        fryNet[leftRight].SetTimer(fryTime);
    }
}
