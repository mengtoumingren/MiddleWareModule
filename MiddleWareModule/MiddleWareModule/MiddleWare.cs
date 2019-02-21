﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleWareModule
{
    public class MiddleWare<T>
    {
        public List<Func<T, Func<Task>, Task>> MiddleWares = null;
        public MiddleWare()
        {
            MiddleWares = new List<Func<T, Func<Task>, Task>>();
        }

        public void Add(Func<T, Func<Task>, Task> middleWare)
        {
            MiddleWares.Add(middleWare);
        }
        public void Add(IMiddleWare<T> middleWare)
        {
            MiddleWares.Add(async (s, next) =>
            {
                await middleWare.DealWith(s, next);
            });
        }
        public async Task Execute(T t)
        {
            //列表数据倒序，从最后一个注册的func一层一层往上包
            MiddleWares.Reverse();
            await MiddleWares[MiddleWares.Count - 1].Invoke(t, Execute(t,-1, MiddleWares.Count - 2, null));

        }

        private Func<Task> Execute(T t,int index ,int count,Func<Task> func)
        {
            //Console.WriteLine(index+"/"+count);
            if(index< count)
            {
                return Execute(t,++index,count, async () => await MiddleWares[index].Invoke(t, func));
            }
            return func;
        }
    }
}
