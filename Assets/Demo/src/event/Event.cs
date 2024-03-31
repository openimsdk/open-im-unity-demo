using System;
using System.Collections.Generic;

public delegate void CallBack();
public delegate void CallBack<T>(T arg);
public delegate void CallBack<T, X>(T arg1, X arg2);
public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);
public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
public delegate void CallBack<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);
public class EventDispator
{
    Dictionary<EventType, Delegate> m_EventTable = new Dictionary<EventType, Delegate>();

    public EventDispator()
    {

    }

    void OnListenerAdding(EventType eventType, Delegate callBack)
    {
        //先判断事件码是否存在于事件表中
        //如果不存在
        if (!m_EventTable.ContainsKey(eventType))
        {
            //先给字典添加事件码,委托设置为空
            m_EventTable.Add(eventType, null);
        }

        //当前事件码和委托是否一致
        //如果不一致,是不能绑定在一起的
        //先把事件码传进去,接收值是 Delegate
        //这句代码是先把事件码拿出来
        Delegate d = m_EventTable[eventType];
        //d为空或d 的参数如果和callBack参数不一样
        if (d != null && d.GetType() != callBack.GetType())
        {
            //抛出异常
            throw new Exception(string.Format("尝试为事件{0}添加不同事件的委托,当前事件所对应的委托是{1},要添加的委托类型{2}", eventType, d.GetType(), callBack.GetType()));
        }
    }
    void OnListenerRemoving(EventType eventType, Delegate callBack)
    {
        //判断是否包含指定键
        if (m_EventTable.ContainsKey(eventType))
        {
            //先把事件码拿出来
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            else if (d.GetType() != callBack.GetType())//判断移除的委托类型是否和d的一致
            {
                throw new Exception(string.Format("移除监听错误，尝试为事件{}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        else  //不存在事件码的情况
        {
            throw new Exception(string.Format("移除监听错误;没有事件码", eventType));
        }
    }

    /// <summary>
    /// 移除监听后的判断，主要为精简代码
    /// </summary>
    /// <param name="eventType"></param>
    void OnListenerRemoved(EventType eventType)
    {
        //判断当前的事件码所对应的事件是否为空
        //如果为空，事件码就没用了，就将事件码移除
        if (m_EventTable[eventType] == null)
        {
            //移除事件码
            m_EventTable.Remove(eventType);
        }
    }


    //*********************************************************************************************************************************
    /// <summary>
    /// 添加监听 静态 无参
    /// </summary>
    /// <param name="eventType">事件码</param>
    /// <param name="callBack">委托</param>
    public void AddListener(EventType eventType, CallBack callBack)
    {
        //调用事件监听是不是有错误方法
        OnListenerAdding(eventType, callBack);

        //已经存在的委托进行关联,相当于链式关系,再重新赋值
        //两个类型不一致,要强转换
        //委托对象可使用 "+" 运算符进行合并。
        //一个合并委托调用它所合并的两个委托。只有相同类型的委托可被合并。"-" 运算符可用于从合并的委托中移除组件委托。
        //使用委托的这个有用的特点，您可以创建一个委托被调用时要调用的方法的调用列表。这被称为委托的 多播（multicasting），也叫组播。
        //下面的程序演示了委托的多播
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] + callBack;
    }

    /// <summary>
    /// 添加监听 静态 一个参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    /// 因为有一个参数，方法后要加一个泛型“<T>”,大写的T来代表
    /// CallBack 也是一个泛型，方法是有参数的，所以CallBack也是有参数的
    /// 除此之外其它与无参方法基本一致
    /// 泛函数 T 可以指定为任意的类型，多参数也是
    public void AddListener<T>(EventType eventType, CallBack<T> callBack)
    {
        //调用事件监听是不是有错误方法
        OnListenerAdding(eventType, callBack);

        //这里是有参方法需要更改的地方
        //强制转换类型要加一个泛型 "<T>"
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] + callBack;
    }

    /// <summary>
    /// 添加监听 静态 两个参数
    /// </summary>
    public void AddListener<T, X>(EventType eventType, CallBack<T, X> callBack)
    {
        OnListenerAdding(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] + callBack;
    }

    /// <summary>
    /// 添加监听 静态 三个参数
    /// </summary>
    public void AddListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
    {
        OnListenerAdding(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X, Y>)m_EventTable[eventType] + callBack;
    }

    /// <summary>
    /// 添加监听 静态 四个参数
    /// </summary>
    public void AddListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack)
    {
        OnListenerAdding(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X, Y, Z>)m_EventTable[eventType] + callBack;
    }

    /// <summary>
    /// 添加监听 静态 五个参数
    /// </summary>
    public void AddListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack)
    {
        OnListenerAdding(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[eventType] + callBack;
    }


    //*******************************************************************************************************************************
    /// <summary>
    /// 移除监听 静态 无参
    /// </summary>
    /// <param name="eventType">事件码</param>
    /// <param name="callBack">委托</param>
    public void RemoveListener(EventType eventType, CallBack callBack)
    {
        //移除监听前的判断
        OnListenerRemoving(eventType, callBack);

        //这句话是主要的
        //事件码对应的委托-callBack 然后再重新赋值，强转型首字母要大写
        //移除监听
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] - callBack;

        //移除监听后的判断
        OnListenerRemoved(eventType);
    }

