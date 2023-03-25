using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class TicketDB
    {
        string cadena = "server=localhost; user=root; database=miticket; password=Euniceso.6;";

        public bool Guardar(Ticket ticket, List<DetalleTicket> detalles)
        {
            bool inserto = false;
            int idTicket = 0;
            try
            {
                StringBuilder sqlTicket = new StringBuilder();
                sqlTicket.Append(" INSERT INTO ticket (Fecha, IdentidadCliente, CodigoUsuario, ISV, Descuento, SubTotal, Total, DescSolicitud, DescRespuesta) VALUES (@Fecha, @IdentidadCliente, @CodigoUsuario, @ISV, @Descuento, @SubTotal, @Total, @DescSolicitud, @DescRespuesta); ");
                sqlTicket.Append(" SELECT LAST_INSERT_ID(); ");

                StringBuilder sqlDetalle = new StringBuilder();
                sqlDetalle.Append(" INSERT INTO detalleticket (IdTicket, CodigoProducto, Precio, ISV, Total, Descuento, Fecha, DescSolicitud, DescRespuesta) VALUES (@IdTicket, @CodigoProducto, @Precio, @ISV, @Total, @Descuento, @Fecha, @DescSolicitud, @DescRespuesta); ");

                using (MySqlConnection con = new MySqlConnection(cadena))
                {
                    con.Open();

                    MySqlTransaction tran = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                    try
                    {
                        using (MySqlCommand cmd1 = new MySqlCommand(sqlTicket.ToString(), con, tran))
                        {
                            cmd1.CommandType = System.Data.CommandType.Text;
                            cmd1.Parameters.Add("@Fecha", MySqlDbType.DateTime).Value = ticket.Fecha;
                            cmd1.Parameters.Add("@IdentidadCliente", MySqlDbType.VarChar, 25).Value = ticket.IdentidadCliente;
                            cmd1.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = ticket.CodigoUsuario;
                            cmd1.Parameters.Add("@ISV", MySqlDbType.Decimal).Value = ticket.ISV;
                            cmd1.Parameters.Add("@Descuento", MySqlDbType.Decimal).Value = ticket.Descuento;
                            cmd1.Parameters.Add("@SubTotal", MySqlDbType.Decimal).Value = ticket.SubTotal;
                            cmd1.Parameters.Add("@Total", MySqlDbType.Decimal).Value = ticket.Total;
                            cmd1.Parameters.Add("@DescSolicitud", MySqlDbType.VarChar, 250).Value = ticket.DescSolicitud;
                            cmd1.Parameters.Add("@DescRespuesta", MySqlDbType.VarChar, 250).Value = ticket.DescRespuesta;
                            idTicket = Convert.ToInt32(cmd1.ExecuteScalar());
                        }

                        foreach (DetalleTicket detalle in detalles)
                        {
                            using (MySqlCommand cmd2 = new MySqlCommand(sqlDetalle.ToString(), con, tran))
                            {
                                cmd2.CommandType = System.Data.CommandType.Text;
                                cmd2.Parameters.Add("@IdTicket", MySqlDbType.Int32).Value = idTicket;
                                cmd2.Parameters.Add("@CodigoProducto", MySqlDbType.VarChar, 80).Value = detalle.CodigoProducto;
                                cmd2.Parameters.Add("@Precio", MySqlDbType.Decimal).Value = detalle.Precio;
                                cmd2.Parameters.Add("@Total", MySqlDbType.Decimal).Value = detalle.Total;
                                cmd2.Parameters.Add("@ISV", MySqlDbType.Decimal).Value = detalle.ISV;
                                cmd2.Parameters.Add("@Descuento", MySqlDbType.Decimal).Value = detalle.Descuento;
                                cmd2.Parameters.Add("@DescSolicitud", MySqlDbType.VarChar, 250).Value = detalle.DescSolicitud;
                                cmd2.Parameters.Add("@DescRespuesta", MySqlDbType.VarChar, 250).Value = detalle.DescRespuesta;
                                cmd2.Parameters.Add("@Fecha", MySqlDbType.DateTime).Value = detalle.Fecha;
                                cmd2.ExecuteNonQuery();
                            }

                        }
                        tran.Commit();
                        inserto = true;
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        inserto=false;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return inserto;
        }

    }
}
