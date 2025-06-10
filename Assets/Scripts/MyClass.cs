using UnityEngine;

public class MyClass : MonoBehaviour
{
    public int Health = 100;
    public float Speed = 1.5f;
    public string Name = "서범석";
    public bool IsAlive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("게임이 시작되었습니다!");
        Debug.Log($"int Health : {Health}, float Speed : {Speed}, string Name : {Name}, bool IsAlive : {IsAlive}");

        HealthCheck();
        IsGameOver();
        //Repeat5();
        Repeat10();
        PrintAstricks();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("게임이 실행 중입니다.");

        if (Input.GetKey(KeyCode.W))
        {
            //transform.Translate(0, 0, 5 * Time.deltaTime);
            transform.position += new Vector3(0, 0, 5 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //transform.Translate(0, 0, -5 * Time.deltaTime);
            transform.position += new Vector3(0, 0, -5 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //transform.Translate(0, 0, 5 * Time.deltaTime);
            transform.position += new Vector3(-5 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //transform.Translate(0, 0, -5 * Time.deltaTime);
            transform.position += new Vector3(5 * Time.deltaTime, 0, 0);
        }


        if (Input.GetKey(KeyCode.Space))
        {
            //transform.Translate(0, 0, -5 * Time.deltaTime);
            
            transform.position += new Vector3(0, 5 * Time.deltaTime, 0);
        }
    }

    void HealthCheck()
    {
        // [2025-05-15] 실습 1
        if (Health > 50)
        {
            Debug.Log("플레이어가 건강합니다.");
        }
        else if (Health > 0)
        {
            Debug.Log("플레이어가 위험한 상태입니다.");
        }
        else
        {
            Debug.Log("플레이어가 사망했습니다.");
        }
    }

    void IsGameOver()
    {
        // [2025-05-15] 실습 2
        if (Health == 0)
        {
            Debug.Log("Game Over");
        }
    }

    void Repeat5()
    {
        // [2025-05-15] 실습 3
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("반복 횟수 : " + i);
        }
    }

    void Repeat10()
    {
        // [2025-05-15] 실습 4
        for (int i = 1; i <= 10; i++)
        {
            Debug.Log("반복 횟수 : " + i);
        }
    }

    void PrintAstricks()
    {
        // [2025-05-15] 실습 5
        string line = string.Empty;

        for (int i = 0; i < 5; i++)
        {
            line = string.Empty;
            for (int j = 0; j < i + 1; j++)
            {
                line += "*";
            }

            Debug.Log(line);
        }

        //Debug.Log("*");
        //Debug.Log("**");
        //Debug.Log("***");
        //Debug.Log("****");
        //Debug.Log("*****");
    }
}
