using System;

namespace Entidades
{
    public class DetalleTicket
    {
        public int Id { get; set; }
        public int IdTicket { get; set; }
        public string CodigoProducto { get; set; }
        public decimal Precio { get; set; }
        public decimal ISV { get; set; }
        public decimal Total { get; set; }
        public decimal Descuento { get; set; }          
        public string DescSolicitud { get; set; }
        public string DescRespuesta { get; set; }
        public DateTime Fecha { get; set; }

        public DetalleTicket()
        {
        }

        public DetalleTicket(int id, int idTicket, string codigoProducto, decimal precio, decimal isv, decimal total, decimal descuento, string descsolicitud, string descrespuesta, DateTime fecha)
        {
            Id=id;
            IdTicket=idTicket;
            CodigoProducto=codigoProducto;
            Precio=precio;
            ISV=isv;
            Total=total;
            Descuento=descuento;
            DescSolicitud=descsolicitud;
            DescRespuesta=descrespuesta;
            Fecha=fecha;

        }
    }
}
