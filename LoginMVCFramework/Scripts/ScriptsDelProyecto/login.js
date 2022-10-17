
axios.interceptors.response.use(response => {
    return response;
}, error => {
    if (error.response.status === 401) {
        window.location.href = "/Inicio/Login"
    }
    return error;
});




async function Iniciar( )
{
    

    try {

        let autenticacionUrl = "/Inicio/InicioDeSesion";
        let redireccionarUrl = "/SesionUsuario/InicioApp";

       // console.log(autenticacionUrl);
       // console.log(redireccionarUrl);


        let Usuario = document.getElementById("User").value;
        let Contrasenia = document.getElementById("Password").value;

        if (Usuario != "" && Contrasenia != "") {



            //Si el validador no esta activo arroja un error que se cacha en el catch contenido dentro de esta funcion  
            const responseAPILocal = await axios.get('http://localhost:3000/api/desktop/network-interfaces')


    
                //obterner mac
                let dataAEnviar = {
                    User: Usuario,
                    Password: Contrasenia,
                    DireccionMac: responseAPILocal.data.macCliente,
                    HostNameCliente: responseAPILocal.data.hostnameCliente
                };


            MensajeCargando();
                axios.post(autenticacionUrl, {
                    Autenticacion: dataAEnviar
                })
                .then(function (response) {
                        console.log(response.data.RespuestaServidor);

                        if (response.data.RespuestaServidor.StatusCode === 200) {
                            MensajeCorrectoTimerEsquinaSuperiorDerecha(response.data.RespuestaServidor.StatusDescription);
                            window.location.href = redireccionarUrl;

                        }
                        else {
                            MensajeErrorSweet("", response.data.RespuestaServidor.StatusDescription);
                        }

                })
                .catch(function (error) {
                    console.log(error);
                    MensajeErrorSweet(error);
                });



            
          

        }
        else {
            MensajeErrorSweet("Asegurese de llenar todos los campos", "Algunos campos estan vacios");
        }



    } catch (e) {
        
        //if (e.message == 'Network Error') {
        //    MensajeErrorSweet("Inicie el servidor local en su maquina", "Hubo un problema al autenticar su pc")
        //} else
        //{
        //    MensajeErrorSweet(e.message , "");
        //}

        MensajeErrorSweet("Le recomendamos que tenga instalado el validador", "No pudimos validar la autenticidad de su equipo");

    }
    OcultarMensajeCargando();







}


let modalParaRecuperarPassword;
function AbrirModalSolicitarContraseña()
{
    modalParaRecuperarPassword = new bootstrap.Modal(document.getElementById('RecuperarContrasenia'));
    modalParaRecuperarPassword.show();
}


async function SolicitarCambioPasswordDesdeWeb()
{
    try {
      
        
        let numeroEmpleado = document.getElementById("numeroEmpleadoRecuperarPass").value;
        let nombreUsuario = document.getElementById("usuarioRecuperarPass").value;


        let espacios = false;
        let cont = 0;

        while (!espacios && (cont < nombreUsuario.length)) {
            if (nombreUsuario.charAt(cont) == " ")
                espacios = true;
            cont++;
        }


        if (espacios) {
            MensajeErrorSweet("El usuario no puede contener espacios en blanco")
        } else {
            if (numeroEmpleado === "" || nombreUsuario === "") {
                MensajeErrorSweet("Llene todos los campos para continuar");
            } else {
                let numeroEmpleadoInt = parseInt(numeroEmpleado);

                if (numeroEmpleadoInt <= 0) {
                    MensajeErrorSweet("Ingrese un numero de empleado valido");
                } else {

                    //Si el validador no esta activo arroja un error que se cacha en el catch contenido dentro de esta funcion  
                    const responseAPILocal = await axios.get('http://localhost:3000/api/desktop/network-interfaces')

                    //console.log(response);

                   let dataAEnviar = {
                       nombreUsuario: nombreUsuario,
                       numEmpleado: numeroEmpleadoInt,
                       direccionMac: responseAPILocal.data.macCliente,
                       hostNameCliente: responseAPILocal.data.hostnameCliente,
                       ipCliente: responseAPILocal.data.ipCliente
                   };

                    MensajeCargando();
                    axios.post('/Inicio/SolicitarCambioPassword', {
                        nuevaSolicitud: dataAEnviar
                    })
                        .then(function (response) {
                            //console.log(response.data.RespuestaServidor);

                            if (response.data.RespuestaServidor.StatusCode === 200) {
                                MensajeCorrectoSweet(response.data.RespuestaServidor.StatusDescription);
                                modalParaRecuperarPassword.hide();

                            } else if (response.data.RespuestaServidor.StatusCode === 401) {


                                MensajeErrorSweet("Espere el tiempo indicado si desea solicitar de nuevo su cambio de contraseña", "Hay una solicitud abierta actualmente, las instrucciones se encuentran en su correo electronico")
                            
                               
                                tiempoExpirar(response.data.RespuestaServidor.StatusDescription);
                            }
                            else {
                                MensajeErrorSweet("", response.data.RespuestaServidor.StatusDescription);
                            }

                            document.getElementById("numeroEmpleadoRecuperarPass").value = ""
                            document.getElementById("usuarioRecuperarPass").value = "";

                            
                            //var myModal = new bootstrap.Modal(document.getElementById('RecuperarContrasenia'));
                            //myModal.hide();

                        })
                        .catch(function (error) {
                            console.log(error);
                            MensajeErrorSweet(error);
                        });
                    OcultarMensajeCargando();
                }


            }


        }






    } catch (e)
    {
        MensajeErrorSweet("Le recomendamos que tenga instalado el validador","No pudimos validar la autenticidad de su equipo");
    }

   
}


