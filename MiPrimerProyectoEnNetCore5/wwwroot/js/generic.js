var editarboton = true;
var borrarboton = true;


function AbrirModalGenerica(titulo = "¿Desea guardar los cambios?",
    texto = "Desea los cambios en la base de datos") {
    return Swal.fire({
        title: titulo,
        text: texto,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si'
    });
}

function Correcto(title ="'Se eliminó correctamente") {
    Swal.fire({
        position: 'top-end',
        icon: 'success',
        title: title,
        showConfirmButton: false,
        timer: 1500
    });
}

function Incorrecto(title = "'Ocurrió un error") {
    Swal.fire({
        icon: 'error',
        title: title
    });
}



function Listar(controller, popup=false, popupCita=false) {
    var table = $('#myTable').DataTable();
    table.destroy();
    $.get(controller + '/Listar',
        function (data) {
            Pintar(data, controller, 'myTable', campos, campoID, popup, true, "cuerpoTabla", "", popupCita);
        });
}

function Pintar(data, controller, table = "myTable", campos, campoID, popup = false, opciones = true, id ="cuerpoTabla", propiedadMostrar="", popupCita = false) {
    $("#"+id).html("");
    var cadena = "";
    var valor = "";
    $.each(data, function (idx, opt) {

        if (popupCita) {
            if (data[idx]['idEstadoCita'] == '2') {
                cadena += "<tr style='color:green;font-weight:bold'>";
            }
            else if (data[idx]['idEstadoCita'] == '3') {
                cadena += "<tr style='color:red;font-weight:bold'>";
            }
            else if (data[idx]['idEstadoCita'] == '5') {
                cadena += "<tr style='color:#00aae4;font-weight:bold'>";
            }
            else if (data[idx]['idEstadoCita'] == '6') {
                cadena += "<tr style='color:tomato;font-weight:bold'>";
            }
            else if (data[idx]['idEstadoCita'] == '7') {
                    cadena += "<tr style='color:purple;font-weight:bold'>";
            }
            else {
                cadena += "<tr>";
            }
        }
        else {
            cadena += "<tr>";
        }

        for (var i = 0; i < campos.length; i++) {     
            valor = data[idx][campos[i]];
            cadena += "<td>" + ((valor!=null)?valor:"") + "</td>";

        }

        if (popupCita) {
            if (opciones) {
                var idVista = $('#txtIdVistaOculto').val();

                cadena += `<td>`;
                cadena += `      <i class="fas fa-eye btn btn-secondary" style="cursor:pointer" data-toggle="modal" 
                                data-target="#modalHistorico" onclick="MostrarPopUpDinamico('Historial','Cita',${data[idx][campoID]})"></i>`;

                //Editar Cita
                if (data[idx]['idEstadoCita'] == '1' || data[idx]['idEstadoCita'] == '6') {
                    cadena += `<i class="fas fa-edit btn btn-primary" style="cursor:pointer" data-toggle="modal"
                                data-target="#modalHistorico" onclick="MostrarPopUpDinamico('Editar Cita','Cita',${data[idx][campoID]})"></i>`;
                }

                //Observar Cita
                if (data[idx]['idEstadoCita'] == '2' && idVista == '1') {
                    cadena += `<i class="fas fa-share btn btn-primary" style="cursor:pointer" data-toggle="modal"
                                data-target="#modalHistorico" onclick="MostrarPopUpDinamico('Observar','Cita',${data[idx][campoID]})"></i>`;
                }

                //Revisar Doctor
                if (data[idx]['idEstadoCita'] == '2' && idVista == '1') {
                    cadena += `<i  class="fas fa-check btn btn-success" style="cursor:pointer" data-toggle="modal"
                                data-target="#modalHistorico" onclick="MostrarPopUpDinamico('Revisar','Cita',${data[idx][campoID]})"></i>`;
                }

                //Eliminar cita
                if (data[idx]['idEstadoCita'] == '1') {
                    cadena += `<i class="fas fa-trash-alt btn btn-danger" 
                                    style="cursor:pointer" onclick="MostrarConfirmacionEliminar('Cita',${data[idx][campoID]})"></i>`;
                }

                //Enviar Cita
                if (data[idx]['idEstadoCita'] == '1') {
                    cadena += `<i class="fas fa-envelope-square btn btn-primary" 
                    style="cursor:pointer" onclick="MostrarConfirmacion('Cita',${data[idx][campoID]})"></i>`;
                }

                //Justificar Cita
                if (data[idx]['idEstadoCita'] == '6') {
                    cadena += `<i class="fas fa-envelope-square btn btn-success" 
                    style="cursor:pointer" onclick="MostrarConfirmacionJustificar('Cita',${data[idx][campoID]})"></i>`;
                }

                //Anular Cita
                if (data[idx]['idEstadoCita'] == '2' && idVista == '1') {
                    cadena += `<i class="fas fa-times btn btn-danger" style="cursor:pointer" data-toggle="modal"
                                data-target="#modalHistorico" onclick="MostrarPopUpDinamico('Anular Cita','Cita',${data[idx][campoID]})"></i>`;
                }

               

                cadena += `</td>`;
            }
        }
        else {

            if (opciones) {

                cadena += `<td>`;

                if (borrarboton) {
                    cadena += `
                               <i class="fas fa-trash btn btn-danger" onclick="Eliminar('${controller}', ${data[idx][campoID]})" style="cursor:pointer"></i>`;
                }

                if (editarboton) {
                    if (!popup)
                        cadena += `<a href="/${controller}/Editar/${data[idx][campoID]}" class="fas fa-edit btn btn-primary" style="cursor:pointer"></a>`;
                    else
                        cadena += `<i class="fas fa-edit btn btn-primary" onclick="Editar('${controller}',${data[idx][campoID]})" data-toggle="modal" data-target="#exampleModal" style="cursor:pointer"></i>`;
                }
                cadena += `</td>`;
            }
            else {
                cadena += `<td>
                               <i class="fas fa-check btn btn-success"
                                onclick="AsignarPersona('${data[idx][campoID]}', '${data[idx][propiedadMostrar]}')" style="cursor:pointer"></i>
                           </td`;
            }
        }

        cadena += "</tr>";
    });
    $("#" + id).append(cadena);

    $("#"+table).DataTable();
}

