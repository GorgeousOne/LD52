using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    public bool _isfilled;
    [SerializeField] private Sprite _spriteFilled;
    [SerializeField] private Sprite _spriteEmpty;

    private SpriteRenderer _icon;

    void OnEnable()
    {
        _icon = transform.GetComponent<SpriteRenderer>();
        _icon.sprite = _spriteEmpty;
    }
    // Start is called before the first frame update
    void Start()
    {
        _isfilled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void fill()
    {
        _isfilled = true;
        _icon.sprite = _spriteFilled;
    }
    public void empty()
    {
        _isfilled = false;
        _icon.sprite = _spriteEmpty;
    }
    public Bucket distancesqr(Vector3 pos,ref float sqrdis)
    {
        float bucketdistance = (pos - transform.position).sqrMagnitude;
        if (bucketdistance < sqrdis)
        {
            sqrdis = bucketdistance;
            return this;
        }
        return null;

    }
}
