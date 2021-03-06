using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using cliente.Partida;

namespace cliente.Menu
{
    public partial class FormMenuPrincipal : Form
    {
        Socket conn;
        Thread atender;

        List<TabMenu> tabs = new List<TabMenu>();
        Dictionary<int, FormPartida> partidas = new Dictionary<int, FormPartida>();

        int idJ;
        string nombre;

        delegate void DelegadoRespuestas(string res);

        public FormMenuPrincipal()
        {
            InitializeComponent();
        }

        private void FormMenuPrincipal_Load(object sender, EventArgs e)
        {
            // Conectar al servidor
            IPAddress addrServer = IPAddress.Parse("147.83.117.22");
            IPEndPoint ipep = new IPEndPoint(addrServer, 50074);

            conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                conn.Connect(ipep);
            }
            catch (SocketException exc)
            {
                MessageBox.Show("No se ha podido conectar con el servidor.", "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.conn = null;
                this.Close();
            }

            // Preparar pantallas
            tabs.Add(new TabLogin(this.conn));
            tabs.Add(new TabRegistro(this.conn));
            tabs.Add(new TabMenuPrincipal(this.conn));
            tabs.Add(new TabInfoPartida(this.conn));

            foreach (TabMenu tab in this.tabs)
            {
                tab.Hide();
                tab.Location = new Point(0, 0);
                this.Controls.Add(tab);
            }
            tabs[0].VisibleChanged += this.tabLogin_VisibleChanged;
            tabs[1].VisibleChanged += this.tabRegistro_VisibleChanged;
            tabs[2].VisibleChanged += this.tabMenuPrincipal_VisibleChanged;
            tabs[3].VisibleChanged += this.tabInfoPartidas_VisibleChanged;

            tabs[0].Show();
            ThreadStart ts = delegate { AtenderServidor(); };
            atender = new Thread(ts);
            atender.Start();
        }

