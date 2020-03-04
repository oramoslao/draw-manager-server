using System;
using System.Collections.Generic;
using System.Text;

namespace DrawManager.Domain.Entities
{
    public class DrawEntry : Entity<int>
    {
        /// <summary>
        /// Id del sorteo a la que pertenece la participación. Llave foránea.
        /// </summary>
        public int DrawId { get; set; }

        /// <summary>
        /// Sorteo.
        /// </summary>
        public Draw Draw { get; set; }

        /// <summary>
        /// Id del participante a la que pertenece la participación. Llave foránea.
        /// </summary>
        public int EntrantId { get; set; }

        /// <summary>
        /// Participante.
        /// </summary>
        public Entrant Entrant { get; set; }

        /// <summary>
        /// Fecha de registro de la participación.
        /// </summary>
        public DateTime RegisteredOn { get; set; }
        
        /// <summary>
        /// Pasos de selección.
        /// </summary>
        public ICollection<PrizeSelectionStep> SelectionSteps { get; set; }
    }
}
