using System;
using System.Collections.Generic;
using System.Text;

namespace DrawManager.Domain.Entities
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
