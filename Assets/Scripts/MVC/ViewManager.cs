using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

//视图信息类
public class ViewInfo
{
    public string prefabName; //视图预制体名称
    public Transform parentTf; //父级
    public BaseController controller; //视图所属控制器
    public int sortingOrder; //显示层级 改变显示顺序
}

///<summary>
///视图管理器
///</summary>
public class ViewManager
{
    public Transform canvasTf; //画布组件
    public Transform worldCanvasTf; //世界画布组件

    private Dictionary<int, IBaseView> _opens; //开启中的视图
    private Dictionary<int, IBaseView> _viewCache; //视图缓存
    private Dictionary<int, ViewInfo> _views; //注册的视图信息

    public ViewManager()
    {
        canvasTf = GameObject.Find("Canvas").transform;
        worldCanvasTf = GameObject.Find("WorldCanvas").transform;
        _opens = new();
        _viewCache = new();
        _views = new();
    }

    public void Register(int key, ViewInfo viewInfo)
    {
        if (!_views.ContainsKey(key))
        {
            _views.Add(key, viewInfo);
        }
    } //注册视图信息

    public void Register(ViewType viewType, ViewInfo viewInfo)
    {
        if (!_views.ContainsKey((int)viewType))
        {
            _views.Add((int)viewType, viewInfo);
        }
    }

    public void UnRegister(int key)
    {
        if (_views.ContainsKey(key))
        {
            _views.Remove(key);
        }
    } //移除视图信息

    public void RemoveView(int key)
    {
        _views.Remove(key);
        _viewCache.Remove(key);
        _opens.Remove(key);
    } //移除面板

    public void RemoveViewByController(BaseController ctl)
    {
        foreach (var item in _views)
        {
            if (item.Value.controller == ctl)
            {
                RemoveView(item.Key);
            }
        }
    } //移除控制器中的面板视图

    public bool IsOpen(int key)
    {
        return _opens.ContainsKey(key);
    } //判断某个视图是否开启

    public IBaseView GetView(int key)
    {
        if (_opens.ContainsKey(key))
        {
            return _opens[key];
        }
        if (_viewCache.ContainsKey(key))
        {
            return _viewCache[key];
        }
        return null;
    } //获取某个视图

    public T GetView<T>(ViewType viewType)
        where T : class, IBaseView
    {
        IBaseView view = GetView((int)viewType);
        if (view != null)
        {
            return view as T;
        }
        return null;
    }

    public T GetView<T>(int key)
        where T : class, IBaseView
    {
        IBaseView view = GetView(key);
        if (view != null)
        {
            return view as T;
        }
        return null;
    }

    public void Destroy(int key)
    {
        IBaseView view = GetView(key);
        if (view != null)
        {
            UnRegister(key);
            view.Destroy();
            _viewCache.Remove(key);
        }
    } //销毁某个视图

    public void Close(ViewType viewType, params object[] args)
    {
        Close((int)viewType, args);
    }

    public void Close(int key, params object[] args)
    {
        if (IsOpen(key))
        {
            IBaseView view = GetView(key);
            if (view != null)
            {
                _opens.Remove(key);
                view.Close(args);
                _views[key].controller.CloseView(view);
            }
        }
    } //关闭面板

    public void CloseAll()
    {
        List<IBaseView> list = _opens.Values.ToList();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            Close(list[i].ViewID);
        }
    }

    public void Open(ViewType viewType, params object[] args)
    {
        Open((int)viewType, args);
    }

    public void Open(int key, params object[] args)
    {
        IBaseView view = GetView(key);
        ViewInfo viewInfo = _views[key];

        if (view == null)
        {
            //不存在的视图 进行资源加载
            string type = ((ViewType)key).ToString(); //类型的字符串脚本跟脚本名称对应
            GameObject viewObj = GameObject.Instantiate(
                Resources.Load<GameObject>($"View/{viewInfo.prefabName}"),
                viewInfo.parentTf
            );
            Canvas canvas = viewObj.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = viewObj.AddComponent<Canvas>();
            }
            if (viewObj.GetComponent<GraphicRaycaster>() == null)
            {
                viewObj.AddComponent<GraphicRaycaster>();
            }
            canvas.overrideSorting = true; //可以设置层级
            canvas.sortingOrder = viewInfo.sortingOrder;
            view = viewObj.AddComponent(System.Type.GetType(type)) as IBaseView; //添加对应View脚本
            view.ViewID = key; //视图ID
            view.Controller = viewInfo.controller; //设置控制器
            _viewCache.Add(key, view); //添加到视图缓存
            viewInfo.controller.OnLoadView(view);
        }

        //如果已经打开 直接返回
        if (_opens.ContainsKey(key))
        {
            return;
        }
        _opens.Add(key, view);

        //如果已经初始化 直接打开
        if (view.IsInit())
        {
            view.SetVisible(true);
            view.Open(args);
            viewInfo.controller.OpenView(view);
        }
        else
        {
            view.InitUI();
            view.InitData();
            view.Open(args);
            viewInfo.controller.OpenView(view);
        }
    }

    //显示伤害数字
    public void ShowHitNum(string num, Color color, Vector3 pos)
    {
        GameObject obj = GameObject.Instantiate(
            Resources.Load<GameObject>("View/HitNum"),
            worldCanvasTf
        );
        obj.transform.position = pos;
        obj.transform.DOMove(pos + Vector3.up * 1.75f, 0.65f).SetEase(Ease.OutBack);
        GameObject.Destroy(obj, 0.75f);
        Text hitText = obj.GetComponent<Text>();
        hitText.text = num;
        hitText.color = color;
    }
}
