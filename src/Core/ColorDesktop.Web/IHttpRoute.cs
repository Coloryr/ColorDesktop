using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ColorDesktop.Web;

public interface IHttpRoute
{
    /// <summary>
    /// 处理网络路由
    /// </summary>
    /// <param name="context">这次请求</param>
    /// <returns>true表示处理成功</returns>
    Task<bool> Process(HttpContext context);
}
