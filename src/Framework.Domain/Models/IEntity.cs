using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Models;

public interface IEntity<out TKey>
{
    public TKey Id { get; }
}
