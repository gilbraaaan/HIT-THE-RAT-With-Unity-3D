using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public bool activeCollider = false;
    public GameObject Palu, Tikus, UIMulai;
    public GameObject cloninganDariTikus;
    public bool waktu = false;
    public float waktuInti, kecepatanWaktu = 1f;
    int score;
    public TMP_Text textScore, ketTextScore, cloneDariTextScore;
    public GameObject ParentKetScore;
    public List<GameObject> Lubang = new List<GameObject>();
    public void Mulai()
    {
        waktu = true;
        UIMulai.SetActive(false);
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    private void Update()
    {
        textScore.text = "SCORE : " + score.ToString();
        RaycastHit hit;
        if(activeCollider == true)
        {
            #region KLIK/SENTUH
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    DeteksiKolider(hit);
                }
            }else if(Input.touchCount > 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if(Physics.Raycast(ray, out hit, 100))
                {
                    DeteksiKolider(hit);
                }
            }
            #endregion
        }
        if(waktu == true)
        {
            waktuInti += 1f / kecepatanWaktu * Time.deltaTime;
            if(waktuInti > 1f)
            {
                menentukanTikus();
                waktuInti = 0f;
                waktu = false;
            }
        }
    }

    void menentukanTikus()
    {
        GameObject cloneTikus = Instantiate(Tikus, Tikus.transform.position, Tikus.transform.rotation) as GameObject;
        cloninganDariTikus = cloneTikus;
        int pilihAcak = Random.Range(0, Lubang.Count - 1);
        cloneTikus.transform.position = new Vector3(Lubang[pilihAcak].transform.position.x,
            Lubang[pilihAcak].transform.position.y - 0.5f, Lubang[pilihAcak].transform.position.z);
        activeCollider = true;
        StartCoroutine(waktuPemrosesan(cloneTikus));
        for (int i = 0; i < Lubang.Count; i++)
        {
            Lubang[i].GetComponent<MeshRenderer>().enabled = false;
        }
        Lubang[pilihAcak].tag = "AdaTikus";
    }

    IEnumerator waktuPemrosesan(GameObject cloninganTikus)
    {
        yield return new WaitForSeconds(2f);
        Destroy(cloninganTikus);
        activeCollider = false;
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < Lubang.Count; i++)
        {
            Lubang[i].GetComponent<MeshRenderer>().enabled = true;
            Lubang[i].tag = "Lubang";
        }
        waktu = true;
    }

    void DeteksiKolider(RaycastHit Hit)
    {
        if(Hit.collider.tag == "Lubang" || Hit.collider.tag == "AdaTikus")
        {
            GameObject clonePalu = Instantiate(Palu, Palu.transform.position, Palu.transform.rotation) as GameObject;
            clonePalu.transform.position = Hit.collider.gameObject.transform.position;
            activeCollider = false;
            Destroy(clonePalu, 0.5f);
        }
    }

    public void TikusTerpukul()
    {
        if (cloninganDariTikus != null)
        {
            cloninganDariTikus.GetComponent<Animator>().SetTrigger("KEPUKUL");
            activeCollider = false;
            score += 1;
            cloninganTextScore();
            cloneDariTextScore.text = "+1";
        }
    }
    public void TikusTidakTerpukul()
    {
        if(score > 0)
        {
            score -= 1;
        }
        cloninganTextScore();
        cloneDariTextScore.text = "-1";
    }

    void cloninganTextScore()
    {
        GameObject cloneKetScore = Instantiate(ketTextScore.gameObject, ketTextScore.transform.position,
            ketTextScore.transform.rotation) as GameObject;
        cloneKetScore.SetActive(true);
        cloneKetScore.transform.SetParent(ParentKetScore.transform);
        cloneDariTextScore = cloneKetScore.GetComponent<TMP_Text>();
        Destroy(cloneDariTextScore.gameObject, 1.5f);
    }
}