    /// <summary>
    /// 移除监听 静态 一个参数
    /// </summary>
    /// <param name="eventType">事件码</param>
    /// <param name="callBack">委托</param>
    public void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        //这里是有参方法需要更改的地方
        //强制转换类型要加一个泛型 "<T>"
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }

    /// <summary>
    /// 移除监听 静态 两个参数
    /// </summary>
    public void RemoveListener<T, X>(EventType eventType, CallBack<T, X> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }

    /// <summary>
    /// 移除监听 静态 三个参数
    /// </summary>
    public void RemoveListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X, Y>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }


    /// <summary>
    /// 移除监听 静态 四个参数
    /// </summary>
    public void RemoveListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X, Y, Z>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }

    /// <summary>
    /// 移除监听 静态 五个参数
    /// </summary>
    public void RemoveListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }

    //******************************************************************************************************************************
    /// <summary>
    /// 广播监听 静态 无参
    /// </summary>
    /// <param name="eventType">事件码</param>
    /// 把事件码所对应的委托从m_EventTable 字典表中取出来，然后调用这个委托
    public void Broadcast(EventType eventType)
    {
        Delegate d;
        //如果拿到这个值成功了，对这个委托进行一个广播
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            //把d强转型CallBack类型
            CallBack callBack = d as CallBack;
            if (callBack != null)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
            }
        }
    }


    /// <summary> 
    /// 广播监听 静态 一个参
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="arg"></param>
    /// 以为有参数，所以在方法后面加一个参数 T arg
    public void Broadcast<T>(EventType eventType, T arg)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            //带有一个参数的委托
            CallBack<T> callBack = d as CallBack<T>;
            if (callBack != null)
            {
                //把参数传过去
                callBack(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
            }
        }
    }

    /// <summary>
    /// 广播 静态 两个参数
    /// </summary>
    public void Broadcast<T, X>(EventType eventType, T arg1, X arg2)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T, X> callBack = d as CallBack<T, X>;
            if (callBack != null)
            {
                callBack(arg1, arg2);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
            }
        }
    }

    /// <summary>
    /// 广播 静态 三个参数
    /// </summary>
    public void Broadcast<T, X, Y>(EventType eventType, T arg1, X arg2, Y arg3)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T, X, Y> callBack = d as CallBack<T, X, Y>;
            if (callBack != null)
            {
                callBack(arg1, arg2, arg3);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
            }
        }
    }

    /// <summary>
    /// 广播 静态 四个参数
    /// </summary>
    public void Broadcast<T, X, Y, Z>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T, X, Y, Z> callBack = d as CallBack<T, X, Y, Z>;
            if (callBack != null)
            {
                callBack(arg1, arg2, arg3, arg4);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
            }
        }
    }

    /// <summary>
    /// 广播 静态 五个参数
    /// </summary>
    public void Broadcast<T, X, Y, Z, W>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T, X, Y, Z, W> callBack = d as CallBack<T, X, Y, Z, W>;
            if (callBack != null)
            {
                callBack(arg1, arg2, arg3, arg4, arg5);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
            }
        }
    }
}
