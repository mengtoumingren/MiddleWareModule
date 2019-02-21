using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MiddleWareModule;

namespace 中间件扩展模型测试
{
    class Program
    {
        class MyProcess : IMiddleWare
        {
            public async Task DealWith(object context, Func<Task> next)
            {
                await Task.Run(() => { Console.WriteLine("     中间件第五层---开始"); });
                await next.Invoke();
                await Task.Run(() => { Console.WriteLine("     中间件第五层---结束"); });
           
            }
        }
        static void Main(string[] args)
        {
            MiddleWare<string> middleWare = new MiddleWare<string>();
            middleWare.Add(async (s, next) =>
            {
                Console.WriteLine("--异常处理层--");
                try
                {
                    await next.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"----------异常：{ex.Message}\r\n{ex.StackTrace}");
                }
                Console.WriteLine("--异常处理层--");
            });

            middleWare.Add(async (s, next) =>
            {
                Console.WriteLine(" 中间件第一层---开始");
                
                await next.Invoke();
                Console.WriteLine(" 中间件第一层---结束");
            });
            middleWare.Add(async (s, next) =>
            {
                Console.WriteLine("  中间件第二层---开始");
                await next.Invoke();
                Console.WriteLine("  中间件第二层---结束");
            });
            middleWare.Add(async (s, next) =>
            {
                Console.WriteLine("   中间件第三层---开始");
                await next.Invoke();
                Console.WriteLine("   中间件第三层---结束");
            });

            middleWare.Add(async (s, next) =>
            {
                await Task.Run(() => { Console.WriteLine("    中间件第四层---开始"); });
                await next.Invoke();
                await Task.Run(() => { Console.WriteLine("    中间件第四层---结束"); });
            });
            middleWare.Add(new MyProcess());

            middleWare.Add(async (s, next) =>
            {
                Console.WriteLine($"       暂停 3s");
                Thread.Sleep(3000);
                Console.WriteLine($"       输入的内容是：{s}");

                await Task.Delay(0);
            });

            middleWare.Execute("哈哈哈哈哈");
            //Task.WaitAll(middleWare.Execute("哈哈哈哈哈"));
            Console.ReadLine();
        }
    }
}
