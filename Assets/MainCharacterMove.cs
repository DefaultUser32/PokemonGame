using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainCharacterMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float activateDifference;
    [SerializeField] MusicManager musicManager;
    [SerializeField] AudioClip ding;
    [SerializeField] AudioClip step;
    [SerializeField] Transform green;
    [SerializeField] Animator doorAnim;
    [SerializeField] Collider2D wall;
    [SerializeField] Image textBox;
    [SerializeField] Text text;
    [SerializeField] SpriteRenderer fadeObject;
    [SerializeField] Material fadeMaterial;

    [SerializeField] RectTransform blueBar;
    [SerializeField] RectTransform orangeBar;
    [SerializeField] RectTransform blueBarWhite;
    [SerializeField] RectTransform orangeBarWhite;
    [SerializeField] RectTransform playerSprite;
    [SerializeField] RectTransform rivalSprite;

    readonly List<string> lines = new List<string>() {"hey, red!", "I was looking forward to seeing you, red." , "my activePokemonRival should be strong to keep me sharp.", "while working on my pokedex, i looked all over for pokemon.", "not only that, i assembled teams that would beat any pokemon type.", "and now", "i'm the pokemon league champion", "red!", "do you know what that means?", "i'll tell you", "i am the most powerful trainer in the world"};
    bool isFrozen = true;
    float playerGreenAngle;
    float playerAngle;
    Vector3 playerGreenDiff;
    Vector3 ofset;
    Vector3 lastVec;
    KeyCode lastKey;
    Animator anim;
    AudioSource selfAudio;
    Color fadeColor = Color.black;
    public void StepSound()
    {
        selfAudio.clip = step;
        selfAudio.Play();
    }
    private void Start()
    {
        fadeObject.enabled = false;
        anim = GetComponent<Animator>();
        selfAudio = GetComponent<AudioSource>();
        StartCoroutine(StartAnim());
        //StartCoroutine(BarAnimation());
    }
    private IEnumerator StartAnim()
    {
        selfAudio.enabled = false; // prevent presound
        yield return new WaitForSeconds(0.5f);
        wall.enabled = false;
        doorAnim.SetBool("open", true);
        yield return new WaitForSeconds(1.75f);
        selfAudio.enabled = true; // prevent presound
        anim.SetBool("isStill", false);
        for (int i = 0; i < 50; i++) {
            transform.position += Vector3.up * speed;
            yield return new WaitForSeconds(0.016f);
        }
        anim.SetBool("isStill", true);
        doorAnim.SetBool("open", false);
        yield return new WaitForSeconds(0.5f);
        isFrozen = false;
        wall.enabled = true;
        yield return null;
    }
    private IEnumerator TextAnim()
    {
        int count = 0;
        bool skipLine = false;
        text.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);
        anim.SetBool("isStill", true);
        isFrozen = true;
        yield return new WaitForSeconds(0.25f);
        selfAudio.clip = ding;
        foreach (string line in lines )
        {
            skipLine = false;
            text.text = "";
            foreach (char letter in line)
            {
                if (Input.GetKey(KeyCode.Space)) skipLine = true;
                if (skipLine) break;
                text.text += letter;
                yield return new WaitForSeconds(0.075f);
            }
            if (skipLine) {
                text.text = line;
                yield return new WaitUntil(() => !Input.GetKey(KeyCode.Space));
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            yield return new WaitUntil(() => !Input.GetKey(KeyCode.Space));
            selfAudio.Play();
        }
        musicManager.SwitchTrack();
        fadeObject.enabled = true;
        for (int i = 0; i <= 10; i++)
        {
            fadeColor.a = math.abs(i) * 0.1f;
            fadeMaterial.color = fadeColor;
            yield return new WaitForSeconds(0.015f);
            if (i == 10 && count < 2)
            {
                i = -10;
                count++;
            }    
        }
        fadeObject.enabled = false;
        text.gameObject.SetActive(false);
        textBox.gameObject.SetActive(false);
        StartCoroutine(BarAnimation());
    }
    private IEnumerator BarAnimation()
    {
        float modifier = ((float)Screen.width) / 625;
        fadeColor = Color.white;
        fadeColor.a = 0f;
        fadeMaterial.color = fadeColor;
        fadeObject.enabled = true;

        for (int i = 0; i < 100; i++)
        {
            orangeBar.position += Vector3.right * 7.5f * modifier;
            blueBar.position += Vector3.left * 7.5f * modifier;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 200; i++)
        {
            orangeBarWhite.position += Vector3.right * 15 * modifier;
            blueBarWhite.position += Vector3.left * 15 * modifier;
            if (blueBarWhite.position.x < -200)
                blueBarWhite.localPosition = Vector3.zero;
            if (orangeBarWhite.position.x > 200)
                orangeBarWhite.localPosition = Vector3.zero;
            
            if (i > 15 && i < 100) {
                playerSprite.position += Vector3.left * 7.5f * modifier;
                rivalSprite.position += Vector3.right * 7.5f * modifier;
            }

            if (i > 125)
            {
                fadeColor.a = 0.01333f * (i - 125);
                fadeMaterial.color = fadeColor;
            }

            yield return new WaitForSeconds(0.02f);
        }
        SceneManager.LoadScene("Battle");
        yield return null;
    }

    private void Update()
    {
        if (isFrozen)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerGreenDiff = green.position - transform.position;
            playerGreenAngle = Vector3.Angle(playerGreenDiff, transform.up);
            playerAngle = anim.GetInteger("dir") * 90;
            if (playerGreenDiff.x < 0 && playerAngle == 270)
                playerAngle = 90;
            if (playerGreenDiff.magnitude <= activateDifference && math.abs(playerAngle - playerGreenAngle) < 10)
            {
                selfAudio.clip = ding;
                selfAudio.Play();
                StartCoroutine(TextAnim());
            }
            
        }
    }

    void FixedUpdate()
    {
        if (isFrozen)
            return;
        ofset = Vector3.zero;
        if (Input.GetKey(lastKey))
        {
            ofset = lastVec;
        } else {
            if (Input.GetKey(KeyCode.W)) {
                anim.SetInteger("dir", 0);
                ofset = Vector3.up;
                lastKey = KeyCode.W;
            } else if (Input.GetKey(KeyCode.S)) {
                anim.SetInteger("dir", 2);
                ofset = Vector3.down;
                lastKey = KeyCode.S;
            } else if (Input.GetKey(KeyCode.D)) {
                anim.SetInteger("dir", 1);
                ofset = Vector3.right;
                lastKey = KeyCode.D;
            } else if (Input.GetKey(KeyCode.A)) {
                anim.SetInteger("dir", 3);
                ofset = Vector3.left;
                lastKey = KeyCode.A;
            } else
            {
                anim.SetBool("isStill", true);
                return;
            }
        }
        anim.SetBool("isStill", false);
        lastVec = ofset;
        transform.position += ofset * speed;
    }
}
