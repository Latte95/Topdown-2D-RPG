using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
  // Move
  float h;
  float v;
  public float speed = 5;
  bool isHorizonMove;
  Vector2 dirVec;

  GameObject scanObject;
  Rigidbody2D rigid;
  Animator anim;
  public GameManager manager;

  void Awake()
  {
    rigid = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
  }

  void Update()
  {

    // Move Value
    // Only assign movement value when not in conversation with an NPC.
    h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
    v = manager.isAction ? 0 : Input.GetAxisRaw("Vertical");

    // Check Button Down & Up
    bool hDown = manager.isAction ? false : Input.GetButtonDown("Horizontal");
    bool vDown = manager.isAction ? false : Input.GetButtonDown("Vertical");
    bool hUp = manager.isAction ? false : Input.GetButtonUp("Horizontal");
    bool vUp = manager.isAction ? false : Input.GetButtonUp("Vertical");

    // Check Horizontal Move
    if (hDown)
      isHorizonMove = true;
    else if (vDown)
      isHorizonMove = false;
    else if (hUp || vUp)
      isHorizonMove = h != 0;

    //Direction
    if (!isHorizonMove)
    {
      if (v == 1)
        dirVec = Vector3.up;
      else if (v == -1)
        dirVec = Vector3.down;
      h = 0;
    }
    else if (isHorizonMove)
    {
      if (h == 1)
        dirVec = Vector3.right;
      else if (h == -1)
        dirVec = Vector3.left;
      v = 0;
    }

    // Animation
    if (anim.GetInteger("vAxisRaw") != v)
    {
      anim.SetBool("isChange", true);
      anim.SetInteger("vAxisRaw", (int)v);
    }
    else if (anim.GetInteger("hAxisRaw") != h)
    {
      anim.SetBool("isChange", true);
      anim.SetInteger("hAxisRaw", (int)h);
    }
    else
      anim.SetBool("isChange", false);


    // Scan Object
    if (Input.GetButtonDown("Jump") && scanObject != null)
      manager.Action(scanObject);
  }

  private void FixedUpdate()
  {
    // Move
    Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
    rigid.velocity = moveVec * speed;

    // Ray
    Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
    RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

    if (rayHit.collider != null)
    {
      scanObject = rayHit.collider.gameObject;
    }
    else
      scanObject = null;
  }
}
