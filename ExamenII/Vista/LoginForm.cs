using Datos;
using Entidades;
using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Login_Activated(object sender, EventArgs e)
        {
            CodigoUsuarioTextBox.Focus();
        }

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AceptarButton_Click(object sender, EventArgs e)
        {
            if (CodigoUsuarioTextBox.Text == string.Empty)
            {
                errorProvider1.SetError(CodigoUsuarioTextBox, "Ingrese un usuario");
                return;
            }
            errorProvider1.Clear();
            if (ContraseñaTextBox.Text == "")
            {
                errorProvider1.SetError(ContraseñaTextBox, "Ingrese una contraseña");
                return;
            }
            errorProvider1.Clear();

            //Validar usuario en la base de datos
            Login login = new Login(CodigoUsuarioTextBox.Text, ContraseñaTextBox.Text);

            UsuarioDB usuarioDB = new UsuarioDB();
            Usuario usuario = new Usuario();

            usuario = usuarioDB.Autenticar(login);

            if (usuario != null)
            {
                if (usuario.EstaActivo)
                {
                    //Crea la Sesión
                    System.Security.Principal.GenericIdentity identidad = new System.Security.Principal.GenericIdentity(usuario.CodigoUsuario);
                    System.Security.Principal.GenericPrincipal principal = new System.Security.Principal.GenericPrincipal(identidad, new string[] { usuario.Rol });
                    System.Threading.Thread.CurrentPrincipal = principal;

                    //Mandar al menu
                    Menu menuFormulario = new Menu();
                    this.Hide();
                    menuFormulario.Show();
                }
                else
                {
                    MessageBox.Show("El usuario esta inactivo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Datos de usuario incorrectos");
            }





        }

        private void MostrarButton_Click(object sender, EventArgs e)
        {
            if (ContraseñaTextBox.PasswordChar == '*')
            {
                ContraseñaTextBox.PasswordChar = '\0';
            }
            else
            {
                ContraseñaTextBox.PasswordChar = '*';
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