function tiempoExpirar(cadena) {
    //let cadena = "02:05";
    let minutos = parseInt(cadena.split(":")[0]);
    let segundos = parseInt(cadena.split(":")[1]);

    document.getElementById("BotonPrimarioSolicitarCambioPass").disabled = true;
    document.getElementById("DivTiempoEspera").style = 'background-color: rgb(236, 235, 235); display: block;';
    
    let timer = setInterval(function () {
        document.getElementById("SegundosEspera").innerHTML = "Tiempo de espera para una nueva solicitud : " +   (minutos < 10 ? "0" + minutos : minutos) + ":" +   (segundos < 10 ? "0" + segundos : segundos);

        segundos--;

        if (minutos == 0 && segundos < 0) {
            segundos = 0
            document.getElementById("DivTiempoEspera").style = 'display: none';
            document.getElementById("BotonPrimarioSolicitarCambioPass").disabled = false;
            clearInterval(timer);
        } else if (segundos == 0 && minutos != 0) {
            minutos--;
            segundos = 59;
        }


    }, 1000);
}



async function CambiarPasswordSolicitudExitosa( idCuentaUsuarioCambiaPass , foliSolicitud)
{
    //console.log();
    //console.log(nombreUsuario);
    console.log(foliSolicitud)
    console.log(parseInt( foliSolicitud))

    try
    {
        let nombreUsuarioCreadorSolicitud = document.getElementById("nombreUsuarioSolicitud").innerHTML;
        let nombreUsuarioSaneado = nombreUsuarioCreadorSolicitud.trim().split('"')[1];
        //console.log(nombreUsuarioCreadorSolicitud.trim().split('"')[1]);

        const responseAPILocal = await axios.get('http://localhost:3000/api/desktop/network-interfaces')

        //console.log(responseAPILocal.status);

        if (responseAPILocal.status === 200)
        {

            let Pass = document.getElementById("cambioPassword").value
            let PassRepetido = document.getElementById("repiteCambioPassword").value


            let espaciosEnPass = false;
            let contEnPass = 0;

            let espaciosEnPassRepetido = false;
            let contEnPassRepetido = 0;



            while (!espaciosEnPass && (contEnPass < Pass.length)) {
                if (Pass.charAt(contEnPass) == " ")
                    espaciosEnPass = true;
                contEnPass++;
            }

            while (!espaciosEnPassRepetido && (contEnPassRepetido < PassRepetido.length)) {
                if (PassRepetido.charAt(contEnPassRepetido) == " ")
                    espaciosEnPassRepetido = true;
                contEnPassRepetido++;
            }


            //MensajeCorrectoSweet("inicio Validacion")
            if (espaciosEnPass || espaciosEnPassRepetido) {
                MensajeErrorSweet("La contraseña no puede contener espacios en blanco")

            } else {
                if (Pass === "" || PassRepetido === "") {

                    MensajeErrorSweet("La contraseña no puede estar vacia")

                } else
                {

                    if (Pass === PassRepetido) {


                        let passwordTrimeado = PassRepetido.trim();

                        let dataAEnviar = {
                            idCuentaUsuarioCambiaPass: idCuentaUsuarioCambiaPass,
                            folioSolicitud: parseInt(foliSolicitud),
                            nombreUsuario: nombreUsuarioSaneado,
                            sustituirPassword: passwordTrimeado,
                            direccionMac: responseAPILocal.data.macCliente,
                            hostNameCliente: responseAPILocal.data.hostnameCliente
                        };
                        console.log(dataAEnviar);

                        MensajeCargando();
                        axios.post('/Inicio/SolicitudConfirmadaCambiaPassword', {
                            solicitudAprobada: dataAEnviar
                        })
                            .then(function (response) {
                                //console.log(response.data.RespuestaServidor);

                                if (response.data.RespuestaServidor.StatusCode === 200) {
                                    MensajeCorrectoSweet(response.data.RespuestaServidor.StatusDescription.toUpperCase() + " ESPERE Y SE LE REDIRECCIONARA AL INICIO AUTOMATICAMENTE");
                                    // console.log(response.data.RespuestaServidor.StatusDescription)
                                    RedireccionarInicioLogin();
                                 

                                }
                                else {
                                    MensajeErrorSweet("", response.data.RespuestaServidor.StatusDescription);
                                    RedireccionarInicioLogin();
                                }

                            })
                            .catch(function (error) {
                                console.log(error);
                                MensajeErrorSweet(error);
                            });

                        OcultarMensajeCargando();

                    } else
                    {
                        MensajeErrorSweet("ingrese la misma contraseña en ambos campos",  "Las contraseñas no son identicas" );
                    }
                 
                   



                }
            }

        }
    }
    catch(e)
    {
        //console.log(e)
        MensajeErrorSweet("Le recomendamos que tenga instalado el validador", "No pudimos validar la autenticidad de su equipo");
    }
 



}



function RedireccionarInicioLogin()
{
    setTimeout(() => {
        // console.log("3 SEGUNDO DE ESPERA ")
        window.location = "/Inicio/Login";
    }, 5000);

}


//Mostrar password para la solicitud de cambio de contraseña
function MostrarPassword() {
    // elementos input de tipo password
    password1 = document.querySelector('.password1');
    password2 = document.querySelector('.password2');

    if (password1.type === "text") {
        password1.type = "password"
        password2.type = "password"
        // showPassword.classList.remove('fa-eye-slash');
    } else {
        password1.type = "text"
        password2.type = "text"
        // showPassword.classList.toggle("fa-eye-slash");
    }

}




function MostrarPasswordIniarSession() {
    passwordInicial = document.querySelector('.passwordInicial');
    if (passwordInicial.type === "text") {
        passwordInicial.type = "password"
    } else {
        passwordInicial.type = "text"
    }
}



