﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleWareModule
{
    public interface IMiddleWare
    {
        Task DealWith(object context, Func<Task> next);
    }
}
