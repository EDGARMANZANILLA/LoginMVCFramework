

let modalCambiarPassword;
document.addEventListener("DOMContentLoaded", () => {

    let EsUsuarioNuevo = document.getElementById("EsUsuarioNuevoElementoDom").innerHTML;


    if (EsUsuarioNuevo == "True") {
        modalCambiarPassword = new bootstrap.Modal(document.getElementById('ResetDesdeAdmin'));
        modalCambiarPassword.show();

    }

});

/** Axios Interceptor */
axios.interceptors.response.use(response => {
    return response;
}, error => {
    if (error.response.status === 401) {
        window.location.href = "/Inicio/Login"
    }
    return error;
});


// $(document).ready(function () {
// $('#sidebar').toggleClass('active');

$('#sidebarCollapse').on('click', function () {
    $('#sidebar').toggleClass('active');
    /* $('footer').css(background: #000000);*/

});


//  });



async function Redireccionar(Url) {

    MensajeCargando();
    let vistaHTML = document.getElementById("RenderBody");
    // console.log(vistaHTML);


    const response = await axios.post(Url)
    //console.log(response);
    vistaHTML.innerHTML = response.data;
    OcultarMensajeCargando();
}


function ValidarUrl(idLlaveCatAppServidor, idTblUsuario) {
    // console.log(id);

    MensajeCargando();

    axios.post('/SesionUsuario/CrearUrl', {

        idCatAppServidor: idLlaveCatAppServidor,
        idTblUsuario: idTblUsuario

    }).then(function (response) {

        console.log(response);
    })


    //axios.post('/SesionUsuario/CrearUrl',
    //    {
    //        idCatAppServidor: idLlaveCatAppServidor,
    //        idTblUsuario: idTblUsuario

    //}).then(function (response) {

    //    if (response.data.RespuestaServidor.StatusCode === 200) {

    //        console.log(response.data.RespuestaServidor.StatusDescription);

    //        MensajeCorrecto_sinClickSweet(response.data.RespuestaServidor.StatusDescription);
    //    } else {

    //        MensajeErrorSweet("", response.data.RespuestaServidor.StatusDescription);
    //    }

    //}).catch(function (error) {
    //    MensajeErrorSweet("Intente mas tarde", response.data.RespuestaServidor.StatusDescription);

    //});
    OcultarMensajeCargando();

}


/***************************************************************************************************************/
/***************************    SCRIPTS DE LAS VISTAS PARCIALES     ********************************************/
/***************************************************************************************************************/
/* Agregar un usuario al loggin centralizado */
function AgregarUsuario() {
    let numEmpleadoSeleccionado = document.getElementById("buscadorEmpleado").value;

    if (numEmpleadoSeleccionado) {

        MensajeCargando();
        axios.post('/UsuarioRoot/RegistrarUsuarioEnAlphaWeb',
            {
                numEmpleado: numEmpleadoSeleccionado

            }).then(function (response) {

                console.log(response.data);
                if (response.data != false) {
                    MensajeCorrecto_sinClickSweet("Usuario agregado con exito");
                } else {
                    MensajeErrorSweet("Al parecer este empleado ya se encuentra registrado");
                }

            }).catch(function (error) {
                MensajeErrorSweet('Intentelo de nuevo mas tarde o contacte al desarrollador', error);
            });


        OcultarMensajeCargando();
    }
    else {
        MensajeErrorSweet('', 'El campo del numero de empleado esta vacio');
    }

}


