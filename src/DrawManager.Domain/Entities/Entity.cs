using System;
using System.Collections.Generic;
using System.Text;

namespace DrawManager.Domain.Entities
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Id de la Entidad. Autogenerado. Llave primaria.
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }
    }
}
