//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoIPC22011903872.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MOVIMIENTO
    {
        public int Codigo_Movimiento { get; set; }
        public int Codigo_Partida { get; set; }
        public string Fila { get; set; }
        public string Columna { get; set; }
        public string Color_Pieza { get; set; }
    
        public virtual PARTIDA PARTIDA { get; set; }
    }
}