/* Vincular usuario con rol en una app */
function VincularUsuarioAPP() {
    console.log("Vinculando USUARIO");

    let numEmpleado = document.getElementById("buscadorEmpleado").value
    let idAppWeb = document.getElementById("SeleccionarAppWeb").value
    let idRol = document.getElementById("SeleccionarRol").value


    if (numEmpleado != '' && idAppWeb != '' && idRol != '') {
        let dataAEnviar = {
            NumEmpleado: numEmpleado,
            IdAppWeb: idAppWeb,
            IdRol: idRol
        };


        MensajeCargando();
        axios.post('/UsuarioRoot/VincularUsuarioConAplicativoWEB',
            {
                nuevoVinculo: dataAEnviar

            }).then(function (response) {

                if (response.data.RespuestaServidor.StatusCode === 200) {
                    MensajeCorrecto_sinClickSweet(response.data.RespuestaServidor.StatusDescription);
                } else {
                    MensajeErrorSweet("", response.data.RespuestaServidor.StatusDescription);
                }

            }).catch(function (error) {
                MensajeErrorSweet('Intentelo de nuevo mas tarde o contacte al desarrollador', error);
            });


        OcultarMensajeCargando();

    }
    else {
        MensajeErrorSweet("Rellene todo los campos dentro de esta pagina", "Error, no todos los campos contienen datos");
    }





    console.log(numEmpleado);
    console.log(idAppWeb);
    console.log(idRol);
}


/* Ver Vinculos por empleado */
function DibujarTablaViculos() {
    $("#TblVinculosEmpleado").append(

        "<table id='TblViculosRolesEmpleado'  class='table table-striped display table-bordered table-hover' cellspacing='0'  style='width:100%'>" +
        " <caption class='text-uppercase text-center'>Vinculos de roles con aplicaciones del usuario</caption>" +
        "<thead class='tabla'>" +

        "<tr class='text-center text-uppercase'>" +

        "<th>Id </th>" +
        "<th>Num. Empleado </th>" +
        "<th>Usuario </th>" +
        "<th>Aplicacion </th>" +
        "<th>Direccion</th>" +
        "<th>Rol</th>" +
        "<th>V. booleano </th>" +
        "<th>Activo</th>" +
        "<th></th>" +

        "</tr>" +
        "</thead>" +
        "</table>"
    );
};

