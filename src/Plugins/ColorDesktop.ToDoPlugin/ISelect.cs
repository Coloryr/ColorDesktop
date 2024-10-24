using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.ToDoPlugin;

public interface ISelect<T>
{
    public void Select(T item);
}
