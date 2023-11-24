using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class playerCtrl : MonoBehaviour
{
    GameObject Player;
    public static int HP = 5;
    public int Max_HP = 5;
    private float speed = 5.0f;
    private float jump_power = 8.0f;
    private int jump_count = 2;
    public float rot_speed = 200.0f;

    public GameObject melee_enemy;
    float spawn_delay = 10.0f;
    public GameObject MeleeCollider;
    float melee_delay = 0.0f;
    float melee_coll_delay = 0.0f;
    public GameObject PlayerShuriken;
    public static int PlayerShurikenNum = 10;
    float range_delay = 0.0f;

    float die_delay = 0.0f;
    public static Transform player_hit;
    public Transform player_hittemp;
    public static Vector3 Player_pos;

    public static float score = 0.0f;
    public static int difficulty;

    private AudioSource audio_source;//효과음
    public AudioClip audio_step;
    public AudioClip audio_jump;
    public AudioClip audio_attack;
    private Rigidbody rigid;
    private GameObject model;
    private Animator PlayerAnim;
    public static Ray ScreenRay;
    public Image[] HP_image = new Image[5]; //UI
    public TextMeshProUGUI score_text;
    public TextMeshProUGUI die_text;
    public TextMeshProUGUI difficulty_text;
    public TextMeshProUGUI shuriken_num_text;
    bool ismove;
    bool die = false;

    // Start is called before the first frame update
    void Start()
    {
        player_hit = player_hittemp;
        model = transform.GetChild(0).gameObject;
        Player = this.gameObject;
        audio_source = gameObject.AddComponent<AudioSource>();
        PlayerAnim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Player_pos = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
        if(!die)//죽으면 어떠한 조작도 불가능 하도록
        {
            Rotate();
            Jump();
            Move();
            Attack();
            Difficulty();
            Spawnenemy();
        }
        UI();
        Die();
    }
    void UI()//UI
    {
        score += Time.deltaTime;
        score_text.text = "Score: " + (int)score;
        switch(difficulty)//난이도 텍스트
        {
            case 1:
                difficulty_text.text = "Eazy";
                break;
            case 2:
                difficulty_text.text = "Normal";
                break;
            case 3:
                difficulty_text.text = "Hard";
                break;
            case 4:
                difficulty_text.text = "Hell";
                break;
        }
        if(HP >= 0)//체력 UI 구현부
        {
            for(int i = 0; i < HP; i++)
            {
                HP_image[i].enabled = true;
            }
            for(int j = HP; j <Max_HP; j++)
            {
                HP_image[j].enabled = false;
            }
        }
        shuriken_num_text.text = "x" + PlayerShurikenNum;
    }
    public static void Hit()//플레이어 피격
    {
        Instantiate(player_hit, Player_pos, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
        HP--;
    }
    void Difficulty()//점수 100점마다 난이도 증가
    {
        difficulty = (int)(playerCtrl.score / 100) + 1;
        if(difficulty > 4)
            difficulty = 4;
    }
    void Rotate()//마우스 회전에 따라 플레이어 회전
    {
        float h = Input.GetAxis("Mouse X");
        Vector3 dir = new Vector3(0, h, 0);

        this.transform.eulerAngles += dir * rot_speed * Time.deltaTime;
    }
    void Move()//플레이어 이동함수
    {
        if(Input.GetKey(KeyCode.W)){
            PlayerAnim.SetBool("IsRunningForward", true);//앞으로 뒤는 애니메이션
            model.transform.localEulerAngles = new Vector3(0, 0, 0);
            this.transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * (speed * Time.deltaTime));
            ismove = true;
        }
        if(Input.GetKey(KeyCode.S)){
            PlayerAnim.SetBool("IsRunningBack", true);//뒤으로 뒤는 애니메이션
            model.transform.localEulerAngles = new Vector3(0, 0, 0);
            this.transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * (speed * Time.deltaTime));
            ismove = true;
        }

        if(Input.GetKey(KeyCode.A)){
            PlayerAnim.SetBool("IsRunningForward", true);
            model.transform.localEulerAngles = new Vector3(0, -90, 0);
            this.transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * (speed * Time.deltaTime));
            ismove = true;
        }

        if(Input.GetKey(KeyCode.D)){
            PlayerAnim.SetBool("IsRunningForward", true);
            model.transform.localEulerAngles = new Vector3(0, 90, 0);
            this.transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * (speed * Time.deltaTime));
            ismove = true;
        }
        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))//키가 떨어졌을 때 애니메이션 비활성화
        {
            PlayerAnim.SetBool("IsRunningForward", false);
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            PlayerAnim.SetBool("IsRunningBack", false);
        }
        else
        {
            ismove = false;
            audio_source.loop = ismove;
        }
    }
    void Jump()//점프
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jump_count > 0)//점프 횟수가 남아있을때 점프 가능
            {
                audio_source.clip = audio_jump;
                if(jump_count == 1)
                {
                    PlayerAnim.SetTrigger("DoubleJump");
                    rigid.AddForce(Vector3.up * jump_power, ForceMode.Impulse);
                    jump_count--;
                    audio_source.Play();
                }
                if(jump_count == 2)
                {
                    PlayerAnim.SetTrigger("Jump");
                    rigid.AddForce(Vector3.up * jump_power, ForceMode.Impulse);
                    jump_count--;
                    audio_source.Play();
                }
            }
        }
    }
    void Attack()
    {
        Melee();
        Range();
    }
    void Melee()//근거리 공격 함수
    {
        melee_delay += Time.deltaTime;          //근접공격 속도
        melee_coll_delay += Time.deltaTime;
        if(Input.GetMouseButtonDown(0) && melee_delay > 1.0f)
        {
            PlayerAnim.SetTrigger("Melee");     //근접 공격 애니메이션
            audio_source.clip = audio_attack;   //공격 사운드
            audio_source.Play();
            melee_delay = 0.0f;
        }

        if(melee_coll_delay < 0.7f)
        {
            MeleeCollider.SetActive(true);
        }
        else
            MeleeCollider.SetActive(false);
    }
    void Range() //원거리 공격 함수
    {
        range_delay += Time.deltaTime;          //원거리공격 속도
        if(Input.GetMouseButtonDown(1) && PlayerShurikenNum > 0 && range_delay > 1.5f)
        {
            PlayerAnim.SetTrigger("Throw");     //원거리 공격 애니메이션
            ScreenRay = Camera.main.ScreenPointToRay(Input.mousePosition);  //UI로 표시한 조준점과 최대한 오차가 없도록하는 코드
            Instantiate(PlayerShuriken, this.transform.position, this.transform.rotation);
            PlayerShurikenNum--;
            range_delay = 0.0f;
        }
    }
    void Spawnenemy()//근접 적 유닛 소환함수
    {
        spawn_delay += Time.deltaTime;
        if(spawn_delay > (10.0f - difficulty * 2)) //난이도에 따라 소환 빈도가 높아짐
        {
            Vector3 enemytrans;
            do//플레이어의 시야에서 소환되지 않도록 거리 조절
            {
                enemytrans = new Vector3(this.transform.position.x + Random.Range(-30.0f, 30.0f), 10.0f, this.transform.position.z + Random.Range(-30.0f, 30.0f));
            }while(Vector3.Distance(Player_pos, enemytrans) > 60.0f);
            Instantiate(melee_enemy, enemytrans, Quaternion.Euler(0.0f, 0.0f, 0.0f));
            spawn_delay = 0.0f;
        }
    }
    void Die()
    {
        if(HP <= 0)
        {
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;//캐릭터가 공죽에서 죽는 문제가 있어 죽었을때는 콜라이더 비활성화
            die = true;
            PlayerAnim.SetBool("Die", true);
            die_delay += Time.deltaTime;
            die_text.text = "You Die";
            die_text.fontSize += Time.deltaTime * 10;
            if(die_delay > 3f)
                SceneManager.LoadScene("DeadScene");
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if(ismove)
            {
                audio_source.clip = audio_step;
                audio_source.loop = ismove;
                audio_source.Play();
            }
            jump_count = 2; //땅에 닿았을 때 점프횟수 초기화
            MapCreator.onPlayerMap = other.transform;
        }
    }
}
