using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour, IBaseView
{
    public int ViewID { get; set; }
    public BaseController Controller { get; set; }

    protected Canvas _canvas;

    protected Dictionary<string, GameObject> m_cache_gos = new(); //缓存的物体字典

    private bool _isInit = false; //是否初始化

    void Awake()
    {
        _canvas = gameObject.GetComponent<Canvas>();
        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    protected virtual void OnAwake() { }

    protected virtual void OnStart() { }

    public void ApplyControllerFunc(int controllerKey, string eventName, params object[] args)
    {
        this.Controller.ApplyControllerFunc(controllerKey, eventName, args);
    }

    public void ApplyFunc(string eventName, params object[] args)
    {
        this.Controller.ApplyFunc(eventName, args);
    }

    public virtual void Close(params object[] args)
    {
        SetVisible(false);
    }

    public void Destroy()
    {
        Controller = null;
        Destroy(gameObject);
    }

    public void InitUI() { }

    public virtual void InitData()
    {
        _isInit = true;
    }

    public bool IsInit()
    {
        return _isInit;
    }

    public bool IsShow()
    {
        return _canvas.enabled;
    }

    public virtual void Open(params object[] args) { }

    public void SetVisible(bool value)
    {
        _canvas.enabled = value;
    }

    public GameObject Find(string res)
    {

        if (!m_cache_gos.ContainsKey(res))
        {
            m_cache_gos.Add(res, transform.Find(res).gameObject);
        }
        return m_cache_gos[res];
    }

    public T Find<T>(string res)
        where T : Component
    {
        return Find(res).GetComponent<T>();
    }
}
