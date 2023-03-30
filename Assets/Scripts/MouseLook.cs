using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviour
{
    private Vector3 _angles = Vector3.zero;
    private readonly float _maxAngles = 90.0f;
    
    public PhotonView photonview;

    [SerializeField]private Transform _playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom && !photonview.IsMine)
        {
            return;
        }

        if (GameManager.sharedInstance.isPaused != true &&GameManager.sharedInstance.isDead != true)
        {
            float rotateHorizontal = Input.GetAxis("Mouse X");
            float rotateVertical = Input.GetAxis("Mouse Y");
                
            _angles.y += rotateHorizontal ;
         
                            
            _angles.x -= rotateVertical ;
            _angles.x = Math.Clamp(_angles.x, -_maxAngles, _maxAngles);
                            
            _playerTransform.transform.rotation = Quaternion.Euler(_angles);   
        }
    }
}