let tablaVinculos;
function RellenarTablaVinculos(datos) {

    tablaVinculos = $('#TblViculosRolesEmpleado').DataTable({
        "stateSave": false,
        "ordering": true,
        "info": true,
        "searching": true,
        "paging": true,
        "lengthMenu": [12, 30],
        "language":
        {
            "processing": "Procesando...",
            "lengthMenu": "Mostrar _MENU_ registros",
            "zeroRecords": "No se encontraron resultados",
            "emptyTable": "Ningún dato disponible en esta tabla",
            "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "infoFiltered": "(filtrado de un total de _MAX_ registros)",
            "search": "Buscar:",
            "info": "Mostrando de _START_ a _END_ de _TOTAL_ entradas",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "data": datos,
        "columns": [
            { "data": "Id" },
            { "data": "NumEmpleado" },
            { "data": "Usuario" },
            { "data": "NombreApp" },
            { "data": "Direccion" },
            { "data": "Rol" },
            { "data": "Activo" },
            {
                render: function (data, type, row) {

                    if (row.Activo) {
                        return '<h4 class="text-uppercase"> <i Id="RolFila' + row.Id + '" class="text-success fas fa-check"></i>  </h4> ';
                    } else {
                        return '<h4 class="text-uppercase"> <i Id="RolFila' + row.Id + '" class="text-danger fas fa-times-circle"></i>  </h4>';
                    }
                }


            },
            {
                render: function (data, type, row) {

                    if (row.Activo) {
                        return '<button id="BotonPrimario" class="btn" onclick="EditarFila(' + row.Id + ' , ' + row.IdReal + ' )" > Editar </button>';
                    } else {
                        return '<button id="BotonPrimario" class="btn" onclick="EditarFila(' + row.Id + ' , ' + row.IdReal + ' )" > Editar </button>';
                        // return '<button id="BotonPrimario" class=" btn " onclick="ActivarRol('+row.IdReal+', '+row.Id+' )">Activar Rol</button>';
                    }
                }


            }

        ],
        "columnDefs": [


            { className: "text-center col-1", visible: true, "targets": 0, },
            { className: "text-center col-1", visible: true, "targets": 1, },
            { className: "text-center col-2", visible: true, "targets": 2, },
            { className: "text-center col-2", visible: true, "targets": 3, },
            { className: "text-center col-2", visible: true, "targets": 4, },
            { className: "text-center col-1", visible: true, "targets": 5, },
            { className: "text-center col-1", visible: true, "targets": 6, },
            { className: "text-center col-1", visible: true, "targets": 7, },
            { className: "text-center col-1", visible: true, "targets": 8, }

        ],

        "order": [[0, 'asc']]
    });
};


function ObtenerVinculosEmpleado() {
    let numEmpleado = document.getElementById("buscadorEmpleadoVinculo").value;

    if (numEmpleado != "") {
        MensajeCargando();

        axios.post('/UsuarioRoot/ObtenerVinculosUsuario', {
            numEmpleado: numEmpleado
        })
            .then(function (response) {

                //console.log(response);


                if (response.data.RespuestaServidor.StatusCode === 200) {
                    //MensajeCorrecto_sinClickSweet(response.data.RespuestaServidor.StatusDescription);
                    MensajeCorrectoTimerEsquinaSuperiorDerecha("Cargando Correctamente");
                    $("#TblVinculosEmpleado").empty();
                    DibujarTablaViculos();
                    RellenarTablaVinculos(response.data.Roles);

                } else {
                    MensajeErrorSweet("", response.data.RespuestaServidor.StatusDescription);
                }






            })
            .catch(function (error) {
                console.log(error);
                MensajeErrorSweet("", error);
            });

        OcultarMensajeCargando();
    }
    else {
        MensajeErrorSweet("", 'Ingrese un numero de empleado valido');
    }

}

let modal;
function EditarFila(NumeroFilaTabla, IdRegistroRolDesactivar) {

    let seleccionFila = tablaVinculos.rows(NumeroFilaTabla - 1).data();

    /*
    console.log(seleccionFila);
    console.log(seleccionFila[0].Usuario);
    console.log(seleccionFila[0].Rol);
    console.log(seleccionFila[0].NombreApp);
    */



    //Cambiar el titulo del modal
    let nombreModal = seleccionFila[0].Usuario + " || " + seleccionFila[0].NombreApp + " || rol : " + seleccionFila[0].Rol;
    document.getElementById("TituloModal").innerText = nombreModal;

    //Cambiar el selec de acuerdo a si el rol esta activo = 1 y desactivo = 0
    let estaActivoRol = seleccionFila[0].Activo;
    console.log(estaActivoRol);
    let estadoRol = estaActivoRol == true ? 1 : 0;
    document.getElementById("SelectCambioDeRol").value = estadoRol;
    //agregar el boton para guardar el cambio de estado del rol
    document.getElementById("GuardarCambiosRol").setAttribute('onclick', 'ActivaDesactivaRol(' + NumeroFilaTabla + ' , ' + IdRegistroRolDesactivar + ', ' + estadoRol + ');');
    //Cargar y mostrar el modal
    modal = new bootstrap.Modal(document.getElementById('EditarRol'));
    modal.show();
}


function ActivaDesactivaRol(NumeroFilaTabla, IdRegistroRolDesactivar, estadoRol) {

    let rolACambiar = document.getElementById("SelectCambioDeRol").value

    if (rolACambiar != estadoRol) {

        rolACambiar == 0 /*Desactivar*/ ? DesactivarRol(IdRegistroRolDesactivar, NumeroFilaTabla) /*Desactivar*/ : ActivarRol(IdRegistroRolDesactivar, NumeroFilaTabla)  /*Activar */;

    } else {
        let banderaRol = estadoRol == 0 ? "Desactivado" : "Activado";
        MensajeErrorSweet('No puede cambiar el estado del rol porque selecciono el mismo que ya tenia : ' + banderaRol + '', 'Error, Intente cambiar el estado del rol');
    }



    //console.log("rol", rolACambiar);
    //console.log("Fila" ,  NumeroFilaTabla);
    //console.log("Registroen DB" ,IdRegistroRolDesactivar);

}




function DesactivarRol(IdRegistroRolDesactivar, NumeroFilaTabla) {
    let dataTableVinculosRoles = $('#TblViculosRolesEmpleado').DataTable();
    //Se captura la pagina donde se encuentra actualmente
    let pagina_actual = dataTableVinculosRoles.page();
    //Guardamos la página actual en el local storage
    localStorage.setItem("pagina_actual", pagina_actual);


    MensajeCargando();
    axios.post('/UsuarioRoot/DesactivarRol', {
        idRelacionesRolAplicaciones: IdRegistroRolDesactivar
    }).then(function (response) {

        MensajeCorrectoTimerEsquinaSuperiorDerecha("Se desactivo el rol con exito!");

        //si todo fue bien capturamos la página guardada anteriormente
        let paginaGuardada = parseInt(localStorage.getItem("pagina_actual"));

        // Pregutnamos si existe el item
        if (paginaGuardada != undefined) {

            let numeroFilaCorrecto = NumeroFilaTabla - 1

            // para una fila de datatable es -1 porque se inicia a contar desde 0
            let row = dataTableVinculosRoles.row(numeroFilaCorrecto);
            $('#TblViculosRolesEmpleado').DataTable().cell(row, 6).data(false).page(paginaGuardada).draw(false);


            //para una fila en espacifico no aplica -1 porque es la fila real y esa se empieza a contar desde el 1
            let IdCampoCheckFilaRolActivoInnactivo = "RolFila" + NumeroFilaTabla;
            let Eliminarclases = document.getElementById("" + IdCampoCheckFilaRolActivoInnactivo + "");

            //Eliminar el check y agregar el icono de la x
            Eliminarclases.classList.remove("text-success");
            Eliminarclases.classList.remove("fa-check");
            Eliminarclases.classList.add("text-danger");
            Eliminarclases.classList.add("fa-times-circle");


            //Cerrar el modal
            modal.hide();


            //Eliminamos el item para que no genere conflicto a la hora de dar click en otro botón detalle
            localStorage.removeItem("pagina_actual");
        }


    }).catch(function (error) {
        MensajeErrorSweet("No se pudo desactivar el rol, intente de nuevo", error);
    });
    OcultarMensajeCargando();
}



function ActivarRol(IdRegistroRolDesactivar, NumeroFilaTabla) {
    let dataTableVinculosRoles = $('#TblViculosRolesEmpleado').DataTable();
    //Se captura la pagina donde se encuentra actualmente
    let pagina_actual = dataTableVinculosRoles.page();
    //Guardamos la página actual en el local storage
    localStorage.setItem("pagina_actual", pagina_actual);

    MensajeCargando();
    axios.post('/UsuarioRoot/ActivarRol', {
        idRelacionesRolAplicaciones: IdRegistroRolDesactivar
    }).then(function (response) {

        MensajeCorrectoTimerEsquinaSuperiorDerecha("Se activo el rol con exito!");

        //si todo fue bien capturamos la página guardada anteriormente
        let paginaGuardada = parseInt(localStorage.getItem("pagina_actual"));

        // Pregutnamos si existe el item
        if (paginaGuardada != undefined) {

            let row = dataTableVinculosRoles.row(NumeroFilaTabla - 1);
            $('#TblViculosRolesEmpleado').DataTable().cell(row, 6).data(true).page(paginaGuardada).draw(false);


            //para una fila en espacifico no aplica -1 porque es la fila real y esa se empieza a contar desde el 1
            let IdCampoCheckFilaRolActivoInnactivo = "RolFila" + NumeroFilaTabla;
            let Eliminarclases = document.getElementById("" + IdCampoCheckFilaRolActivoInnactivo + "");

            //Eliminar el check y agregar el icono de la x
            Eliminarclases.classList.remove("text-danger");
            Eliminarclases.classList.remove("fa-times-circle");
            Eliminarclases.classList.add("text-success");
            Eliminarclases.classList.add("fa-check");


            //Cerrar el modal
            modal.hide();

            //Eliminamos el item para que no genere conflicto a la hora de dar click en otro botón detalle
            localStorage.removeItem("pagina_actual");
        }

    }).catch(function (error) {
        MensajeErrorSweet("No se pudo desactivar el rol, intente de nuevo", error);
    });
    OcultarMensajeCargando();

}




/***************************************************************************************************************/
/***************************    scrip vista parcial ResetearContraseniaUsuario    ********************************************/
/***************************************************************************************************************/
function ResetearPassword() {
    let numEmpleadoResetearPass = document.getElementById("buscadorEmpleadoReset").value
    console.log(numEmpleadoResetearPass);

    if (numEmpleadoResetearPass != "") {
        Swal.fire({
            title: '¿Esta seguro que desea resetear la contraseña para el usuario ' + numEmpleadoResetearPass + '?',
            text: 'Continue solo si esta seguro de querer hacerlo!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, Continuar!',
            cancelButtonText: 'No, cancelar!',
            footer: '<a href="#">Contactar al desarrollador?</a>'
        }).then((result) => {
            if (result.isConfirmed) {

                MensajeCargando();
                axios.post('/UsuarioRoot/ResetearPassword', {
                    numEmpleado: numEmpleadoResetearPass
                }).then(function (response) {



                    if (response.data.RespuestaServidor.StatusCode === 200) {
                        MensajeCorrectoSweet("La contraseña se cambio a : " + response.data.RespuestaServidor.StatusDescription);
                    }
                    else {
                        MensajeErrorSweet("", response.data.RespuestaServidor.StatusDescription);
                    }



                }).catch(function (error) {
                    MensajeErrorSweet("Intentelo de nuevo mas tarde", error);
                });
                OcultarMensajeCargando();

            }
        })

    } else {
        MensajeErrorSweet('llene el campo "Usuario"', 'El usuario no puede estar vacio');
    }



}



function cambiarPassword(idCuentaUsuarioCambiaPass) {
    let nuevopass = document.getElementById("nuevoPass").value;
    let repetidopass = document.getElementById("repitenuevoPass").value;



    let espacios = false;
    let espacios2 = false;
    let cont = 0;
    let cont2 = 0;

    while (!espacios && (cont < nuevopass.length)) {
        if (nuevopass.charAt(cont) == " ")
            espacios = true;
        cont++;
    }

    while (!espacios2 && (cont < repetidopass.length)) {
        if (repetidopass.charAt(cont) == " ")
            espacios2 = true;
        cont2++;
    }


    if (espacios || espacios2) {
        MensajeErrorSweet("La contraseña no puede contener espacios en blanco")

    } else {
        if (nuevopass === "" || repetidopass === "") {

            MensajeErrorSweet("La contraseña no puede estar vacia")


        } else {
            if (nuevopass.trim() === repetidopass.trim()) {

                let passwordTrimeado = nuevopass.trim();
                let idCuentaUsuarioCambiaPassInteger = parseInt(idCuentaUsuarioCambiaPass);
                MensajeCargando();
                axios.post('/SesionUsuario/CambiarPasswordUsuarioNuevo', {
                    nuevaContrasenia: passwordTrimeado,
                    idCuentaUsuarioCambiaPass: idCuentaUsuarioCambiaPassInteger
                }).then(function (response) {

                    //console.log(response.data)
                    //console.log(response.data.RespuestaServidor)


                    if (response.data.RespuestaServidor.StatusCode === 200) {
                        MensajeCorrectoTimerEsquinaSuperiorDerecha(response.data.RespuestaServidor.StatusDescription);

                        //Cerrar el modal
                        modalCambiarPassword.hide();

                    }
                    else {
                        MensajeErrorSweet("", response.data.RespuestaServidor.StatusDescription);
                    }

                }).catch(function (error) {
                    MensajeErrorSweet("No se pudo desactivar el rol, intente de nuevo", error);
                });
                OcultarMensajeCargando();
            }
            else {
                MensajeErrorSweet("Las contraseñas de los campos tienen que ser identicas")
            }
        }

    }

}



function MostrarPasswordUsuarioNuevo() {
    password1 = document.querySelector('.password1');
    password2 = document.querySelector('.password2');

    if (password1.type === "text") {
        password1.type = "password"
        password2.type = "password"
    } else {
        password1.type = "text"
        password2.type = "text"
    }
}

