using System;
using System.Collections.Generic;
using System.Text;

namespace DrawManager.Domain.Entities
{
    public class Entrant : Entity<int>
    {
        /// <summary>
        /// Codigo del concursante. Cedula
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Nombre y Apellidos del concursante.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Sucursal 1
        /// </summary>
        public string Subsidiary { get; set; }
        /// <summary>
        /// Oficina
        /// </summary>
        public string Office { get; set; }
        /// <summary>
        /// Unidad
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// Departamento
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// SubDepartamento
        /// </summary>
        public string SubDepartment { get; set; }
        /// <summary>
        /// Region
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// Ciudad
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Sucursal 2
        /// </summary>
        public string BranchOffice { get; set; }

        /// <summary>
        /// Participaciones.
        /// </summary>
        public ICollection<DrawEntry> DrawEntries { get; set; }
        /// <summary>
        /// Pasos de selección.
        /// </summary>
        public ICollection<PrizeSelectionStep> SelectionSteps { get; set; }

        //public Entrant()
        //{
        //    DrawEntries = new List<DrawEntry>();
        //    SelectionSteps = new List<PrizeSelectionStep>();
        //}
    }
}