        private void FormMenuPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn != null)
            {
                // Desconectar servidor
                atender.Interrupt();
                string pet = "0/";
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);
                conn.Shutdown(SocketShutdown.Both);
                conn.Close();
            }
        }

        private void AtenderServidor()
        {
            ThreadStart ts;
            Thread thread;
            int idP;
            while (true)
            {
                DelegadoRespuestas delegado;
                
                byte[] res_b = new byte[512];
                conn.Receive(res_b);
                if (res_b[0] == 0)
                    continue;
                
                string entrada = Encoding.ASCII.GetString(res_b).Split('\0')[0];
                string[] trozos = entrada.Split("~~END~~");

                for (int i = 0; i < trozos.Length; i++)
                {
                    if (trozos[i] == "")
                        continue;
                    string[] res = trozos[i].Split('/', 2);
                    
                    int codigo = Convert.ToInt32(res[0]);
                    string mensaje = res[1];

                    switch (codigo)
                    {
                        case 1:
                            // Registrarse
                            delegado = new DelegadoRespuestas(((TabRegistro)tabs[1]).ActualizarRegistro);
                            tabs[1].Invoke(delegado, new object[] { mensaje });
                            break;
                        case 2:
                            // Iniciar sesión
                            delegado = new DelegadoRespuestas(((TabLogin)tabs[0]).ActualizarLogin);
                            tabs[0].Invoke(delegado, new object[] { mensaje });
                            break;
                        case 3:
                            // Mostrar partidas jugadas
                            delegado = new DelegadoRespuestas(((TabMenuPrincipal)tabs[2]).ActualizarDataGrid);
                            tabs[2].Invoke(delegado, new object[] { mensaje });
                            break;
                        case 4:
                            // Información de una partida
                            delegado = new DelegadoRespuestas(((TabInfoPartida)tabs[3]).MostrarInfoPartida);
                            tabs[3].Invoke(delegado, new object[] { mensaje });
                            break;
                        case 5:
                            // Media de puntos del jugador
                            delegado = new DelegadoRespuestas(((TabMenuPrincipal)tabs[2]).ActualizarMedia);
                            tabs[2].Invoke(delegado, new object[] { mensaje });
                            break;
                        case 6:
                            // Lista de conectados
                            delegado = new DelegadoRespuestas(((TabMenuPrincipal)tabs[2]).ActualizarListaConectados);
                            tabs[2].Invoke(delegado, new object[] { mensaje });
                            foreach (FormPartida form in partidas.Values)
                            {
                                // El host la tiene que ver en el lobby para poder invitar
                                if (form.host)
                                {
                                    delegado = new DelegadoRespuestas(form.ActualizarListaConectados);
                                    form.Invoke(delegado, new object[] { mensaje });
                                }
                            }
                            break;
                        case 7:
                            // Crear un lobby
                            idP = Convert.ToInt32(mensaje);
                            ts = delegate { AbrirPartidaHost(idP); };
                            thread = new Thread(ts);
                            thread.Start();
                            break;
                        case 8:
                            // Respuesta a la invitación de unirse al lobby
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].RespuestaInvitacion);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 9:
                            // Invitación para unirse lobby
                            ts = delegate { Invitacion(mensaje); };
                            thread = new Thread(ts);
                            thread.Start();
                            break;
                        case 10:
                            // Abandonar lobby y/o partida
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].PartidaCancelada);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                                partidas.Remove(idP);
                            }
                            break;
                        case 11:
                            // Lista de jugadores en el lobby
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].AtenderListaJugadores);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 13:
                            // Recibes un mensaje por el chat
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].ActualizarChat);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 14:
                            // Empezar la partida (orden de las fichas del tablero)
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].PartidaEmpezada);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 15:
                            // Cambio de turno
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].PartidaTurno);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 16:
                            // Resultado dados
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].TirarDados);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 17:
                            // Nueva posición del ladrón
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].Colocar);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 18:
                            // Posición de poblado colocado
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].Colocar);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 19:
                            // Posición ciudad colocada
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].Colocar);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 20:
                            // Posición carretera colocada
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].Colocar);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 21:
                            // Jugador del turno compra carta
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].ComprarCarta);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 22:
                            // Jugador del turno usa carta caballero
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].UsarCarta);                                
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 23:
                            // Jugador del turno usa carta carretera
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].UsarCarta);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 24:
                            // Jugador del turno usa carta invento
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].UsarCarta);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 25:
                            // Jugador del turno usa carta monopolio
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].UsarCarta);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 26:
                            // Jugador entrega recursos por monopolio
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].DarMonopolio);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 27:
                            // Llega una oferta de comercio
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].ComercioOferta);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 28:
                            // Respuesta de comercio (aceptar o no)
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].ComercioRespuesta);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 29:
                            // Resultado de un comercio
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].ComercioResultado);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 30:
                            // Resultado de un comercio marítimo o con la banca
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].ComercioMaritimo);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 31:
                            // Se indica que un jugador ha entregado al ladrón
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].DarLadron);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 32:
                            // Se indica a quien se elige robar al mover ladrón
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].DarLadron);
                                partidas[idP].Invoke(delegado, new object[] { trozos[i] });
                            }
                            break;
                        case 33:
                            // Id partida en la BBDD trás finalizar la partida
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].PartidaGanada);
                                partidas[idP].Invoke(delegado, new object[] { mensaje });
                            }
                            break;
                        case 34:
                            // Nombre del jugador y puntos obtenidos al finalizar la partida
                            idP = Convert.ToInt32(mensaje.Split("/")[0]);
                            if (partidas.ContainsKey(idP))
                            {
                                delegado = new DelegadoRespuestas(partidas[idP].Participacion);
                                partidas[idP].Invoke(delegado, new object[] { mensaje.Split("/")[1] });
                            }
                            break;
                        case 35:
                            // Se indica si el jugador ha sido dado de baja correctamente
                            delegado = new DelegadoRespuestas(((TabLogin)tabs[0]).DarBaja);
                            tabs[0].Invoke(delegado, new object[] { mensaje });
                            break;
                    }
                }
            }
        }

        private void tabLogin_VisibleChanged(object sender, EventArgs e)
        {
            // Solo nos importa cuando se oculta
            if (!tabs[0].Visible)
            {
                // Miramos el tag
                switch (tabs[0].Tag)
                {
                    case "REGISTRO":
                        // Se muestra el tab registro
                        tabs[1].Show();
                        break;
                    case "LOGIN":
                        // Se abre el menú principal
                        this.idJ = ((TabLogin)tabs[0]).idJ;
                        this.nombre = ((TabLogin)tabs[0]).nombre;
                        ((TabMenuPrincipal)tabs[2]).idJ = this.idJ;
                        tabs[2].Show();
                        break;
                }
            }
        }

        private void tabRegistro_VisibleChanged(object sender, EventArgs e)
        {
            // Solo nos importa cuando se oculta
            if (!tabs[1].Visible)
            {
                // Miramos el tag
                switch (tabs[1].Tag)
                {   
                    // Si el registro es positivo o se ha dado a ir atrás
                    // Se muestra el tab de login
                    case "REGISTRO SI":
                    case "ATRAS":
                        tabs[0].Show();
                        break;
                }
            }
        }
        
        private void tabMenuPrincipal_VisibleChanged(object sender, EventArgs e)
        {
            // Solo nos importa cuando se oculta 
            if (!tabs[2].Visible)
            {
                // Miramos del estado del que partimos
                switch (tabs[2].Tag)
                {
                    case "DESCONECTAR":
                        // Si desconexión directamente cerramos el form
                        this.Close();
                        break;
                    case "INFO PARTIDA":
                        // Si es info partida abrimos el tab de participación
                        tabs[3].Show();
                        break;
                }
            }
        }

        private void tabInfoPartidas_VisibleChanged(object sender, EventArgs e)
        {
            // Solo nos importa cuando se oculta
            if (!tabs[3].Visible)
            {
                // Sino se ve el de participación se debe mostrar el de menú principal
                tabs[2].Show();
            }
        }

        /// <summary>
        /// Abre form de partida 
        /// </summary>
        /// <param name="idP"> identificador de la partida </param>
        private void AbrirPartidaHost(int idP)
        {
            FormPartida form = new FormPartida(this.conn, idP, this.nombre, true);
            partidas.Add(idP, form);
            form.FormClosed += EliminarEntradaForm;
            form.ShowDialog();
        }

        /// <summary>
        /// Abre un fomr indicando que has sido invitado a una partida
        /// </summary>
        /// <param name="mensaje"> String de la forma: idPartida/host </param>
        private void Invitacion(string mensaje)
        {
            string[] trozos = mensaje.Split("/");
            string pet;
            int idP = Convert.ToInt32(trozos[0]);
            FormEscoger form = new FormEscoger(
                "Has sido invitado por " + trozos[1],
                "Aceptar", "Rechazar"
            );
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.Yes)
            {
                // Aceptar
                FormPartida formP = new FormPartida(this.conn, idP, this.nombre, false);
                partidas.Add(idP, formP);
                formP.FormClosed += EliminarEntradaForm;
                formP.ShowDialog();
            } else
            {
                // Rechazar
                pet = "9/" + trozos[0] + "/NO";
                byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
                conn.Send(pet_b);
            }
        }

        /// <summary>
        /// Cierra el form y sale de la partida 
        /// </summary>
        private void EliminarEntradaForm(object sender, EventArgs e)
        {
            FormPartida form = (FormPartida)sender;
            string pet = "10/" + form.idP.ToString();
            byte[] pet_b = System.Text.Encoding.ASCII.GetBytes(pet);
            conn.Send(pet_b);
            partidas.Remove(form.idP);
        }
    }
}
