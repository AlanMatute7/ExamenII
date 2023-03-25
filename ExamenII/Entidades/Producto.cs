namespace Entidades
{
    public class Producto
    {
        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public string Marca { get; set; }
        public decimal Precio { get; set; }
        public byte[] Foto { get; set; }
        public string Modelo { get; set; }

        public Producto()
        {
        }

        public Producto(string codigo, string tipo, string marca, decimal precio, byte[] foto, string modelo)
        {
            Codigo=codigo;
            Tipo=tipo;
            Marca=marca;
            Precio=precio;
            Foto=foto;
            Modelo=modelo;
        }
    }
}
