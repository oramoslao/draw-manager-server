using DrawManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrawManager.Domain.Entities
{
    public class PrizeSelectionStep : Entity<int>
    {
        /// <summary>
        /// Id del premio. Parte de la llave primaria.
        /// </summary>
        public int PrizeId { get; set; }

        /// <summary>
        /// Premio.
        /// </summary>
        public Prize Prize { get; set; }

        /// <summary>
        /// Id del participante. Parte de la llave primaria.
        /// </summary>
        public int EntrantId { get; set; }

        /// <summary>
        /// Participante.
        /// </summary>
        public Entrant Entrant { get; set; }

        /// <summary>
        /// Id de la participación ganadora. Parte de la llave primaria.
        /// </summary>
        public int DrawEntryId { get; set; }

        /// <summary>
        /// Participación ganadora.
        /// </summary>
        public DrawEntry DrawEntry { get; set; }

        /// <summary>
        /// Fecha de registro.
        /// </summary>
        public DateTime RegisteredOn { get; set; }
        /// <summary>
        /// Tipo del paso de selección del premio.
        /// </summary>
        public PrizeSelectionStepType PrizeSelectionStepType { get; set; }
        
    }
}