function PintarMultiplePopUp(data, controller, divTablaModal, cabeceras = ["Id", "Nombre Completo"], campos, campoID, propiedadMostrar = "", campoaRellenar = controller)
{
    $("#" + divTablaModal).html("");
    var contenido = "";
    contenido += "<table id='tbModalMultiple'>";
    contenido += " <thead class='thead-dark'>";
    for (var i = 0; i < cabeceras.length; i++) {
        valor = cabeceras[i];
        contenido += "<th>" + ((valor != null) ? valor : "") + "</th>";
    }
    contenido += "<th></th>";
    contenido += "<tbody>";
    $.each(data, function (idx, opt) {
        contenido += "<tr>";

        for (var i = 0; i < campos.length; i++) {
            valor = data[idx][campos[i]];
            contenido += "<td>" + ((valor != null) ? valor : "") + "</td>";
        }
        contenido += `<td>
                           <i class="fas fa-check btn btn-success"
                            onclick="AsignarNombre('${campoaRellenar}','${data[idx][campoID]}', '${data[idx][propiedadMostrar]}')" style="cursor:pointer"></i>
                       </td`;
        contenido += "</tr>";
    });
    contenido += "</tbody>";
    contenido += "</table>";

    $("#" + divTablaModal).html(contenido);
    $("#tbModalMultiple").DataTable();
}

function Imprimir() {
    var contenidoTabla = '<h1>Reporte a Imprimir</h1>' + $('#myTable').prop("outerHTML");
    contenidoTabla = contenidoTabla.replace($('#tcheck').prop("outerHTML"), "");
    var ventana = window.open();
    var pagina = $(document.body);
    ventana.document.write(contenidoTabla);
    ventana.print();
    ventana.close();
    $(document.body) = pagina;
}

$(document).ready(function () {
    $('#myTable').DataTable();
});

function Exportar(tipoReporte,controller) {
    if (tipoReporte == "Pdf") {

        var anombrePropiedades = [];

        $("[name=nombrePropiedades]:checked").each(function () {
            anombrePropiedades.push($(this).val());
        });

        $.ajax(
            {
                type: "POST",
                url: controller + '/exportarPdfDatos',
                data: { nombrePropiedades: anombrePropiedades },
                success: function (data) {
                    var a = document.createElement("a");
                    a.href = data;
                    a.download = "reporte.pdf";
                    a.click();
                }
            }
        );
    }
    else {
        $("#tipoReporte").val(tipoReporte);
        $("#formExcel").submit();
    }
}

function AsignarPersona(id, valorPropiedadMostrar) {
    $("#txtIdPersonaPopUp").val(id);
    $("#txtNombrePersonaPopUp").val(valorPropiedadMostrar);
    $("#btnCerrarModal").click();
}


function AsignarNombre(controller, id, valorPropiedadMostrar) {
    $("#txtId" + controller + "PopUp").val(id);
    $("#txtNombre" + controller + "PopUp").val(valorPropiedadMostrar);
    $("#btnCerrarModal").click();
}


function LimpiarDatos() {
    $('.form-control').each(function () {
        $(this).val("");
    }
    );
}