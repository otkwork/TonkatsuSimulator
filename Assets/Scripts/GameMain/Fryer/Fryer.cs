using UnityEngine;

public class Fryer : MonoBehaviour
{
    [SerializeField] FryerNet[] fryNet = new FryerNet[2];   // ���E�̃t���C���[ 
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
        // �w��̖Ԃ�fryTime�����ɓ����
        fryNet[leftRight].SetTimer(fryTime);
    }
}
