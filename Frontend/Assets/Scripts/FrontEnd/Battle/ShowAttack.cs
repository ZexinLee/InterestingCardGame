using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAttack : MonoBehaviour
{   
    public static void Attack()
    {       
        foreach(Action act in Data.InfoList)
        {
                       
            if (act.action == "att")
            {
                if (act.start == 10 || act.start == -10)
                {
                    Debug.Log("Start 是" + act.start);
                    Debug.Log("End 是" + act.end);
                    if (act.end > 0)
                    {
                        int tempend = act.end-1;
                        if (act.end == 8)
                        {
                            tempend = 6;
                        }
                        Global.GetInstance().Myparticles[tempend].SetActive(false);
                        Global.GetInstance().Myparticles[tempend].SetActive(true);
                        Global.GetInstance().IfAttack = false;
                        Global.GetInstance().Attacker = -1;
                        Global.GetInstance().Aim = -1;
                    }
                    else if (act.end < 0)
                    {
                        int tempend = -act.end - 1;
                        if (act.end == -8)
                        {
                            tempend = 6;
                        }
                        Global.GetInstance().Yourparticles[tempend].SetActive(false);
                        Global.GetInstance().Yourparticles[tempend].SetActive(true);
                        Global.GetInstance().IfAttack = false;
                        Global.GetInstance().Attacker = -1;
                        Global.GetInstance().Aim = -1;
                    }

                }
                else
                {
                    Debug.Log("Start 是" + act.start);
                    Debug.Log("End 是" + act.end);
                    if (act.start > 0)  //我方攻击
                    {

                        if (act.end < 0)
                        {   //攻击对方
                            int tempStart = act.start - 1;
                            int tempEnd = -act.end - 1;
                            if (act.end == -8)
                            {
                                tempEnd = 6;
                            }

                            Global.GetInstance().HasAttak[act.start - 1] = true;
                            Animation attack = Global.GetInstance().MyCardPositions[tempStart].GetComponent<Animation>();
                            if (attack.isPlaying == false)
                            {
                                attack.Play("Attacker1");
                            }


                            Global.GetInstance().Yourparticles[tempEnd].SetActive(false);
                            Global.GetInstance().Yourparticles[tempEnd].SetActive(true);
                            Global.GetInstance().IfAttack = false;
                            Global.GetInstance().Attacker = -1;
                            Global.GetInstance().Aim = -1;
                        }
                        else if (act.end > 0)
                        {
                            int tempStart = act.start - 1;
                            int tempEnd = act.end - 1;
                            if (act.end == 8)
                            {
                                tempEnd = 6;
                            }

                            Animation attack = Global.GetInstance().MyCardPositions[tempStart].GetComponent<Animation>();
                            if (attack.isPlaying == false)
                            {
                                attack.Play("Attacker1");
                            }


                            Global.GetInstance().Yourparticles[tempEnd].SetActive(false);
                            Global.GetInstance().Yourparticles[tempEnd].SetActive(true);
                            Global.GetInstance().IfAttack = false;
                            Global.GetInstance().Attacker = -1;
                            Global.GetInstance().Aim = -1;
                        }



                    }
                    else if (act.start < 0) //对方攻击
                    {
                        if (act.end > 0)
                        {    //攻击我
                            int tempStart = -act.start - 1;
                            int tempEnd = act.end - 1;
                            if (act.end == 8)
                            {
                                tempEnd = 6;
                            }

                            Animation attack = Global.GetInstance().YourCardPositions[tempStart].GetComponent<Animation>();
                            if (attack.isPlaying == false)
                            {
                                attack.Play("Attacker1");
                            }


                            Global.GetInstance().Myparticles[tempEnd].SetActive(false);
                            Global.GetInstance().Myparticles[tempEnd].SetActive(true);
                            Global.GetInstance().IfAttack = false;
                            Global.GetInstance().Attacker = -1;
                            Global.GetInstance().Aim = -1;
                        }
                        else if (act.end < 0)
                        {
                            int tempStart = -act.start - 1;
                            int tempEnd = -act.end - 1;
                            if (act.end == -8)
                            {
                                tempEnd = 6;
                            }

                            Animation attack = Global.GetInstance().YourCardPositions[tempStart].GetComponent<Animation>();
                            if (attack.isPlaying == false)
                            {
                                attack.Play("Attacker1");
                            }

                            Global.GetInstance().Myparticles[tempEnd].SetActive(false);
                            Global.GetInstance().Myparticles[tempEnd].SetActive(true);
                            Global.GetInstance().IfAttack = false;
                            Global.GetInstance().Attacker = -1;
                            Global.GetInstance().Aim = -1;
                        }
                    }
                }
            }

        }
        
    }


}
