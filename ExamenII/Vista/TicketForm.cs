using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Vista
{
    public partial class TicketForm : Form
    {
        public TicketForm()
        {
            InitializeComponent();
        }

        Cliente miCliente = null;
        ClienteDB clienteDB = new ClienteDB();
        Producto miProducto = null;
        ProductoDB productoDB = new ProductoDB();
        List<DetalleTicket> listaDetalles = new List<DetalleTicket>();
        TicketDB facturaDB = new TicketDB();

        decimal subTotal = 0;
        decimal isv = 0;
        decimal totalAPagar = 0;
        decimal descuento = 0;

        private void LimpiarControles()
        {
            miCliente = null;
            miProducto = null;
            listaDetalles = null;
            FechaDateTimePicker.Value = DateTime.Now;
            IdentidadTextBox.Clear();
            NombreClienteTextBox.Clear();
            CodigoProductoTextBox.Clear();
            DescripcionProductoTextBox.Clear();
            ExistenciaTextBox.Clear();
            CantidadTextBox.Clear();
            DetalleDataGridView.DataSource = null;
            subTotal = 0;
            isv = 0;
            descuento = 0;
            totalAPagar = 0;
            SubTotalTextBox.Clear();
            ISVTextBox.Clear();
            DescuentoTextBox.Clear();
            TotalTextBox.Clear();
        }

        private void GuardarButton_Click(object sender, EventArgs e)
        {
            Ticket miFactura = new Ticket();
            miFactura.Fecha = FechaDateTimePicker.Value;
            miFactura.IdentidadCliente = miCliente.Identidad;
            miFactura.CodigoUsuario = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            miFactura.SubTotal = subTotal;
            miFactura.ISV = isv;
            miFactura.Descuento = descuento;
            miFactura.Total = totalAPagar;

            bool inserto = facturaDB.Guardar(miFactura, listaDetalles);
            if (inserto)
            {
                MessageBox.Show("Factura registrada correctamente");
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();

                LimpiarControles();
            }
            else
            {
                MessageBox.Show("No se pudo registrar la factura");
            }
        }

        private void IdentidadTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(IdentidadTextBox.Text))
            {
                miCliente = new Cliente();
                miCliente = clienteDB.DevolverClientePorIdentidad(IdentidadTextBox.Text);
                NombreClienteTextBox.Text = miCliente.Nombre;
            }
            else
            {
                miCliente = null;
                NombreClienteTextBox.Clear();
            }
        }

        private void FacturaForm_Load(object sender, EventArgs e)
        {
            CodigoUsuarioTextBox.Text = System.Threading.Thread.CurrentPrincipal.Identity.Name;
        }

        private void BuscarClienteButton_Click(object sender, EventArgs e)
        {
            BuscarClienteForm buscarClienteForm = new BuscarClienteForm();
            buscarClienteForm.ShowDialog();
            miCliente = buscarClienteForm.cliente;
            IdentidadTextBox.Text = miCliente.Identidad;
            NombreClienteTextBox.Text = miCliente.Nombre;
        }

        private void CodigoProductoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(CodigoProductoTextBox.Text))
            {
                miProducto = new Producto();
                miProducto = productoDB.DevolverProductoPorCodigo(CodigoProductoTextBox.Text);
                DescripcionProductoTextBox.Text = miProducto.Descripcion;
                ExistenciaTextBox.Text = miProducto.Existencia.ToString();
            }
            else
            {
                miProducto = null;
                DescripcionProductoTextBox.Clear();
                ExistenciaTextBox.Clear();
            }
        }

        private void BuscraProductoButton_Click(object sender, EventArgs e)
        {
            BuscarProductoForm form = new BuscarProductoForm();
            form.ShowDialog();
            miProducto = new Producto();
            miProducto = form.producto;
            CodigoProductoTextBox.Text = miProducto.Codigo;
            DescripcionProductoTextBox.Text = miProducto.Descripcion;
            ExistenciaTextBox.Text = miProducto.Existencia.ToString();
        }

        private void CantidadTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(CantidadTextBox.Text))
            {
                if (Convert.ToInt32(ExistenciaTextBox.Text) > 0)
                {
                    if (Convert.ToInt32(ExistenciaTextBox.Text) >= Convert.ToInt32(CantidadTextBox.Text))
                    {
                        DetalleTicket detalle = new DetalleTicket();
                        detalle.CodigoProducto = miProducto.Codigo;
                        detalle.Precio = miProducto.Precio;
                        detalle.Cantidad = Convert.ToInt32(CantidadTextBox.Text);
                        detalle.Total = miProducto.Precio * Convert.ToDecimal(CantidadTextBox.Text);
                        detalle.Descripcion = miProducto.Descripcion;

                        subTotal += detalle.Total;
                        isv = subTotal * 0.15M;
                        totalAPagar = subTotal + isv;

                        listaDetalles.Add(detalle);
                        DetalleDataGridView.DataSource = null;
                        DetalleDataGridView.DataSource = listaDetalles;
                        SubTotalTextBox.Text = subTotal.ToString("N2");
                        ISVTextBox.Text = isv.ToString("N2");
                        TotalTextBox.Text = totalAPagar.ToString("N2");

                        miProducto = null;
                        CodigoProductoTextBox.Clear();
                        DescripcionProductoTextBox.Clear();
                        ExistenciaTextBox.Clear();
                        CantidadTextBox.Clear();
                        CodigoProductoTextBox.Focus();
                    }
                    else
                    {
                        MessageBox.Show("No hay suficientes productos de: " + miProducto.Descripcion);
                    }
                }
                else
                {
                    MessageBox.Show("No hay existencia de productos de: "  + miProducto.Descripcion);
                }

            }
        }

        private void DescuentoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(DescuentoTextBox.Text))
            {
                descuento = Convert.ToDecimal(DescuentoTextBox.Text);
                totalAPagar = totalAPagar - descuento;
                TotalTextBox.Text = totalAPagar.ToString();

            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                string linea = "--------------------------------------------------------------------------------------------------------------------------------------------";
                int ydetalles = 250;
                Bitmap bitmap = Properties.Resources.encabezado;
                Image image = bitmap;
                e.Graphics.DrawImage(image, 10, 10);

                e.Graphics.DrawString("Cliente: ", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(10, 200));
                e.Graphics.DrawString(miCliente.Nombre, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(80, 200));

                e.Graphics.DrawString("Fecha: ", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(550, 200));
                e.Graphics.DrawString(FechaDateTimePicker.Value.ToString(), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(610, 200));

                e.Graphics.DrawString(linea, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(10, 230));

                e.Graphics.DrawString("Producto", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(10, ydetalles));
                e.Graphics.DrawString("Cantidad", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(420, ydetalles));
                e.Graphics.DrawString("Precio", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(550, ydetalles));
                e.Graphics.DrawString("Total", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(700, ydetalles));

                foreach (DetalleTicket item in listaDetalles)
                {
                    ydetalles = ydetalles + 25;
                    e.Graphics.DrawString(item.Descripcion, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(10, ydetalles));
                    e.Graphics.DrawString(item.Cantidad.ToString(), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(420, ydetalles));
                    e.Graphics.DrawString(item.Precio.ToString("N2"), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(550, ydetalles));
                    e.Graphics.DrawString(item.Total.ToString("N2"), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(700, ydetalles));
                }
                e.Graphics.DrawString(linea, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(10, ydetalles + 20));

                e.Graphics.DrawString("Sub Total: ", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(600, ydetalles + 50));
                e.Graphics.DrawString(subTotal.ToString("N2"), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(700, ydetalles + 50));
                e.Graphics.DrawString("ISV: ", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(653, ydetalles + 75));
                e.Graphics.DrawString(isv.ToString("N2"), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(700, ydetalles + 75));
                e.Graphics.DrawString("Descuento: ", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(591, ydetalles + 100));
                e.Graphics.DrawString(descuento.ToString("N2"), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(700, ydetalles + 100));
                e.Graphics.DrawString("Total: ", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(640, ydetalles + 125));
                e.Graphics.DrawString(totalAPagar.ToString("N2"), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(700, ydetalles + 125));



            }
            catch (Exception)
            {
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
