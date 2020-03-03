using System;
using System.Collections.Generic;
using System.Text;

namespace DrawManager.Domain.Entities
{
    public class User : Entity<int>
    {
        /// <summary>
        /// Usuario.
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Hash.
        /// </summary>
        public byte[] Hash { get; set; }
        /// <summary>
        /// Salt.
        /// </summary>
        public byte[] Salt { get; set; }
    }
}
