using DrawManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawManager.Domain.Entities
{
    public class Prize : Entity<int>
    {
        /// <summary>
        /// Nombre del premio.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Descripción del premio.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Intentos perdedores para escoger el ganador del premio.
        /// </summary>
        public int AttemptsUntilChooseWinner { get; set; }
        /// <summary>
        /// Fecha de entrega del premio.
        /// </summary>
        public DateTime? ExecutedOn { get; set; }
        /// <summary>
        /// Id del sorteo al que pertenece. Llave foránea.
        /// </summary>
        public int DrawId { get; set; }

        /// <summary>
        /// Bandera que indica si el premio ya fue entregado.
        /// </summary>
        public bool Delivered => SelectionSteps.Count == AttemptsUntilChooseWinner + 1 && SelectionSteps.Any(st => st.PrizeSelectionStepType == PrizeSelectionStepType.Winner);

        /// <summary>
        /// Sorteo.
        /// </summary>
        public Draw Draw { get; set; }
        /// <summary>
        /// Pasos de selección.
        /// </summary>
        public List<PrizeSelectionStep> SelectionSteps { get; set; }

        public Prize()
        {
            SelectionSteps = new List<PrizeSelectionStep>();
        }
    }
}
