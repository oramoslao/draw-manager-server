using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DrawManager.Domain.Entities
{
    public class Draw : Entity<int>
    {
        /// <summary>
        /// Nombre del sorteo.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Descripcion del sorteo.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Bandera que especifica si el concurso soporta multiples participaciones por parte de un mismo concursante.
        /// </summary>
        public bool AllowMultipleParticipations { get; set; }
        /// <summary>
        /// Fecha para la que se encuentra programada el sorteo.
        /// </summary>
        public DateTime ProgrammedFor { get; set; }
        /// <summary>
        /// Fecha de ejecucion y cierre del sorteo.
        /// </summary>
        public DateTime? ExecutedOn { get; set; }
        /// <summary>
        /// Grupo del sorteo.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Cantidad de premios del sorteo.
        /// </summary>
        public int PrizesQty => Prizes.Count;
        /// <summary>
        /// Cantidad de participaciones del sorteo.
        /// </summary>
        public int EntriesQty => DrawEntries.Count;
        /// <summary>
        /// Bandera que indica cuando el sorteo se ha completado.
        /// </summary>
        public bool IsCompleted => Prizes.Count > 0 && Prizes.ToList().TrueForAll(p => p.Delivered);

        /// <summary>
        /// Premios.
        /// </summary>
        public ICollection<Prize> Prizes { get; set; }
        /// <summary>
        /// Participaciones.
        /// </summary>
        public ICollection<DrawEntry> DrawEntries { get; set; }

        //public Draw()
        //{
        //    Prizes = new List<Prize>();
        //    DrawEntries = new List<DrawEntry>();
        //}
    }
}
