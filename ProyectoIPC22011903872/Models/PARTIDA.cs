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
    
    public partial class PARTIDA
    {
        public int Codigo_Partida { get; set; }
        public int Codigo_Usuario_1 { get; set; }
        public string ResultadoLocal { get; set; }
        public int TIPO { get; set; }
        public int MODO { get; set; }
        public int CONTRINCANTE { get; set; }
        public Nullable<int> Punteo_1 { get; set; }
        public Nullable<int> Punteo_2 { get; set; }
        public Nullable<int> movimientos_1 { get; set; }
        public Nullable<int> movimientos_2 { get; set; }
        public string Tiempo_Jugador1 { get; set; }
        public string Tiempo_Jugador2 { get; set; }
        public System.DateTime Fecha { get; set; }
    
        public virtual MODO MODO1 { get; set; }
        public virtual USUARIO USUARIO { get; set; }
        public virtual TIPO_CONTRINCANTE TIPO_CONTRINCANTE { get; set; }
        public virtual TIPO_PARTIDA TIPO_PARTIDA { get; set; }
    }
}
